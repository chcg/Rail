using System.Xml.Serialization;

namespace Rail.Model
{
    public class CrossingTrack : BaseTrack
    {
        [XmlAttribute("Length1")]
        public double Length1 { get; set; }

        [XmlAttribute("Length2")]
        public double Length2 { get; set; }

        [XmlAttribute("Angle")]
        public double Angle { get; set; }
    }
}
