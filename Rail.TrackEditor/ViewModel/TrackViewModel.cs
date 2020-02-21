using Rail.Mvvm;
using Rail.Tracks;

namespace Rail.TrackEditor.ViewModel
{
    public class TrackViewModel : BaseViewModel
    {
        public static TrackViewModel Create(TrackBase track)
        {
            string typeName = track.GetType().Name;
            return typeName switch
            {
                nameof(TrackStraight) => new TrackStraightViewModel((TrackStraight)track),
                nameof(TrackCurved) => new TrackCurvedViewModel((TrackCurved)track),
                nameof(TrackTurnout) => new TrackTurnoutViewModel((TrackTurnout)track),
                _ => null
            };
        }


    }
}
