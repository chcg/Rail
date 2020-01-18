using Rail.Mvvm;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Rail.Model
{
    [XmlRoot("TrackList")]
    public class TrackList : BaseProject
    {
        public TrackList()
        {
        }

        [XmlElement("TrackType")]
        public List<TrackType> TrackTypes { get; set; }

        public static TrackList Load(string path)
        {
            TrackList trackList = BaseProject.Load<TrackList>(path);
            trackList.TrackTypes.ForEach(trackType => trackType.Update());
            return trackList;
        }
    }
}
