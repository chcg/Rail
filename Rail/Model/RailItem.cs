using Rail.Controls;
using Rail.Misc;
using Rail.Mvvm;
using Rail.Trigonometry;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Model
{
    [DebuggerDisplay("RailItem Index={DebugIndex} Id={Id} X={Position.X} Y={Position.Y} A={Angle}")]

    public class RailItem
    {
        private static int globalDebugIndex = 0;
        private Angle angle;

        public RailItem(TrackBase track, Point pos, ushort layer) 
        {
            this.DebugIndex = globalDebugIndex++;
            this.Id = track.Id;
            this.Track = track;
            this.Position = pos;
            this.Angle = 0.0;
            this.Layer = layer;
            this.DockPoints = track.DockPoints.Select(dp => new RailDockPoint(this, dp).Move(this.Position)).ToArray();
        }

        [XmlIgnore]
        public int DebugIndex { get; private set; }

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
        public Angle Angle
        {
            get
            {
                return this.angle;
            }
            set
            {
                this.angle = value;
                this.DockPoints.ForEach(dp => dp.Reset(this.Position, value));
            }
        }

        [XmlAttribute("Layer")]
        public ushort Layer { get; set; }

        //[XmlArray("Docks")]
        //[XmlArrayItem("Dock")]
        //public RailDock[] Docks { get; set; }

        [XmlIgnore]
        public bool IsSelected { get; set; }

        [XmlIgnore]
        public bool HasOnlyOneDock { get { return this.DockPoints.One(dp => dp.IsDocked);  } }

        [XmlIgnore]
        public bool HasDocks { get { return this.DockPoints.Any(dp => dp.IsDocked); } }

        public void Move(Vector vec)
        {
            this.Position += vec;
            this.DockPoints.ForEach(dp => dp.Move(vec));
        }

        public void Rotate(Angle angle)
        {
            Debug.WriteLine($"++Rotate {this.DebugIndex} RailItemAngle {this.Angle} RotateAngle {angle}");
            this.Angle += angle;
            this.DockPoints.ToList().ForEach(dp => dp.Rotate(angle, this.Position));
            Debug.WriteLine($"--Rotate {this.DebugIndex} RailItemAngle {this.Angle}");
        }

        public void Rotate(Angle angle, Point center)
        {
            this.Angle += angle;
            this.Position = this.Position.Rotate(angle, center);
            this.DockPoints.ToList().ForEach(dp => dp.Rotate(angle, center));
        }

        public void Rotate(Angle angle, RailItem center)
        {
            Rotate(angle, center.Position);
        }

        public void DrawRailItem(DrawingContext drawingContext, RailViewMode viewMode)
        {
            TransformGroup transformGroup = new TransformGroup();
            transformGroup.Children.Add(new RotateTransform (this.Angle));
            transformGroup.Children.Add(new TranslateTransform(this.Position.X, this.Position.Y));
            drawingContext.PushTransform(transformGroup);

            //Debug.WriteLine($"DrawRailItem {this.IsSelected}");
            this.Track.Render(drawingContext, viewMode, this.IsSelected);

            

            drawingContext.Pop();

            DrawDebug(drawingContext);
            DrawDebugDogpoints(drawingContext);
        }

        [Conditional("DEBUGINFO")]
        public void DrawDebug(DrawingContext drawingContext)
        {
            drawingContext.DrawRectangle(Brushes.White, new Pen(Brushes.Blue, 1), new Rect(this.Position, new Size(32, 20)));
            drawingContext.DrawText(new FormattedText(this.DebugIndex.ToString(), CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface("Verdana"), 16, Brushes.Blue, 1.25), this.Position);
        }

        [Conditional("DEBUGINFO")]
        public void DrawDebugDogpoints(DrawingContext drawingContext)
        {
            double width = 48;
            foreach (var dp in this.DockPoints)
            {
                Point pos = dp.Position - new Vector((dp.Angle < 45 || dp.Angle > 225 ? width : 0), (dp.Angle < 135 || dp.Angle > 315 ? 20 : 0));
                drawingContext.DrawRectangle(Brushes.White, new Pen(Brushes.Blue, 1), new Rect(pos, new Size(width, 20)));
                string str = $"{dp.DebugIndexDockPoint}-{dp.DockedWith?.DebugIndexRail},{dp.DockedWith?.DebugIndexDockPoint}";
                drawingContext.DrawText(new FormattedText(str, CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface("Verdana"), 16, Brushes.Blue, 1.25), pos);
            }
        }

        private static Pen dockPen = new Pen(Brushes.Blue, 1);
        private static Pen positionPen = new Pen(Brushes.Red, 2);
        
        public void DrawDockPoints(DrawingContext drawingContext)
        {
            foreach (var point in this.DockPoints)
            {
                drawingContext.DrawEllipse(null, dockPen, point.Position, this.Track.RailSpacing / 2, this.Track.RailSpacing / 2);
                if (!point.IsDocked)
                {
                    drawingContext.DrawLine(positionPen, point.Position, point.Position.Circle(point.Angle, this.Track.RailSpacing));
                }
            }
        }

        public bool IsInside(Point point, RailViewMode viewMode)
        {
            TransformGroup grp = new TransformGroup();
            grp.Children.Add(new TranslateTransform(this.Position.X, this.Position.Y));
            grp.Children.Add(new RotateTransform(this.Angle, this.Position.X, this.Position.Y));

            Geometry geometry = viewMode == RailViewMode.Tracks ? this.Track.GeometryTracks.Clone() : this.Track.GeometryRail.Clone();
            geometry.Transform = grp;
            bool f = geometry.FillContains(point);
            return f;
        }

        public bool IsInside(Rect rec, RailViewMode viewMode)
        {
            TransformGroup grp = new TransformGroup();
            grp.Children.Add(new TranslateTransform(this.Position.X, this.Position.Y));
            grp.Children.Add(new RotateTransform(this.Angle, this.Position.X, this.Position.Y));

            Geometry geometry = viewMode == RailViewMode.Tracks ? this.Track.GeometryTracks.Clone() : this.Track.GeometryRail.Clone();
            geometry.Transform = grp;
            bool f = rec.Contains(geometry.Bounds);
            return f;
        }

        public void UndockAll()
        {
            foreach (RailDockPoint dockPoint in this.DockPoints.Where(d => d.IsDocked))
            {
                dockPoint.Undock();
            }
        }


        /// <summary>
        /// Find subgraph without railItem.
        /// </summary>
        /// <param name="railItem">Item to find subgraph from.</param>
        /// <returns>List with rail items.</returns>
        //public List<RailItem> FindSubgraph()
        //{
        //    List<RailItem> railItems = new List<RailItem>();
        //    FindSubgraphRecursive(railItems, this);
        //    railItems.Remove(this);
        //    return railItems;
        //}

        //private static void FindSubgraphRecursive(List<RailItem> railItems, RailItem railItem)
        //{
        //    var dockedItems = railItem.DockPoints.Where(d => d.IsDocked).Select(d => d.DockedWith.RailItem).ToList();
        //    foreach (RailItem item in dockedItems)
        //    {
        //        if (!railItems.Contains(item))
        //        {
        //            railItems.Add(item);
        //            FindSubgraphRecursive(railItems, item);
        //        }
        //    }
        //}

        public List<RailItem> FindSubgraph()
        {
            // list with new items not inspected
            List<RailItem> listFound = new List<RailItem>();
            // list with inspected items
            List<RailItem> listScanned = new List<RailItem>();

            // add start item
            listFound.Add(this);

            RailItem item;
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
            listScanned.Remove(this);
            return listScanned;
        }
    }
}
