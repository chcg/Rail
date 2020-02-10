using Rail.Controls;
using Rail.Misc;
using Rail.Trigonometry;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Model
{
    public abstract class RailBase
    {
        protected readonly Pen linePen = new Pen(TrackBrushes.TrackFrame, 2);
        protected readonly Pen dotPen = new Pen(Brushes.White, 2) { DashStyle = DashStyles.Dot };

        //protected static readonly Pen dockPen = new Pen(Brushes.Blue, 1);
        //protected static readonly Pen positionPen = new Pen(Brushes.Red, 2);
        protected static int globalDebugIndex = 0;

        public RailBase()
        { }
        
        //public RailBase(RailBase railBase)
        //{
        //    this.DebugIndex = globalDebugIndex++;
        //    this.Position = railBase.Position;
        //    this.Angle = railBase.Angle;
        //    this.Layer = railBase.Layer;
        //    this.DockPoints = railBase.DockPoints;
        //    this.DockPoints.ForEach(d => d.Group(this));
        //}

        [XmlIgnore, JsonIgnore]
        public int DebugIndex { get; protected set; }

        [XmlElement("Position")]
        [JsonPropertyName("Position")]
        public Point Position { get; set; }

        [XmlElement("Angle")]
        [JsonPropertyName("Angle")]
        public Angle Angle { get; set; }

        [XmlElement("Layer")]
        public Guid Layer { get; set; }

        [XmlArray("DockPoints")]
        [XmlArrayItem("DockPoint")]
        public List<RailDockPoint> DockPoints { get; set; }

        [XmlIgnore, JsonIgnore]
        public bool IsSelected { get; set; }

        [XmlIgnore, JsonIgnore]
        public List<RailDockPoint> FreeDockPoints {  get { return DockPoints.Where(d => !d.IsDocked).ToList(); } }
        
        [XmlIgnore, JsonIgnore]
        public bool HasOnlyOneDock { get { return this.DockPoints.One(dp => dp.IsDocked); } }

        [XmlIgnore, JsonIgnore]
        public bool HasDocks { get { return this.DockPoints.Any(dp => dp.IsDocked); } }

        [XmlIgnore, JsonIgnore]
        public abstract List<TrackMaterial> Materials { get; }

        public abstract RailBase Clone();

        [XmlIgnore, JsonIgnore]
        public Transform RailTransform
        {
            get
            {
                TransformGroup transformGroup = new TransformGroup();
                transformGroup.Children.Add(new RotateTransform(this.Angle));
                transformGroup.Children.Add(new TranslateTransform(this.Position.X, this.Position.Y));
                return transformGroup;
            }
        }

        public abstract void DrawRailItem(DrawingContext drawingContext, RailViewMode viewMode, RailLayer layer);

        public void DrawDockPoints(DrawingContext drawingContext)
        {
            this.DockPoints.ForEach(d => d.Draw(drawingContext));
        }

        protected abstract Geometry GetGeometry(RailViewMode viewMode);

        public virtual bool IsInside(Point point, RailViewMode viewMode)
        {
            Geometry geometry = GetGeometry(viewMode);
            bool f = geometry.FillContains(point);
            return f;
        }

        public virtual bool IsInside(Rect rec, RailViewMode viewMode)
        {
            Geometry geometry = GetGeometry(viewMode);
            bool f = rec.Contains(geometry.Bounds);
            return f;
        }

        public virtual RailBase Move(Vector vec)
        {
            this.Position += vec;
            return this;
        }

        public virtual void Rotate(Rotation rotation, Point center)
        {
            this.Angle += rotation;
            this.Position = this.Position.Rotate(rotation, center);
        }

        public void UndockAll()
        {
            foreach (RailDockPoint dockPoint in this.DockPoints.Where(d => d.IsDocked))
            {
                dockPoint.Undock();
            }
        }

        /// <summary>
        /// Find docked subgraph including this one.
        /// </summary>
        /// <returns>List of all docked rail items.</returns>
        public List<RailBase> FindSubgraph()
        {
            // list with new items not inspected
            List<RailBase> listFound = new List<RailBase>();
            // list with inspected items
            List<RailBase> listScanned = new List<RailBase>();

            // add start item
            listFound.Add(this);

            RailBase item;
            while ((item = listFound.TakeLastOrDefault()) != null)
            {
                // move item from listFound to listScanned
                listScanned.Add(item);

                // check if children already in one of the lists and add to listFound if not
                listFound.AddRange(item.DockPoints.
                    Where(d => d.IsDocked).
                    Select(d => d.DockedWith.RailItem).
                    Where(d => (!listFound.Contains(d)) && (!listScanned.Contains(d))));
            }

            // remove original
            // listScanned.Remove(this);
            return listScanned;
        }

        #region Debug

        [Conditional("DEBUG")]
        public virtual void DebugInfo()
        {
            Debug.WriteLine($"{this.GetType().Name} DebugIndex={DebugIndex} Position={Position} Angle={Angle} DockPoints");
            Debug.Indent();
            if (this.DockPoints != null)
            {
                foreach (RailDockPoint dockPoint in this.DockPoints)
                {
                    dockPoint.DebugInfo();
                }
            }
            Debug.Unindent();
        }

        #endregion
    }
}
