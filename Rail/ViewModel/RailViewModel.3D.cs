using Rail.Controls;
using Rail.Misc;
using Rail.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace Rail.ViewModel
{
    public partial class RailViewModel
    {
        private readonly Pen blackPen = new Pen(Brushes.Black, 1);
        private readonly Brush plateBrush = new SolidColorBrush(Colors.Green);

        public void Update3Dxxx()
        {

            // render plate image
            DrawingVisual drawingVisual = new DrawingVisual();
            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            {
                drawingContext.DrawGeometry(plateBrush, blackPen, new PathGeometry(new PathFigureCollection
                {
                    new PathFigure(this.railPlan.PlatePoints.FirstOrDefault(), new PathSegmentCollection
                    (
                        this.railPlan.PlatePoints.Skip(1).Select(p => new LineSegment(p, true))
                    ), true)
                }));

                this.railPlan.Rails.ForEach(r => r.DrawRailItem(drawingContext, RailViewMode.Terrain, this.railPlan.Layers.FirstOrDefault(l => l.Id == r.Layer)));

            }
            RenderTargetBitmap bitmap = new RenderTargetBitmap(this.Width, this.Height, 96, 96, PixelFormats.Default);
            bitmap.Render(drawingVisual);
            this.PlateImage = bitmap;

            // create 3D plate
            Point3DCollection point3DCollection = new Point3DCollection()
            {
                new Point3D(-this.Width / 2, -this.Height / 2, 0),
                new Point3D(+this.Width / 2, -this.Height / 2, 0),
                new Point3D(-this.Width / 2, +this.Height / 2, 0),
                new Point3D(+this.Width / 2, +this.Height / 2, 0)
            };
            this.PlatePoint3DCollection = point3DCollection;
        }

        public void Update3D()
        {
            this.Layers3D.Clear();

            double height = 0;
            this.railPlan.Layers.ForEach(l => { CreateLayer(l, height); height += l.Height; });
        }

        public Brush RenderLayer(RailLayer layer)
        {
            DrawingVisual drawingVisual = new DrawingVisual();
            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            {
                if (layer.PlateColor != Colors.Transparent)
                {
                    Color ground = layer.PlateColor;
                    ground.A = 150;
                    drawingContext.DrawGeometry(new SolidColorBrush(ground), blackPen, new PathGeometry(new PathFigureCollection
                    {
                        new PathFigure(this.railPlan.PlatePoints.FirstOrDefault(), new PathSegmentCollection
                        (
                            this.railPlan.PlatePoints.Skip(1).Select(p => new LineSegment(p, true))
                        ), true)
                    }));
                }

                this.railPlan.Rails.Where(r => r.Layer == layer.Id).ForEach(r => r.DrawRailItem(drawingContext, RailViewMode.Terrain, layer));

            }
            RenderTargetBitmap bitmap = new RenderTargetBitmap(this.Width, this.Height, 96, 96, PixelFormats.Default);
            bitmap.Render(drawingVisual);
            return new ImageBrush(bitmap);
        }

        public void CreateLayer(RailLayer layer, double heigth)
        {
            if (!layer.Show)
            {
                return;
            }
            GeometryModel3D model = new GeometryModel3D();

            // geometrie
            MeshGeometry3D geometrie= new MeshGeometry3D();
            geometrie.Positions = new Point3DCollection()
            {
                new Point3D(-this.Width / 2, -this.Height / 2, heigth),
                new Point3D(+this.Width / 2, -this.Height / 2, heigth),
                new Point3D(-this.Width / 2, +this.Height / 2, heigth),
                new Point3D(+this.Width / 2, +this.Height / 2, heigth)
            }; 
            geometrie.Normals = new Vector3DCollection(new Vector3D[] { new Vector3D(0, 0, 1), new Vector3D(0, 0, 1), new Vector3D(0, 0, 1), new Vector3D(0, 0, 1) });
            geometrie.TextureCoordinates = new PointCollection( new Point[] { new Point(0, 1), new Point(1, 1), new Point(0, 0), new Point(1, 0) });
            geometrie.TriangleIndices = new Int32Collection(new int[] { 0, 1, 2, 1, 3, 2 });
            model.Geometry = geometrie;

            // material
            model.Material = new DiffuseMaterial(RenderLayer(layer));

            // transform
            Transform3DGroup group = new Transform3DGroup();

            //ScaleTransform3D zoomTransformation = new ScaleTransform3D();
            //Binding zoomBinding = new Binding("ZoomFactor");
            //zoomBinding.Source = this.ZoomFactor;
            //BindingOperations.SetBinding(zoomTransformation, ScaleTransform3D.ScaleXProperty, zoomBinding);
            //BindingOperations.SetBinding(zoomTransformation, ScaleTransform3D.ScaleYProperty, zoomBinding);
            //BindingOperations.SetBinding(zoomTransformation, ScaleTransform3D.ScaleZProperty, zoomBinding);
            //group.Children.Add(zoomTransformation);
            group.Children.Add(new ScaleTransform3D(ZoomFactor, ZoomFactor, ZoomFactor));
            group.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), TiltAngle)));
            group.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), RotationAngle)));
            model.Transform = group;

            this.Layers3D.Add(model);
        }

        private Point3DCollection platePoint3DCollection;
        public Point3DCollection PlatePoint3DCollection
        {
            get
            {
                return this.platePoint3DCollection;
            }
            set
            {
                this.platePoint3DCollection = value;
                NotifyPropertyChanged(nameof(PlatePoint3DCollection));
            }
        }
        
        private ImageSource plateImage;
        public ImageSource PlateImage
        {
            get
            {
                return this.plateImage;
            }
            set
            {
                this.plateImage = value;
                NotifyPropertyChanged("PlateImage");
            }
        }

        private double rotationAngle = 0;
        public double RotationAngle
        {
            get
            {
                return this.rotationAngle;
            }
            set
            {
                this.rotationAngle = value;
                NotifyPropertyChanged(nameof(RotationAngle));
            }
        }

        private double tiltAngle = -45;
        public double TiltAngle
        {
            get
            {
                return this.tiltAngle;
            }
            set
            {
                this.tiltAngle = value;
                NotifyPropertyChanged(nameof(TiltAngle));
            }
        }

        private Model3DCollection layers3D = new Model3DCollection();
        public Model3DCollection  Layers3D 
        {
            get
            {
                return this.layers3D;
            }
            //set
            //{
            //    this.layers3D = value;
            //    NotifyPropertyChanged(nameof(Layers3D));
            //}
        }
    }
}
