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
    public class RailGroup : RailBase
    {
        public RailGroup()
        { }

        public RailGroup(IEnumerable<RailBase> railItems) 
        {
            this.DebugIndex = globalDebugIndex++;

            var extDockPoints = railItems.SelectMany(r => r.DockPoints).Where(d => !d.IsDocked || !railItems.Contains(d.DockedWith.RailItem)).ToList(); 

            RailItem firstRailItme = (RailItem)railItems.FirstOrDefault();
            this.Layer = firstRailItme.Layer;
            this.Position = firstRailItme.Position; // set before new RailGroupItem
            this.Angle = 0.0;

            railItems.DebugList($"RailGroup Const original rails");
            this.Rails = railItems.Cast<RailItem>().ToList();
            this.Rails.ForEach(r => r.IsSelected = false);

            extDockPoints.ForEach(d => d.Group(this));

            this.DockPoints = extDockPoints;

            DebugInfo();
                        
            this.IsSelected = true;
        }

        public IEnumerable<RailItem> Resolve()
        {
            this.DockPoints.ForEach(d => d.Ungroup());
            return this.Rails;
        }

        /// <summary>
        /// 
        /// </summary>
        [XmlArray("Rails")]
        [XmlArrayItem("Rail")]
        public List<RailItem> Rails { get; set; }

        [XmlIgnore, JsonIgnore]
        public override List<TrackMaterial> Materials
        {
            get { return this.Rails.SelectMany(r => r.Materials).ToList(); }
        }


        public override RailBase Clone()
        {
            var clone = new RailGroup()
            {
                DebugIndex = globalDebugIndex++,
                Position = this.Position,
                Angle = this.Angle,
                Layer = this.Layer,
                //DockPoints = this.DockPoints.Select(d => d.Clone()).ToList(),
                Rails = this.Rails.Select(r => (RailItem)r.Clone()).ToList()
                 
            };
            clone.DockPoints = this.DockPoints.Select(d => d.Clone(clone)).ToList();
            return clone;
        }

        public override void DrawRailItem(DrawingContext drawingContext, RailViewMode viewMode, RailLayer layer)
        {
            this.DockPoints.ForEach(d => d.DrawOpen(drawingContext));

            this.Rails.ForEach(r => r.DrawRailItem(drawingContext, viewMode, layer));

            if (this.IsSelected && viewMode < RailViewMode.Terrain)
            {
                var geometrie = GetGeometry(viewMode/*, null*/);
                drawingContext.DrawDrawing(new GeometryDrawing(null, this.linePen, geometrie));
                drawingContext.DrawDrawing(new GeometryDrawing(null, this.dotPen, geometrie));
            }
        }

        protected override Geometry GetGeometry(RailViewMode viewMode/*, Transform transform*/)
        {
            return this.Rails.Select(r =>
            {
                Geometry geometry = r.Track.TrackGeometry.Clone();
                geometry.Transform = r.RailTransform;
                return geometry;
            }).Aggregate((a, b) => new CombinedGeometry(GeometryCombineMode.Union, a, b));

        }

        public override bool IsInside(Point point, RailViewMode viewMode)
        {
            bool f = this.Rails.Any(r => r.IsInside(point, viewMode));
            return f;
        }

        public override bool IsInside(Rect rec, RailViewMode viewMode)
        {
            bool f = this.Rails.Any(r => r.IsInside(rec, viewMode));
            return f;
        }

        public override RailBase Move(Vector vec)
        {
            base.Move(vec);
            this.Rails.ForEach(r => r.Move(vec));
            return this;
        }

        public override void Rotate(Rotation rotation, Point center)
        {
            base.Rotate(rotation, center);
            this.Rails.ForEach(r => r.Rotate(rotation, center));
        }

#region Debug

        [Conditional("DEBUG")]
        public new void DebugInfo()
        {
            Debug.WriteLine($"RailGroup DebugIndex={DebugIndex}");
            Debug.Indent();
            this.DockPoints.DebugList();
            this.Rails.DebugList("Children");
            Debug.Unindent();
        }

#endregion
    }
}
