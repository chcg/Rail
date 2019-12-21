using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Model
{
    public class TrackEndPiece : TrackBase
    {
        [XmlAttribute("Length")]
        public double Length { get; set; }

        protected override void Create()
        {
            // Tracks
            this.GeometryTracks = StraitGeometry(this.Length, StraitOrientation.Center, this.RailSpacing);

            DrawingGroup drawingTracks = new DrawingGroup();
            drawingTracks.Children.Add(new GeometryDrawing(trackBrush, linePen, this.GeometryTracks));
            drawingTracks.Children.Add(this.textDrawing);
            this.drawingTracks = drawingTracks;

            // Rail
            this.GeometryRail = StraitGeometry(this.Length, StraitOrientation.Center, this.sleepersSpacing);

            DrawingGroup drawingRail = new DrawingGroup();
            if (this.Ballast)
            {
                drawingRail.Children.Add(StraitBallast(this.RailSpacing, StraitOrientation.Center, 0, null));
            }
            this.drawingRail = drawingRail;

            // Terrain
            this.drawingTerrain = drawingRail;

            // dock points
            this.DockPoints = new List<TrackDockPoint>
            {
                new TrackDockPoint(0, new Point(this.RailSpacing / 2.0, 0.0), 315, this.dockType)
            };
        }
    }
}
