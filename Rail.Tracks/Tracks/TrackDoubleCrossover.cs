using Rail.Tracks.Properties;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Rail.Tracks
{
    public class TrackDoubleCrossover : TrackCrossing
    {
        public override void Update(TrackType trackType)
        {
            this.Length = GetValue(trackType.Lengths, this.LengthId);
            this.LengthB = GetValue(trackType.Lengths, this.LengthBId);
            this.CrossingAngle = GetValue(trackType.Angles, this.CrossingAngleId);

            this.Name = $"{Resources.TrackDoubleCrossover}";
            this.Description = $"{this.Article} {Resources.TrackDoubleCrossover}";

            base.Update(trackType);
        }

    }
}
