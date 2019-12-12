using System;
using System.Collections.Generic;
using System.Text;
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
            this.Geometry = CreateStraitTrackGeometry(this.Spacing);

            // Tracks
            DrawingGroup drawingTracks = new DrawingGroup();
            drawingTracks.Children.Add(new GeometryDrawing(trackBrush, linePen, this.Geometry));
            drawingTracks.Children.Add(this.textDrawing);
            this.drawingTracks = drawingTracks;

            // Rail
            DrawingGroup drawingRail = new DrawingGroup();
            if (this.Ballast)
            {
                drawingRail.Children.Add(StraitBallast(this.Spacing, StraitOrientation.Center, 0, null));
            }
            this.drawingRail = drawingRail;

            // Terrain
            this.drawingTerrain = drawingRail;

            this.DockPoints = new List<TrackDockPoint>
            {
                new TrackDockPoint(this.Spacing / 2.0, 0.0, 315)
            };
        }
    }
}
