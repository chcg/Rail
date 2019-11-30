using System.Collections.Generic;
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

        public override void Update(double spacing)
        {
            base.Update(spacing);

            this.Geometry = CreateCrossingTrackGeometry(this.Length1, this.Length2, this.Angle);
            
            this.DockPoints = new List<TrackDockPoint>
            {
                new TrackDockPoint(this, -this.Length1 / 2.0, 0.0, 135),
                new TrackDockPoint(this,  this.Length1 / 2.0, 0.0, 315),
                new TrackDockPoint(this, -this.Length1 / 2.0, 0.0, 135),
                new TrackDockPoint(this,  this.Length1 / 2.0, 0.0, 315)
            };
        }
    }
}
