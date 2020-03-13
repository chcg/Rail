using Rail.Tracks.Properties;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Rail.Tracks
{
    public class TrackAdjustment : TrackStraight
    {
        [XmlElement("LengthTo")]
        public double LengthTo { get; set; }

        

        public override void Update(TrackType trackType)
        {
            base.Update(trackType);

            this.Name = $"{Resources.TrackStraightAdjustment} {Length} - {LengthTo} mm";
            this.Description = $"{this.Article} {Resources.TrackStraightAdjustment} {Length} - {LengthTo} mm";
            
        }
    }
}
