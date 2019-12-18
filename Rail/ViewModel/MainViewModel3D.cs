using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Media3D;

namespace Rail.ViewModel
{
    public partial class MainViewModel
    {
        public void Update3D()
        {
            TriangleNet.Geometry.Polygon polygon = new TriangleNet.Geometry.Polygon();
            polygon.Points.AddRange(this.RailPlan.PlatePoints.Select(p => new TriangleNet.Geometry.Vertex(p.X, p.Y)));
            TriangleNet.Mesh mesh = (TriangleNet.Mesh)(new TriangleNet.Meshing.GenericMesher()).Triangulate(polygon, null, null);

            List<TriangleNet.Topology.Triangle> triangles = mesh.Triangles.ToList();

            double centerX = this.RailPlan.PlatePoints.Max(p => p.X) / 2.0;
            double centerY = this.RailPlan.PlatePoints.Max(p => p.Y) / 2.0;

            MeshGeometry3D plateGeometry3D = new MeshGeometry3D();
            //foreach (Point point in this.RailPlan.PlatePoints)
            //{

            //    plateGeometry3D.Positions.Add(new Point3D(point.X - centerX, point.Y - centerY, 0));
            //    plateGeometry3D.Normals.Add(new Vector3D(0, 0, 1));
            //}

            plateGeometry3D.Positions.Add(new Point3D(-1000, -1000, 0));
            plateGeometry3D.Positions.Add(new Point3D(+1000, -1000, 0));
            plateGeometry3D.Positions.Add(new Point3D(-1000, +1000, 0));
            plateGeometry3D.Positions.Add(new Point3D(+1000, +1000, 0));

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
    }
}
