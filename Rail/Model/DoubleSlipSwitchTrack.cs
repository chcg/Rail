using System.Xml.Serialization;

namespace Rail.Model
{
    public class DoubleSlipSwitchTrack : BaseTrack
    {
        [XmlAttribute("Length")]
        public double Length { get; set; }

        [XmlAttribute("Angle")]
        public double Angle { get; set; }

    }
}
