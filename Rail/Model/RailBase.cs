using Rail.Controls;
using Rail.Misc;
using Rail.Trigonometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Model
{
    public abstract class RailBase
    {
        //protected static readonly Pen dockPen = new Pen(Brushes.Blue, 1);
        //protected static readonly Pen positionPen = new Pen(Brushes.Red, 2);
        protected static int globalDebugIndex = 0;

        [XmlIgnore]
        public int DebugIndex { get; protected set; }

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

        [XmlIgnore]
        public Angle Angle { get; set; }

        [XmlAttribute("Angle")]
        public double AngleInt
        {
            get { return this.Angle; }
            set { this.Angle = value; }
        }

        [XmlAttribute("Gradient")]
        public double Gradient { get; set; }

        [XmlAttribute("Height")]
        public double Height { get; set; }

        [XmlAttribute("Layer")]
        public Guid Layer { get; set; }

        [XmlArray("DockPoints")]
        [XmlArrayItem("DockPoint")]
        public List<RailDockPoint> DockPoints { get; set; }

        [XmlIgnore]
        public bool IsSelected { get; set; }

        [XmlIgnore]
        public bool HasOnlyOneDock { get { return this.DockPoints.One(dp => dp.IsDocked); } }

        [XmlIgnore]
        public bool HasDocks { get { return this.DockPoints.Any(dp => dp.IsDocked); } }

        [XmlIgnore]
        public abstract List<TrackMaterial> Materials { get; }


        public void CopyTo(RailBase railBase)
        {
            railBase.DebugIndex = this.DebugIndex;
            railBase.Position = this.Position;
            railBase.Angle = this.Angle;
            railBase.Gradient = this.Gradient;
            railBase.Height = this.Height;
            railBase.Layer = this.Layer;
            railBase.DockPoints = this.DockPoints;
        }

        [XmlIgnore]
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
        
        public abstract bool IsInside(Point point, RailViewMode viewMode);
               
        public abstract bool IsInside(Rect rec, RailViewMode viewMode);

        public virtual void Move(Vector vec)
        {
            this.Position += vec;
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
    }
}
