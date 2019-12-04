using System.Xml.Serialization;

namespace Rail.Model
{
    public abstract class TrackCurvedTurnout : TrackBase
    {
        [XmlAttribute("InnerRadius")]
        public double InnerRadius { get; set; }

        [XmlAttribute("InnerAngle")]
        public double InnerAngle { get; set; }

        [XmlAttribute("OuterRadius")]
        public double OuterRadius { get; set; }

        [XmlAttribute("OuterAngle")]
        public double OuterAngle { get; set; }
    }
}
