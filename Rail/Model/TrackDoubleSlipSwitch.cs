using System.Xml.Serialization;

namespace Rail.Model
{
    public class TrackDoubleSlipSwitch : TrackBase
    {
        [XmlAttribute("Length")]
        public double Length { get; set; }

        [XmlAttribute("Angle")]
        public double Angle { get; set; }

    }
}
