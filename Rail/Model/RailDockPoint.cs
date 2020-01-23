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
        //private TrackDockPoint trackDockPoint;

        private Angle angle;
        private Point position;

        public RailDockPoint()
        { }

        public RailDockPoint(RailBase railItem, TrackDockPoint trackDockPoint)
        {
            this.Id = Guid.NewGuid();
            this.RailItem = railItem;
            //this.trackDockPoint = trackDockPoint;
            this.DebugDockPointIndex = trackDockPoint.DebugIndex;
            this.DockType = trackDockPoint.DockType;

            this.angle = trackDockPoint.Angle;
            this.position = trackDockPoint.Position;
        }

        /// <summary>
        /// Called on load
        /// </summary>
        /// <param name="railItem"></param>
        /// <param name="trackDockPoint"></param>
        public void Update(RailBase railItem, TrackDockPoint trackDockPoint)
        {
            this.RailItem = railItem;
            this.RailItem = railItem;
            //this.trackDockPoint = trackDockPoint;
            this.DebugDockPointIndex = trackDockPoint.DebugIndex;
            this.DockType = trackDockPoint.DockType;

            this.angle = trackDockPoint.Angle;
            this.position = trackDockPoint.Position;
        }

        [XmlAttribute("Id")]
        public Guid Id { get; set; }

        [XmlIgnore]
        public RailBase RailItem { get; private set; }

        [XmlIgnore]
        public int DebugDockPointIndex { get; set; }

        [XmlIgnore]
        public int DebugRailIndex { get { return this.RailItem.DebugIndex; } }

        [XmlIgnore]
        public string DebugOutput { get { return $"({DebugRailIndex},{DebugDockPointIndex})"; } }

        [XmlIgnore]
        public Point Position { get { return this.RailItem.Position + (Vector)this.position.Rotate(this.RailItem.Angle); } }

        [XmlIgnore]
        public Angle Angle { get { return this.RailItem.Angle + this.angle;  } }

        [XmlIgnore]
        public string DockType { get; private set; }

        [XmlIgnore]
        public Guid Layer { get { return this.RailItem.Layer; } }

        [XmlAttribute("DockedWithId")]
        public Guid DockedWithId { get; set; }

        [XmlIgnore]
        public RailDockPoint DockedWith { get; private set; }
        
        [XmlIgnore]
        public bool IsDocked {  get { return this.DockedWith != null; } }
        
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
            return this.Distance(pos) < ((RailItem)this.RailItem).Track.RailSpacing;
        }

        public bool IsInside(RailDockPoint p)
        {
            return this.Distance(p.Position) < ((RailItem)this.RailItem).Track.RailSpacing;
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
