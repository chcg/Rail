using Rail.Mvvm;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Rail.Model
{
    [XmlRoot("TrackList")]
    public class TrackList : BaseVersionedProject
    {
        [XmlElement("TrackType")]
        public List<TrackType> TrackTypes { get; set; }

        public static TrackList Load(string path)
        {
            TrackList trackList = BaseVersionedProject.Load<TrackList>(path);
            trackList.TrackTypes.ForEach(trackType => trackType.Update());
            return trackList;
        }
    }
}
