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

        public override void Update(double spacing, bool ballast)
        {
            base.Update(spacing, ballast);

            this.Geometry = CreateCrossingTrackGeometry(this.Length1, this.Length2, this.Angle);
            this.BallastDrawing = CreateCrossingBallastDrawing(this.Length1, this.Length2, this.Angle);
            this.RailDrawing = CreateCrossingTrackDrawing(this.Length1, this.Length2, this.Angle);

            this.DockPoints = new List<TrackDockPoint>
            {
                new TrackDockPoint(new Point(-this.Length1 / 2.0, 0.0).Rotate( this.Angle /2),  this.Angle /2 + 135),
                new TrackDockPoint(new Point(-this.Length1 / 2.0, 0.0).Rotate(-this.Angle /2), -this.Angle /2 + 135),
                new TrackDockPoint(new Point( this.Length1 / 2.0, 0.0).Rotate( this.Angle /2),  this.Angle /2 + 45-90),
                new TrackDockPoint(new Point( this.Length1 / 2.0, 0.0).Rotate(-this.Angle /2), -this.Angle /2 + 45-90),
            };
        }
    }
}
