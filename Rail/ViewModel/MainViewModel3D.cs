using Rail.Controls;
using Rail.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace Rail.ViewModel
{
    public partial class MainViewModel
    {
        private readonly Pen blackPen = new Pen(Brushes.Black, 1);
        private readonly Brush plateBrush = new SolidColorBrush(Colors.Green);

        public void Update3D()
        {

            // render plate image
            DrawingVisual drawingVisual = new DrawingVisual();
            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            {
                drawingContext.DrawGeometry(plateBrush, blackPen, new PathGeometry(new PathFigureCollection
                {
                    new PathFigure(this.RailPlan.PlatePoints.FirstOrDefault(), new PathSegmentCollection
                    (
                        this.RailPlan.PlatePoints.Skip(1).Select(p => new LineSegment(p, true))
                    ), true)
                }));

                this.RailPlan.Rails.ForEach(r => r.DrawRailItem(drawingContext, RailViewMode.Terrain, this.RailPlan.Layers.FirstOrDefault(l => l.Id == r.Layer)));

            }
            RenderTargetBitmap bitmap = new RenderTargetBitmap(this.railPlan.Width, this.railPlan.Height, 96, 96, PixelFormats.Default);
            bitmap.Render(drawingVisual);
            this.PlateImage = bitmap;

            // create 3D plate
            Point3DCollection point3DCollection = new Point3DCollection()
            {
                new Point3D(-this.railPlan.Width / 2, -this.railPlan.Height / 2, 0),
                new Point3D(+this.railPlan.Width / 2, -this.railPlan.Height / 2, 0),
                new Point3D(-this.railPlan.Width / 2, +this.railPlan.Height / 2, 0),
                new Point3D(+this.railPlan.Width / 2, +this.railPlan.Height / 2, 0)
            };
            this.PlatePoint3DCollection = point3DCollection;
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
    }
}
