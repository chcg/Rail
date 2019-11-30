using System.Xml.Serialization;

namespace Rail.Model
{
    public abstract class TrackCurvedTurnout : TrackBase
    {
        [XmlAttribute("Radius")]
        public double Radius { get; set; }

        [XmlAttribute("Angle")]
        public double Angle { get; set; }

        [XmlAttribute("Distance")]
        public double Length { get; set; }
    }
}
