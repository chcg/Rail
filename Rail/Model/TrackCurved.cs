using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Model
{
    public class TrackCurved : TrackBase
    {
        [XmlAttribute("Radius")]
        public double Radius { get; set; }

        [XmlAttribute("Angle")]
        public double Angle { get; set; }

        public override void Update(double spacing)
        {
            base.Update(spacing);

            this.Geometry = CreateCurvedTrackGeometry(this.Angle, this.Radius);

            this.DockPoints = new List<TrackDockPoint>
            {
                new TrackDockPoint(this.Radius, 0, 225.0),
                new TrackDockPoint(this.Radius, 0, 45.0)
            };
        }
    }
}
