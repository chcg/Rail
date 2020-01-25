using Rail.Controls;
using Rail.Trigonometry;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Model
{
    public class RailGroup : RailBase
    {
        protected readonly Pen linePen = new Pen(TrackBrushes.TrackFrame, 2);
        protected readonly Pen dotPen = new Pen(Brushes.White, 2) { DashStyle = DashStyles.Dot };

        public RailGroup()
        { }

        public RailGroup(IEnumerable<RailBase> railItems)
        {
            this.Rails = railItems.Cast<RailItem>().ToList();
            this.Layer = this.Rails.First().Layer;
            this.Rails.ForEach(r => r.IsSelected = false);
            this.DockPoints = new List<RailDockPoint>();
            this.IsSelected = true;

            var intDockPoints =
            this.DockPoints = this.Rails.SelectMany(r => r.DockPoints).Where(d => !d.IsDocked || !this.Rails.Contains(d.DockedWith.RailItem)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        [XmlArray("Rails")]
        [XmlArrayItem("Rail")]
        public List<RailItem> Rails { get; set; }

        [XmlIgnore]
        public override List<TrackMaterial> Materials
        {
            get { return this.Rails.SelectMany(r => r.Materials).ToList(); }
        }

        public override void DrawRailItem(DrawingContext drawingContext, RailViewMode viewMode, RailLayer layer)
        {
            this.Rails.ForEach(r => r.DrawRailItem(drawingContext, viewMode, layer));

            if (this.IsSelected && viewMode < RailViewMode.Terrain)
            {
                //GeometryGroup geometryGroup = new GeometryGroup();
                //geometryGroup.FillRule = FillRule.EvenOdd;

                var x = this.Rails.Select(r =>
                {
                    Geometry geometrie = viewMode switch { RailViewMode.Tracks => r.Track.GeometryTracks, RailViewMode.Rail => r.Track.GeometryRail, _ => null };

                    TransformGroup transformGroup = new TransformGroup();
                    transformGroup.Children.Add(new RotateTransform(r.Angle));
                    transformGroup.Children.Add(new TranslateTransform(r.Position.X, r.Position.Y));

                    Geometry newGeometry = geometrie.Clone();
                    newGeometry.Transform = transformGroup;
                    //geometryGroup.Children.Add(newGeometry);
                    return newGeometry;
                }).ToArray();

                CombinedGeometry combinedGeometry = new CombinedGeometry(GeometryCombineMode.Union, x[0], x[1]);

                //GeometryGroup combinedGeometry = new GeometryGroup();
                //combinedGeometry.FillRule = FillRule.Nonzero;
                //combinedGeometry.Children.Add(x[0]);
                //combinedGeometry.Children.Add(x[1]);

                drawingContext.DrawDrawing(new GeometryDrawing(null, this.linePen, combinedGeometry));
                drawingContext.DrawDrawing(new GeometryDrawing(null, this.dotPen, combinedGeometry));

            }
        }

        public override void Move(Vector vec)
        {
            this.Rails.ForEach(r => r.Position += vec);
        }

        public override void Rotate(Rotation rotation, Point center)
        {
            this.Rails.ForEach(r =>
            {
                r.Angle += rotation;
                r.Position = r.Position.Rotate(rotation, center);
            });
        }

        public override bool IsInside(Point point, RailViewMode viewMode)
        {
            bool isInside = this.Rails.Select(r => r.IsInside(point, viewMode)).Aggregate((a,b) => a | b);
            return isInside;
        }

        public override bool IsInside(Rect rec, RailViewMode viewMode)
        {
            bool isInside = this.Rails.Select(r => r.IsInside(rec, viewMode)).Aggregate((a, b) => a | b);
            return isInside;
        }
    }
}
