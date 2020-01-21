using Rail.Trigonometry;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Xml.Serialization;

namespace Rail.Model
{
    public class RailDockPoint
    {
        private TrackDockPoint trackDockPoint;

        private Angle angle;
        private Point position;

        public RailDockPoint()
        { }

        public RailDockPoint(RailItem railItem, TrackDockPoint trackDockPoint, Point pos)
        {
            this.Id = Guid.NewGuid();
            this.RailItem = railItem;
            this.trackDockPoint = trackDockPoint;
            this.DebugDockPointIndex = trackDockPoint.DebugIndex;
            //this.Position = trackDockPoint.Position + (Vector)pos;
            //this.Angle = trackDockPoint.Angle;
            this.DockType = trackDockPoint.DockType;

            this.angle = trackDockPoint.Angle;
            this.position = trackDockPoint.Position;
        }

        /// <summary>
        /// Called on load
        /// </summary>
        /// <param name="railItem"></param>
        /// <param name="trackDockPoint"></param>
        public void Update(RailItem railItem, TrackDockPoint trackDockPoint)
        {
            this.RailItem = railItem;
            this.RailItem = railItem;
            this.trackDockPoint = trackDockPoint;
            this.DebugDockPointIndex = trackDockPoint.DebugIndex;
            this.DockType = trackDockPoint.DockType;

            this.angle = trackDockPoint.Angle;
            this.position = trackDockPoint.Position;
        }

        public void Reset(Point position, Angle angle)
        {
            Debug.WriteLine($"Reset {this.RailItem.DebugIndex} position {position} angle {Angle}");
            //this.Position = this.trackDockPoint.Position.Rotate(angle) + (Vector)position;
            //this.Angle = this.trackDockPoint.Angle + angle;
        }

        [XmlAttribute("Id")]
        public Guid Id { get; set; }

        [XmlIgnore]
        public RailItem RailItem { get; private set; }

        [XmlIgnore]
        public int DebugDockPointIndex { get; set; }

        [XmlIgnore]
        public int DebugRailIndex { get { return this.RailItem.DebugIndex; } }

        [XmlIgnore]
        public string DebugOutput { get { return $"({DebugRailIndex},{DebugDockPointIndex})"; } }

        [XmlIgnore]
        public Point Position { get { return this.RailItem.Position + (Vector)this.position + (Vector)this.position.Rotate(this.RailItem.Angle, this.RailItem.Position); } }

        //[XmlAttribute("X")]
        //public double X
        //{
        //    get { return this.Position.X; }
        //    set { this.Position.X = value; }
        //}

        //[XmlAttribute("Y")]
        //public double Y
        //{
        //    get { return this.Position.Y; }
        //    set { this.Position.Y = value; }
        //}

        //[XmlIgnore]
        //public Angle Angle { get; private set;}

        //[XmlAttribute("Angle")]
        //public double AngleInt
        //{
        //    get { return this.Angle; }
        //    set { this.Angle = value; }
        //}

        [XmlIgnore]
        public Angle Angle { get { return this.RailItem.Angle + this.angle;  } }

        [XmlIgnore]
        public string DockType { get; private set; }

        [XmlIgnore]
        public Guid Layer { get { return this.RailItem.Layer; } }

        [XmlAttribute("DockedWithId")]
        public Guid DockedWithId { get; set; }

        //[XmlAttribute("DockedWithIndex")]
        //public int DockedWithIndex { get; set; }

        //private RailDockPoint dockedWith;

        [XmlIgnore]
        public RailDockPoint DockedWith { get; private set; }
        
        [XmlIgnore]
        public bool IsDocked {  get { return this.DockedWith != null; } }


        public void Move(Vector vec)
        {
            //this.Position += vec;
        }

        //public RailDockPoint Move(Point pos)
        //{
        //    this.Position += (Vector)pos;
        //    return this;
        //}

        public void Rotate(Angle angle)
        {
            //this.Angle += angle;
        }

        public void Rotate(Rotation rotation)
        {
            //this.Angle += rotation;
        }

        public void Rotate(Angle angle, Point center)
        {
            Rotate(angle);
            //this.Position = this.Position.Rotate(angle, center);
        }

        public void Rotate(Rotation rotation, Point center)
        {
            Rotate(rotation);
            //this.Position = this.Position.Rotate(rotation, center);
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

        public void AdjustDock(RailDockPoint dockTo)
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

            Dock(dockTo);
        }

        public void Dock(RailDockPoint dockTo)
        {
            this.DockedWith = dockTo;
            this.DockedWithId = dockTo.Id;
            dockTo.DockedWith = this;
            dockTo.DockedWithId = this.Id;
        }

        public void Undock()
        {
            this.DockedWith.DockedWith = null;
            this.DockedWith.DockedWithId = Guid.Empty;
            this.DockedWith = null;
            this.DockedWithId = Guid.Empty;
        }
    }
}
