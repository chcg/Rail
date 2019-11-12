using System.Xml.Serialization;

namespace Rail.Model
{

    public class StraightTrack : BaseTrack
    {
        [XmlAttribute("Length")]
        public double Length { get; set; }
    }
}
