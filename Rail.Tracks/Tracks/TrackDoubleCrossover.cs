using Rail.Tracks.Properties;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Rail.Tracks
{
    public class TrackDoubleCrossover : TrackCrossing
    {
        public override void Update(TrackType trackType)
        {
            this.LengthA = GetValue(trackType.Lengths, this.LengthAName);
            this.LengthB = GetValue(trackType.Lengths, this.LengthBName);
            this.CrossingAngle = GetValue(trackType.Angles, this.CrossingAngleName);

            this.Name = $"{Resources.TrackDoubleCrossover}";
            this.Description = $"{this.Article} {Resources.TrackDoubleCrossover}";

            base.Update(trackType);
        }

    }
}
