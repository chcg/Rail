using Rail.Misc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Rail.Model
{
    public class RailDockPoint
    {
        private RailItem railItem;
        private Point position;
        private Angle angle;
        private string dockType;

        public RailDockPoint(RailItem railItem, TrackDockPoint trackDockPoint)
        {
            this.railItem = railItem;
            this.position = trackDockPoint.Position;
            this.angle = trackDockPoint.Angle;
            this.dockType = trackDockPoint.DockType;
        }

        public Point Position { get { return this.position; } }

        public double X {  get { return this.position.X; } }

        public double Y { get { return this.position.Y; } }

        public Angle Angle { get { return this.angle; } }

        public string DockType { get { return this.dockType; } }

        public ushort Layer { get { return this.railItem.Layer; } }

        public RailDockPoint DockedWith { get; set; }

        public bool IsDocked {  get { return this.DockedWith != null; } }

        public RailItem RailItem { get { return this.railItem; } }

        public void Move(Vector vec)
        {
            this.position += vec;
        }

        public RailDockPoint Move(Point pos)
        {
            this.position += (Vector)pos;
            return this;
        }

        public void Rotate(Angle angle)
        {
            this.angle += angle;
        }

        public void Rotate(Angle angle, Point center)
        {
            Rotate(angle);
            this.position = this.position.Rotate(angle, center);
        }

        public double Distance(Point p)
        {
            return this.position.Distance(p);
        }

        public double Distance(RailDockPoint p)
        {
            return this.position.Distance(p.Position);
        }
    }
}
