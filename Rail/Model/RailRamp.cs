using Rail.Controls;
using Rail.Misc;
using Rail.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Model
{
    public class RailRamp : RailGroup
    {
        //private static readonly double PIFactor = Math.PI / 180.0;

        //protected readonly Pen linePen = new Pen(TrackBrushes.TrackFrame, 2);
        //protected readonly Pen dotPen = new Pen(Brushes.White, 2) { DashStyle = DashStyles.Dot };

        public RailRamp()
        { }

        public RailRamp(IEnumerable<RailBase> railItems)
        {
            //Settings settings = Settings.Default;

            //this.DebugIndex = globalDebugIndex++;

            //RailItem firstRailItme = (RailItem)railItems.FirstOrDefault();
            //this.Position = firstRailItme.Position; // set before new RailGroupItem
            //this.Angle = 0.0;

            //this.Layer = firstRailItme.Layer;

            //this.Rails = railItems.Cast<RailItem>().Select(i => new RailRampItem(i, this)).ToList();

            //this.Rails.ForEach(r => r.IsSelected = false);

            //int num = this.Rails.Count();

            //// calculate slope gradient
            //double height = this.LayerHeigh;
            //for (int i = 1; i <= num / 2; i++)
            //{
            //    height -= this.Rails[i - 1].SetGradientInPercent(settings.RampKinkAngle * i);
            //    height -= this.Rails[num - i].SetGradientInPercent(settings.RampKinkAngle * i);
            //}
            ////    height -= this.Rails[1].SetGradientInPercent(2.5);
            ////height -= this.Rails[this.Rails.Count() - 2].SetGradientInPercent(2.5);
             
            //var innerRails = this.Rails.Skip(4).SkipLast(4).ToList();
            //double length = innerRails.Sum(r => r.Length);
            //double angle = Math.Asin(height / length) / PIFactor;
            //innerRails.ForEach(r => r.SetGradient(angle));

            //this.DockPoints = new List<RailDockPoint>();
            //this.IsSelected = true;

            //var intDockPoints =
            //this.DockPoints = this.Rails.SelectMany(r => r.DockPoints).Where(d => !d.IsDocked || !this.Rails.Contains(d.DockedWith.RailItem)).ToList();

            //CreateCombinedGeometries();
        }

        //public IEnumerable<RailItem> Resolve()
        //{
        //    return this.Rails.Select(r => new RailItem(r)).ToList();
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        //[XmlArray("Rails")]
        //[XmlArrayItem("Rail")]
        //public List<RailRampItem> Rails { get; set; }

        //[XmlIgnore, JsonIgnore]
        //public override List<TrackMaterial> Materials
        //{
        //    get { return this.Rails.SelectMany(r => r.Materials).ToList(); }
        //}

        [XmlIgnore]
        public double LayerHeigh = 100.0;

        public override RailBase Clone()
        {
            var clone = new RailRamp()
            {
                DebugIndex = globalDebugIndex++,
                Position = this.Position,
                Angle = this.Angle,
                Layer = this.Layer,
                //DockPoints = this.DockPoints.Select(d => d.Clone()).ToList(),
                Rails = this.Rails.Cast<RailRampItem>().Select(r => (RailItem)r.Clone()).ToList()
            };
            clone.DockPoints = this.DockPoints.Select(d => d.Clone(clone)).ToList();
            return clone;
        }

        //private Geometry combinedGeometryTracks;
        //private Geometry combinedGeometryRail;

        //private void CreateCombinedGeometries()
        //{
        //    this.combinedGeometryTracks = this.Rails.Select(r =>
        //    {
        //        TransformGroup transformGroup = new TransformGroup();
        //        transformGroup.Children.Add(new RotateTransform(r.Angle));
        //        transformGroup.Children.Add(new TranslateTransform(r.Position.X, r.Position.Y));

        //        //Transform transformGroup = r.RailTransform;

        //        Geometry geometry = r.Track.GeometryTracks.Clone();
        //        geometry.Transform = transformGroup;
        //        return geometry;
        //    }).Aggregate((a, b) => new CombinedGeometry(GeometryCombineMode.Union, a, b));

        //    this.combinedGeometryRail = this.Rails.Select(r =>
        //    {

        //        Geometry geometry = r.Track.GeometryRail.Clone();
        //        geometry.Transform = r.RailTransform;
        //        return geometry;
        //    }).Aggregate((a, b) => new CombinedGeometry(GeometryCombineMode.Union, a, b));
        //}

        //public override void DrawRailItem(DrawingContext drawingContext, RailViewMode viewMode, RailLayer layer)
        //{
        //    drawingContext.PushTransform(this.RailTransform);

        //    this.Rails.ForEach(r => r.DrawRailItem(drawingContext, viewMode, layer));

        //    if (this.IsSelected && viewMode < RailViewMode.Terrain)
        //    {
        //        /*
        //        //GeometryGroup geometryGroup = new GeometryGroup();
        //        //geometryGroup.FillRule = FillRule.EvenOdd;

        //        var x = this.Rails.Select(r =>
        //        {
        //            Geometry geometrie = viewMode switch { RailViewMode.Tracks => r.Track.GeometryTracks, RailViewMode.Rail => r.Track.GeometryRail, _ => null };

        //            TransformGroup transformGroup = new TransformGroup();
        //            transformGroup.Children.Add(new RotateTransform(r.Angle));
        //            transformGroup.Children.Add(new TranslateTransform(r.Position.X, r.Position.Y));

        //            Geometry newGeometry = geometrie.Clone();
        //            newGeometry.Transform = transformGroup;
        //            //geometryGroup.Children.Add(newGeometry);
        //            return newGeometry;
        //        }).ToArray();

        //        CombinedGeometry combinedGeometry = new CombinedGeometry(GeometryCombineMode.Union, x[0], x[1]);

        //        //GeometryGroup combinedGeometry = new GeometryGroup();
        //        //combinedGeometry.FillRule = FillRule.Nonzero;
        //        //combinedGeometry.Children.Add(x[0]);
        //        //combinedGeometry.Children.Add(x[1]);

        //        drawingContext.DrawDrawing(new GeometryDrawing(null, this.linePen, combinedGeometry));
        //        drawingContext.DrawDrawing(new GeometryDrawing(null, this.dotPen, combinedGeometry));
        //        */
        //        Geometry geometrie = viewMode switch { RailViewMode.Tracks => combinedGeometryTracks, RailViewMode.Rail => combinedGeometryRail, _ => null };
        //        drawingContext.DrawDrawing(new GeometryDrawing(null, this.linePen, geometrie));
        //        drawingContext.DrawDrawing(new GeometryDrawing(null, this.dotPen, geometrie));
        //    }

        //    drawingContext.Pop();
        //}


        //protected override Geometry GetGeometry(RailViewMode viewMode, Transform transform)
        //{
        //    Geometry geometry = viewMode switch { RailViewMode.Tracks => this.combinedGeometryTracks.Clone(), RailViewMode.Rail => this.combinedGeometryRail.Clone(), _ => null };
        //    geometry.Transform = this.RailTransform;
        //    return geometry;
        //}
    }
}
