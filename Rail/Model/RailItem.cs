using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Model
{
    [DebuggerDisplay("RailItem Id={Id} X={Position.X} Y={Position.Y} A={Angle}")]

    public class RailItem
    {
        public RailItem()
        { }

        public RailItem(TrackBase track, Point pos)
        {
            this.Id = track.Id;
            this.Track = track;
            this.Position = pos;
            this.Angle = 0.0;
            this.DockPoints = track.DockPoints.Select(dp => new RailDockPoint(dp).Move(pos)).ToArray();
        }

        //public RailItem(TrackBase track, double x, double y, double angle, int[] docks)
        //{
        //    this.Id = track.Id;
        //    this.Position.X = x;
        //    this.Position.Y = y;
        //    this.Angle = angle;
        //}

        [XmlAttribute("Id")]
        public string Id { get; set; }

        [XmlIgnore]
        public TrackBase Track { get; set; }

        [XmlIgnore]
        public RailDockPoint[] DockPoints { get; private set; }

        [XmlIgnore]
        public Point Position;

        [XmlAttribute("X")]
        public double X 
        {
            get { return this.Position.X; } 
            set { this.Position.X = value; }
        }

        [XmlAttribute("Y")]
        public double Y 
        {
            get { return this.Position.Y; }
            set { this.Position.Y = value; }
        }

        [XmlAttribute("Angle")]
        private double Angle { get; set; }

        [XmlArray("Docks")]
        [XmlArrayItem("Dock")]
        public RailDock[] Docks { get; set; }
                
        public void Move(Vector vec)
        {
            this.Position += vec;
            this.DockPoints.ToList().ForEach(dp => dp.Move(vec));
        }

        public void Rotate(double angle)
        {
            this.Angle += angle;
            this.DockPoints.ToList().ForEach(dp => dp.Rotate(angle, this.Position));
        }

        public void Rotate(double angle, Point center)
        {
            double a = angle * (Math.PI / 180.0);
            double sin = Math.Sin(a);
            double cos = Math.Cos(a);

            this.Angle += angle;
            this.Position = new Point(center.X + cos * (this.Position.X - center.X) - sin * (this.Position.Y - center.Y),
                                      center.Y + sin * (this.Position.X - center.X) + cos * (this.Position.Y - center.Y));

            this.DockPoints.ToList().ForEach(dp => dp.Rotate(angle, center));
        }

        public void Rotate(double angle, RailItem center)
        {
            Rotate(angle, center.Position);
        }

        public void DrawTrack(DrawingContext drawingContext)
        {
            TransformGroup transformGroup = new TransformGroup();
            transformGroup.Children.Add(new RotateTransform (this.Angle));
            transformGroup.Children.Add(new TranslateTransform(this.Position.X, this.Position.Y));
            drawingContext.PushTransform(transformGroup);

            this.Track.Render(drawingContext);

            drawingContext.Pop();
        }

        private static Pen dockPen = new Pen(Brushes.Blue, 1);
        private static Pen positionPen = new Pen(Brushes.Red, 2);

        private double Sin(double value)
        {
            return Math.Sin(value * Math.PI / 180.0);
        }

        private double Cos(double value)
        {
            return Math.Cos(value * Math.PI / 180.0);
        }

        public void DrawDockPoints(DrawingContext drawingContext)
        {
            //TransformGroup transformGroup = new TransformGroup();
            //transformGroup.Children.Add(new RotateTransform(this.Angle));
            //transformGroup.Children.Add(new TranslateTransform(this.Position.X, this.Position.Y));
            //drawingContext.PushTransform(transformGroup);


            //foreach (TrackDockPoint point in this.Track.DockPoints)
            //{
            //    drawingContext.DrawEllipse(null, dockPen, point, 3.0, 3.0);
            //    drawingContext.DrawLine(positionPen, point, new Point(
            //        point.X + (Cos(point.Angle) * 16) - (Sin(point.Angle) * 16),
            //        point.Y + (Sin(point.Angle) * 16) + (Cos(point.Angle) * 16)));
            //}

            foreach (var point in this.DockPoints)
            {
                drawingContext.DrawEllipse(null, dockPen, point.Position, 3.0, 3.0);
                drawingContext.DrawLine(positionPen, point.Position, new Point(
                    point.X + (Cos(point.Angle) * 16) - (Sin(point.Angle) * 16),
                    point.Y + (Sin(point.Angle) * 16) + (Cos(point.Angle) * 16)));
            }

            //drawingContext.Pop();
        }

        public bool IsInside(Point point)
        {
            TransformGroup grp = new TransformGroup();
            grp.Children.Add(new TranslateTransform(this.Position.X, this.Position.Y));
            grp.Children.Add(new RotateTransform(this.Angle, this.Position.X, this.Position.Y));

            Geometry geometry = this.Track.Geometry.Clone();
            geometry.Transform = grp;
            bool f = geometry.FillContains(point);
            return f;
        }
    }
}
