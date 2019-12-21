using Rail.Misc;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Model
{
    public class TrackCrossing : TrackBase
    {
        [XmlAttribute("Length1")]
        public double Length1 { get; set; }

        [XmlAttribute("Length2")]
        public double Length2 { get; set; }

        [XmlAttribute("Angle")]
        public double Angle { get; set; }

        protected override void Create()
        {


            // Tracks
            this.GeometryTracks = new CombinedGeometry(
                StraitGeometry(this.Length1, StraitOrientation.Center, this.RailSpacing, -this.Angle / 2),
                StraitGeometry(this.Length2, StraitOrientation.Center, this.RailSpacing, +this.Angle / 2));

            DrawingGroup drawingTracks = new DrawingGroup();
            drawingTracks.Children.Add(new GeometryDrawing(trackBrush, linePen, this.GeometryTracks));
            drawingTracks.Children.Add(this.textDrawing);
            this.drawingTracks = drawingTracks;

            DrawingGroup drawingTracksSelected = new DrawingGroup();
            drawingTracksSelected.Children.Add(new GeometryDrawing(trackBrushSelected, linePen, this.GeometryTracks));
            drawingTracksSelected.Children.Add(this.textDrawing);
            this.drawingTracksSelected = drawingTracksSelected;

            // Rail
            this.GeometryRail = new CombinedGeometry(
                StraitGeometry(this.Length1, StraitOrientation.Center, this.sleepersSpacing, -this.Angle / 2),
                StraitGeometry(this.Length2, StraitOrientation.Center, this.sleepersSpacing, +this.Angle / 2));
            DrawingGroup drawingRail = new DrawingGroup();
            if (this.Ballast)
            {
                drawingRail.Children.Add(StraitBallast(this.Length1, StraitOrientation.Center, -this.Angle / 2));
                drawingRail.Children.Add(StraitBallast(this.Length2, StraitOrientation.Center, +this.Angle / 2));
            }
            drawingRail.Children.Add(StraitRail(false, this.Length1, StraitOrientation.Center, -this.Angle / 2));
            drawingRail.Children.Add(StraitRail(false, this.Length2, StraitOrientation.Center, +this.Angle / 2));
            this.drawingRail = drawingRail;

            DrawingGroup drawingRailSelected = new DrawingGroup();
            if (this.Ballast)
            {
                drawingRailSelected.Children.Add(StraitBallast(this.Length1, StraitOrientation.Center, -this.Angle / 2));
                drawingRailSelected.Children.Add(StraitBallast(this.Length2, StraitOrientation.Center, +this.Angle / 2));
            }
            drawingRailSelected.Children.Add(StraitRail(true, this.Length1, StraitOrientation.Center, -this.Angle / 2));
            drawingRailSelected.Children.Add(StraitRail(true, this.Length2, StraitOrientation.Center, +this.Angle / 2));
            this.drawingRailSelected = drawingRailSelected;

            // Terrain
            this.drawingTerrain = drawingRail;

            this.DockPoints = new List<TrackDockPoint>
            {
                new TrackDockPoint(0, new Point(-this.Length1 / 2.0, 0.0).Rotate( this.Angle /2),  this.Angle /2 + 135, this.dockType),
                new TrackDockPoint(1, new Point(-this.Length1 / 2.0, 0.0).Rotate(-this.Angle /2), -this.Angle /2 + 135, this.dockType),
                new TrackDockPoint(2, new Point( this.Length1 / 2.0, 0.0).Rotate( this.Angle /2),  this.Angle /2 + 45-90, this.dockType),
                new TrackDockPoint(3, new Point( this.Length1 / 2.0, 0.0).Rotate(-this.Angle /2), -this.Angle /2 + 45-90, this.dockType),
            };
        }
    }
}
