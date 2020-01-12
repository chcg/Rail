using Rail.Trigonometry;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace Rail.Model
{
    public class RailDockPoint
    {
        private TrackDockPoint trackDockPoint;

        public RailDockPoint(RailItem railItem, TrackDockPoint trackDockPoint, Point pos)
        {
            this.RailItem = railItem;
            this.trackDockPoint = trackDockPoint;
            this.DebugIndexDockPoint = trackDockPoint.DebugIndex;
            this.Position = trackDockPoint.Position + (Vector)pos;
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

        //public RailDockPoint Move(Point pos)
        //{
        //    this.Position += (Vector)pos;
        //    return this;
        //}

        public void Rotate(Angle angle)
        {
            this.Angle += angle;
        }

        public void Rotate(Rotation rotation)
        {
            this.Angle += rotation;
        }

        public void Rotate(Angle angle, Point center)
        {
            Rotate(angle);
            this.Position = this.Position.Rotate(angle, center);
        }

        public void Rotate(Rotation rotation, Point center)
        {
            Rotate(rotation);
            this.Position = this.Position.Rotate(rotation, center);
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


            //Rotation rotate = ((Rotation)dockTo.Angle) - ((Rotation)this.Angle) - ((Rotation)new Angle(180));
            Rotation rotate = dockTo.Angle - this.Angle;
            rotate -= new Angle(180.0);
            Debug.WriteLine($"Dock {this.Angle} op {dockTo.Angle} = {rotate}");
            
            var subgraph = this.RailItem.FindSubgraph();
            subgraph.ForEach(i => i.Rotate(rotate, this.RailItem.Position));
            Vector move = dockTo.Position - this.Position;
            subgraph.ForEach(i => i.Move(move));

            this.DockedWith = dockTo;
            dockTo.DockedWith = this;
        }

        public void Undock()
        {
            this.DockedWith.DockedWith = null;
            this.DockedWith = null;
        }
    }
}
