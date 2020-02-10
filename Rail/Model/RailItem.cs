using Rail.Controls;
using Rail.Misc;
using Rail.Mvvm;
using Rail.Trigonometry;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Model
{
    [DebuggerDisplay("RailItem Index={DebugIndex} X={Position.X} Y={Position.Y} A={Angle}")]
    public class RailItem : RailBase
    {
        public RailItem()
        { }

        public RailItem(TrackBase track, Point pos, Guid layer) 
        {
            this.DebugIndex = globalDebugIndex++;
            //this.Id = Guid.NewGuid();
            this.TrackId = track.Id;
            this.Track = track;
            this.Position = pos;
            this.Angle = 0.0;
            this.Layer = layer;
            this.DockPoints = track.DockPoints.Select(dp => new RailDockPoint(this, dp)).ToList();
        }

        //public RailItem(RailGroupItem railGroupItem) : base(railGroupItem)
        //{
        //    this.Track = railGroupItem.Track;
        //    this.TrackId = railGroupItem.TrackId;
        //}

        //public RailItem(RailRampItem railRampItem) : base(railRampItem)
        //{
        //    railRampItem.CopyTo(this);
        //    this.Track = railRampItem.Track;
        //    this.TrackId = railRampItem.TrackId;
        //}

        //public RailItem(RailHelixItem railHelixItem) : base(railHelixItem)
        //{
        //    railHelixItem.CopyTo(this);
        //    this.Track = railHelixItem.Track;
        //    this.TrackId = railHelixItem.TrackId;
        //}

        //[XmlElement("Id")]
        //public Guid Id { get; set; }

        [XmlElement("TrackId")]
        public string TrackId { get; set; }

        [XmlIgnore, JsonIgnore]
        public TrackBase Track { get; set; }

        [XmlIgnore, JsonIgnore]
        public override List<TrackMaterial> Materials
        {
            get { return this.Track.Materials; }
        }

        public override RailBase Clone()
        {
            var clone = new RailItem()
            {
                DebugIndex = globalDebugIndex++,
                Position = this.Position,
                Angle = this.Angle,
                Layer = this.Layer,
                //DockPoints = this.DockPoints.Select(d => d.Clone()).ToList(),
                TrackId = this.TrackId,
                Track = this.Track
            };
            clone.DockPoints = this.DockPoints.Select(d => d.Clone(clone)).ToList();
            return clone;
        }

        public override void DrawRailItem(DrawingContext drawingContext, RailViewMode viewMode, RailLayer layer)
        {
            this.DockPoints.ForEach(d => d.DrawOpen(drawingContext));

            drawingContext.PushTransform(this.RailTransform);

            //Debug.WriteLine($"DrawRailItem {this.IsSelected}");
            this.Track.Render(drawingContext, viewMode, layer.TrackBrush);
            if (this.IsSelected)
            {
                this.Track.RenderSelection(drawingContext, viewMode);
            }
            
            drawingContext.Pop();

            DrawDebug(drawingContext);
            DrawDebugDogpoints(drawingContext);
        }


        protected override Geometry GetGeometry(RailViewMode viewMode)
        {
            Geometry geometry = viewMode == RailViewMode.Tracks ? this.Track.GeometryTracks.Clone() : this.Track.GeometryRail.Clone();
            geometry.Transform = this.RailTransform;
            return geometry;
        }

        #region debug

        [Conditional("DEBUGINFO")]
        public void DrawDebug(DrawingContext drawingContext)
        {
            drawingContext.DrawRectangle(Brushes.White, new Pen(Brushes.Blue, 1), new Rect(this.Position, new Size(32, 20)));
            drawingContext.DrawText(new FormattedText(this.DebugIndex.ToString(), CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface("Verdana"), 9, Brushes.Blue, 1.25), this.Position);
            drawingContext.DrawText(new FormattedText(this.Angle.Value.ToString(), CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface("Verdana"), 9, Brushes.Blue, 1.25), this.Position + new Vector(0, 9));
        }

        [Conditional("DEBUGINFO")]
        public void DrawDebugDogpoints(DrawingContext drawingContext)
        {
            double width = 48;
            foreach (var dp in this.DockPoints)
            {
                Point pos = dp.Position - new Vector((dp.Angle < 45 || dp.Angle > 225 ? width : 0), (dp.Angle < 135 || dp.Angle > 315 ? 20 : 0));
                drawingContext.DrawRectangle(Brushes.White, new Pen(Brushes.Blue, 1), new Rect(pos, new Size(width, 20)));
                string str1 = $"{dp.DebugDockPointIndex}-{dp.DockedWith?.DebugRailIndex},{dp.DockedWith?.DebugDockPointIndex}";
                string str2 = $"{dp.Angle}-{dp.DockedWith?.Angle}";
                drawingContext.DrawText(new FormattedText(str1, CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface("Verdana"), 9, Brushes.Blue, 1.25), pos);
                drawingContext.DrawText(new FormattedText(str2, CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface("Verdana"), 9, Brushes.Blue, 1.25), pos + new Vector(0, 9));
            }
        }

        #endregion
    }
}
