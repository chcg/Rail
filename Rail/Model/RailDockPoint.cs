using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Rail.Model
{
    public class RailDockPoint
    {
        private Point position;
        private double angle;

        public RailDockPoint(TrackDockPoint trackDockPoint)
        {
            this.position = trackDockPoint.Position;
            this.angle = trackDockPoint.Angle;
        }

        public Point Position { get { return this.position; } }

        public double X {  get { return this.position.X; } }

        public double Y { get { return this.position.Y; } }

        public double Angle { get { return this.angle; } }

        public void Move(Vector vec)
        {
            this.position += vec;
        }

        public RailDockPoint Move(Point pos)
        {
            this.position += (Vector)pos;
            return this;
        }

        public void Rotate(double angle)
        {
            this.angle += angle;
        }

        public void Rotate(double angle, Point center)
        {
            double a = angle *(Math.PI / 180.0);
            double sin = Math.Sin(a);
            double cos = Math.Cos(a);

            this.angle += angle;
            this.position = new Point(center.X + cos * (this.position.X - center.X) - sin * (this.position.Y - center.Y),
                                      center.Y + sin * (this.position.X - center.X) + cos * (this.position.Y - center.Y));
        }

        public double Distance(Point p)
        {
            return Math.Sqrt(Math.Pow(p.X - position.X, 2) + Math.Pow(p.Y - position.Y, 2));
        }

        public double Distance(RailDockPoint p)
        {
            return Math.Sqrt(Math.Pow(p.X - position.X, 2) + Math.Pow(p.Y - position.Y, 2));
        }
    }
}
