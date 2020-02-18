using Rail.Tracks.Properties;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Rail.Model
{
    public class TrackStraightAdjustment : TrackStraight
    {
        [XmlAttribute("LengthTo")]
        public double LengthTo { get; set; }

        [XmlIgnore, JsonIgnore]
        public override string Name
        {
            get
            {
                return $"{Resources.TrackStraightAdjustment} {Length} - {LengthTo} mm";
            }
        }

        [XmlIgnore, JsonIgnore]
        public override string Description
        {
            get
            {
                return $"{this.Article} {Resources.TrackStraightAdjustment} {Length} - {LengthTo} mm";
            }
        }
    }
}
