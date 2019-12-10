using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Rail.Model
{
    public class TrackBumper : TrackBase
    {
        [XmlAttribute("Length")]
        public double Length { get; set; }

        protected override void Create()
        {
            this.Geometry = CreateStraitTrackGeometry(this.Length);
            this.RailDrawing = CreateStraitTrackDrawing(this.Length);

            this.DockPoints = new List<TrackDockPoint>
            {
                new TrackDockPoint( this.Length / 2.0, 0.0, 315)
            };
        }
    }
}
