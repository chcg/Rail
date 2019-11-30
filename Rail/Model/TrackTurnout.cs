using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Rail.Model
{
    public abstract class TrackTurnout : TrackBase
    {
        [XmlAttribute("Length")]
        public double Length { get; set; }

        [XmlAttribute("Radius")]
        public double Radius { get; set; }

        [XmlAttribute("Angle")]
        public double Angle { get; set; }
    }
}
