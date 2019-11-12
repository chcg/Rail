using System.Xml.Serialization;

namespace Rail.Model
{
    public abstract class BaseTrack
    {
        [XmlAttribute("Id")]
        public string Id { get; set; }

        [XmlAttribute("Decription")]
        public string Decription { get; set; }
    }
}
