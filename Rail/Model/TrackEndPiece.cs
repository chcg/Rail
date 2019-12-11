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

            DrawingGroup drawing = new DrawingGroup();
            if (this.Ballast)
            {
                drawing.Children.Add(StraitBallast(this.Spacing, StraitOrientation.Center, 0, null));
            }
            this.RailDrawing = drawing;

            this.DockPoints = new List<TrackDockPoint>
            {
                new TrackDockPoint(this.Spacing / 2.0, 0.0, 315)
            };
        }
    }
}
