using Rail.Mvvm;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Rail.Model
{
    [XmlRoot("Tracks")]
    public class Tracks : BaseVersionedProject
    {
        [XmlElement("TrackType")]
        public List<TrackType> TrackTypes { get; set; }

        public static Tracks Load(string path)
        {
            return BaseVersionedProject.Load<Tracks>(path);
        }
    }
}
