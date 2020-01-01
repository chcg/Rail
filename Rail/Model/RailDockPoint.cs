using Rail.Misc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;

namespace Rail.Model
{
    public class RailDockPoint
    {

        private TrackDockPoint trackDockPoint;

        public RailDockPoint(RailItem railItem, TrackDockPoint trackDockPoint)
        {
            this.RailItem = railItem;
            this.trackDockPoint = trackDockPoint;
            this.DebugIndexDockPoint = trackDockPoint.DebugIndex;
            this.Position = trackDockPoint.Position;
            this.Angle = trackDockPoint.Angle;
            this.DockType = trackDockPoint.DockType;
        }

        public void Reset(Point position, Angle angle)
        {
            Debug.WriteLine($"Reset {this.RailItem.DebugIndex} position {position} angle {Angle}");
            this.Position = this.trackDockPoint.Position.Rotate(angle) + (Vector)position;
            this.Angle = this.trackDockPoint.Angle + angle;
        }

        public RailItem RailItem { get; private set; }

        public int DebugIndexDockPoint { get; private set; }

        public int DebugIndexRail { get { return this.RailItem.DebugIndex; } }

        public string DebugOutput { get { return $"({DebugIndexRail},{DebugIndexDockPoint})"; } }

        public Point Position { get; private set; }

        public Angle Angle { get; private set;}

        public string DockType { get; private set; }

        public ushort Layer { get { return this.RailItem.Layer; } }

        public RailDockPoint DockedWith { get; set; }

        public bool IsDocked {  get { return this.DockedWith != null; } }


        public void Move(Vector vec)
        {
            this.Position += vec;
        }

        public RailDockPoint Move(Point pos)
        {
            this.Position += (Vector)pos;
            return this;
        }

        public void Rotate(Angle angle)
        {
            this.Angle += angle;
        }

        public void Rotate(Angle angle, Point center)
        {
            Rotate(angle);
            this.Position = this.Position.Rotate(angle, center);
        }

        public double Distance(Point p)
        {
            return this.Position.Distance(p);
        }

        public double Distance(RailDockPoint p)
        {
            return this.Position.Distance(p.Position);
        }

        public bool IsInside(Point pos)
        {
            return this.Distance(pos) < this.RailItem.Track.RailSpacing;
        }

        public bool IsInside(RailDockPoint p)
        {
            return this.Distance(p.Position) < this.RailItem.Track.RailSpacing;
        }

        public void Dock(RailDockPoint dockTo)
        {
            Debug.WriteLine($"Dock {this.DebugOutput} to {dockTo.DebugOutput}");
            this.DockedWith = dockTo;
            dockTo.DockedWith = this;

            Angle rotate = dockTo.Angle - this.Angle - new Angle(180);
            Debug.WriteLine($"Dock {this.Angle} op {dockTo.Angle} = {rotate}");

            var sub = this.RailItem.FindSubgraph();
            foreach (RailItem rt in sub)
            {
                //rt.Angle += rotate;
                //rt.Position = track.Position.Rotate(rotate, dp);
            }
            this.RailItem.Rotate(rotate);
            this.RailItem.Move(dockTo.Position - this.Position);
        }

        public void Undock()
        {
            this.DockedWith.DockedWith = null;
            this.DockedWith = null;
        }
    }
}
