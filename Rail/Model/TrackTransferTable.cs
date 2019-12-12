using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Model
{
    public class TrackTransferTable : TrackBase
    {

        [XmlAttribute("RailsA")]
        public int RailsA { get; set; }

        [XmlAttribute("RailsB")]
        public int RailsB { get; set; }

        [XmlAttribute("Width")]
        public double Width { get; set; }

        [XmlAttribute("Height")]
        public double Height { get; set; }

        [XmlAttribute("Length")]
        public double Length { get; set; }

        protected override void Create()
        {
            this.Geometry = new RectangleGeometry(new Rect(-this.Width / 2, -this.Height / 2, this.Width, this.Height));

            // Tracks
            DrawingGroup drawingTracks = new DrawingGroup();
            drawingTracks.Children.Add(new GeometryDrawing(trackBrush, linePen, this.Geometry));
            drawingTracks.Children.Add(this.textDrawing);
            this.drawingTracks = drawingTracks;

            // Rail
            DrawingGroup drawingRail = new DrawingGroup();
            if (this.Ballast)
            {
                //drawingRail.Children.Add(StraitBallast(this.Length, StraitOrientation.Center, 0, null));
            }
            //drawingRail.Children.Add(StraitRail(this.Length));
            this.drawingRail = drawingRail;

            // Terrain
            this.drawingTerrain = drawingRail;

            this.DockPoints = new List<TrackDockPoint>
            {
                new TrackDockPoint(-this.Length / 2.0, 0.0, 135),
                new TrackDockPoint( this.Length / 2.0, 0.0, 315)
            };

        }
    }
}
