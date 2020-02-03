using Rail.Controls;
using Rail.Misc;
using Rail.Trigonometry;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Model
{
    //[DebuggerDisplay("RailDockPoint Id={Id} DockPointIndex={DebugDockPointIndex} RailIndex={DebugRailIndex}")]
    //[DebuggerDisplay("RailDockPoint Id={Id} DockedWith={DockedWithId}")]
    [DebuggerDisplay("RailDockPoint Id={DebugRailIndex}/{DebugDockPointIndex} DockedWith={DockedWith.DebugRailIndex}/{DockedWith.DebugDockPointIndex}")]
    public class RailDockPoint
    {
        //private TrackDockPoint trackDockPoint;
        protected static readonly Pen dockPen = new Pen(Brushes.Blue, 1);
        protected static readonly Pen positionPen = new Pen(Brushes.Red, 2);

        // no serialization, will be set from RailBase
        private Angle angle;
        private Point position;

        public RailDockPoint()
        { }

        public RailDockPoint(RailBase railItem, TrackDockPoint trackDockPoint)
        {
            this.Id = Guid.NewGuid();
            this.RailItem = this.RailItemIntern = railItem;
            //this.trackDockPoint = trackDockPoint;
            this.DebugDockPointIndex = trackDockPoint.DebugIndex;
            this.DockType = trackDockPoint.DockType;

            this.angle = trackDockPoint.Angle;
            this.position = trackDockPoint.Position;
        }

        public RailDockPoint(RailGroup railGroup, RailBase railItem,  TrackDockPoint trackDockPoint)
        {
            this.Id = Guid.NewGuid();
            this.RailItem = railGroup;
            this.RailItemIntern = railItem;
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
            this.RailItem = this.RailItemIntern = railItem;
            //this.trackDockPoint = trackDockPoint;
            this.DebugDockPointIndex = trackDockPoint.DebugIndex;
            this.DockType = trackDockPoint.DockType;

            this.angle = trackDockPoint.Angle;
            this.position = trackDockPoint.Position;
        }

        public void Update(RailGroup railGroup, RailBase railItem, TrackDockPoint trackDockPoint)
        {
            this.RailItem = railGroup;
            this.RailItemIntern = railItem;
            //this.trackDockPoint = trackDockPoint;
            this.DebugDockPointIndex = trackDockPoint.DebugIndex;
            this.DockType = trackDockPoint.DockType;

            this.angle = trackDockPoint.Angle;
            this.position = trackDockPoint.Position;
        }

        public void Group(RailGroup railGroup)
        {
            this.RailItem = railGroup;
            // TODO move and rotate
        }

        public void Ungroup()
        {
            this.RailItem = this.RailItemIntern;
            // TODO move and rotate
        }

        [XmlAttribute("Id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Reference to RailItem or RailGroupBase 
        /// </summary>
        /// <remarks>
        /// For external handling like graph operations.
        /// If this is a docking point between a group and extern, this is the group
        /// </remarks>
        [XmlIgnore, JsonIgnore]
        public RailBase RailItem { get; private set; }

        /// <summary>
        /// Reference to RailItem of RailGroupItemBase
        /// </summary>
        /// <remarks>
        /// For internal use like drawing.
        /// If this is a docking point between a group and extern, this is the item inside the group
        /// </remarks>
        [XmlIgnore, JsonIgnore]
        private RailBase RailItemIntern { get; set; }

        [XmlIgnore, JsonIgnore]
        public int DebugDockPointIndex { get; set; }

        [XmlIgnore, JsonIgnore]
        public int DebugRailIndex { get { return this.RailItem.DebugIndex; } }

        [XmlIgnore, JsonIgnore]
        public string DebugOutput { get { return $"({DebugRailIndex},{DebugDockPointIndex})"; } }

        [XmlIgnore, JsonIgnore]
        public Point Position { get { return this.RailItemIntern.Position + (Vector)this.position.Rotate(this.RailItemIntern.Angle); } }

        [XmlIgnore, JsonIgnore]
        public Angle Angle { get { return this.RailItemIntern.Angle + this.angle;  } }

        [XmlIgnore, JsonIgnore]
        public string DockType { get; private set; }

        [XmlIgnore, JsonIgnore]
        public Guid Layer { get { return this.RailItem.Layer; } }

        [XmlAttribute("DockedWithId")]
        public Guid DockedWithId { get; set; }

        [XmlIgnore, JsonIgnore]
        public RailDockPoint DockedWith { get; private set; }
        
        [XmlIgnore, JsonIgnore]
        public bool IsDocked {  get { return this.DockedWith != null; } }

        public RailDockPoint Clone(RailBase railItem)
        {
            var clone = new RailDockPoint()
            {
                Id = Guid.NewGuid(),
                RailItem = railItem,
                RailItemIntern = railItem,
                DebugDockPointIndex = this.DebugDockPointIndex,
                DockType = this.DockType,
                angle = this.angle,
                position = this.position
                // at this time a cone has no dock
            };

            if (IsDocked)
            {
                DockPointCloneDictionary.Add(this, clone);
            }
            
            return clone;
        }

        public RailDockPoint Clone(RailGroup railGroup, RailBase railItem)
        {
            var clone = new RailDockPoint()
            {
                Id = Guid.NewGuid(),
                RailItem = railGroup,
                RailItemIntern = railItem,
                DebugDockPointIndex = this.DebugDockPointIndex,
                DockType = this.DockType,
                angle = this.angle,
                position = this.position
                // at this time a cone has no dock
            };

            if (IsDocked)
            {
                DockPointCloneDictionary.Add(this, clone);
            }

            return clone;
        }

        public void Draw(DrawingContext drawingContext)
        {
            double spacing = ((RailItem)this.RailItemIntern).Track.RailSpacing;
            drawingContext.DrawEllipse(null, dockPen, this.Position, spacing / 2, spacing / 2);
            if (!this.IsDocked)
            {
                drawingContext.DrawLine(positionPen, this.Position, this.Position.Circle(this.Angle, spacing));
            }
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
            return this.Distance(pos) < ((RailItem)this.RailItemIntern).Track.RailSpacing;
        }

        public bool IsInside(RailDockPoint p)
        {
            return this.Distance(p.Position) < ((RailItem)this.RailItemIntern).Track.RailSpacing;
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

        #region Clone

        public static Dictionary<RailDockPoint, RailDockPoint> DockPointCloneDictionary = new Dictionary<RailDockPoint, RailDockPoint>();

        public static void CloneDockPointLinks()
        {
            
            DockPointCloneDictionary.ForEach(d =>
            {
                var oldDP = d.Key;
                var newDP = d.Value;
                var docked = DockPointCloneDictionary[oldDP.DockedWith];
                
                newDP.DockedWithId = docked.Id;
                newDP.DockedWith = docked;
                
            });
            DockPointCloneDictionary.Clear();
        }

        #endregion
    }
}
