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
            RenderPlate();


            //TriangleNet.Geometry.Polygon polygon = new TriangleNet.Geometry.Polygon();
            //polygon.Points.AddRange(this.RailPlan.PlatePoints.Select(p => new TriangleNet.Geometry.Vertex(p.X, p.Y)));
            //TriangleNet.Mesh mesh = (TriangleNet.Mesh)(new TriangleNet.Meshing.GenericMesher()).Triangulate(polygon, null, null);

            //List<TriangleNet.Topology.Triangle> triangles = mesh.Triangles.ToList();

            //double centerX = this.RailPlan.PlatePoints.Max(p => p.X) / 2.0;
            //double centerY = this.RailPlan.PlatePoints.Max(p => p.Y) / 2.0;

            MeshGeometry3D plateGeometry3D = new MeshGeometry3D();
            
            plateGeometry3D.Positions.Add(new Point3D(-this.railPlan.Width / 2, -this.railPlan.Height / 2, 0));
            plateGeometry3D.Positions.Add(new Point3D(+this.railPlan.Width / 2, -this.railPlan.Height / 2, 0));
            plateGeometry3D.Positions.Add(new Point3D(-this.railPlan.Width / 2, +this.railPlan.Height / 2, 0));
            plateGeometry3D.Positions.Add(new Point3D(+this.railPlan.Width / 2, +this.railPlan.Height / 2, 0));

            plateGeometry3D.Normals.Add(new Vector3D(0, 0, 1));
            plateGeometry3D.Normals.Add(new Vector3D(0, 0, 1));
            plateGeometry3D.Normals.Add(new Vector3D(0, 0, 1));
            plateGeometry3D.Normals.Add(new Vector3D(0, 0, 1));

            plateGeometry3D.TextureCoordinates.Add(new Point(0, 1));
            plateGeometry3D.TextureCoordinates.Add(new Point(1, 1));
            plateGeometry3D.TextureCoordinates.Add(new Point(0, 0));
            plateGeometry3D.TextureCoordinates.Add(new Point(1, 0));

            plateGeometry3D.TriangleIndices.Add(0);
            plateGeometry3D.TriangleIndices.Add(1);
            plateGeometry3D.TriangleIndices.Add(2);

            plateGeometry3D.TriangleIndices.Add(1);
            plateGeometry3D.TriangleIndices.Add(3);
            plateGeometry3D.TriangleIndices.Add(2);

            this.PlateGeometry3D = plateGeometry3D;
        }

        private MeshGeometry3D plateGeometry3D;
        public MeshGeometry3D PlateGeometry3D
        {
            get
            {
                return this.plateGeometry3D;
            }
            set
            {
                this.plateGeometry3D = value;
                NotifyPropertyChanged("PlateGeometry3D");
            }
        }

        public void RenderPlate()
        {
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

                this.RailPlan.Rails.ForEach(r => r.DrawRailItem(drawingContext, RailViewMode.Rail));
                
            }
            RenderTargetBitmap bitmap = new RenderTargetBitmap(this.railPlan.Width, this.railPlan.Height, 96, 96, PixelFormats.Default);
            bitmap.Render(drawingVisual);

            this.PlateImage = bitmap;
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

        //public static BitmapSource CreateBitmap(int width, int height, double dpi, Action<DrawingContext> render)
        //{
        //    DrawingVisual drawingVisual = new DrawingVisual();
        //    using (DrawingContext drawingContext = drawingVisual.RenderOpen())
        //    {
        //        render(drawingContext);
        //    }
        //    RenderTargetBitmap bitmap = new RenderTargetBitmap(width, height, dpi, dpi, PixelFormats.Default);
        //    bitmap.Render(drawingVisual);
        //    return bitmap;
        //}
        
//BitmapSource image = ImageTools.CreateBitmap(
//            320, 240, 96,
//            drawingContext =>
//            {
//                drawingContext.DrawRectangle(
//                    Brushes.Green, null, new Rect(50, 50, 200, 100));
//                drawingContext.DrawLine(
//                    new Pen(Brushes.White, 2), new Point(0, 0), new Point(320, 240));
//            });
    }
}
