using Rail.Mvvm;
using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.TrackEditor.ViewModel
{
    public class TrackCurvedViewModel : TrackViewModel
    {
        private readonly TrackCurved track;

        public TrackCurvedViewModel() : this(new TrackCurved(), MainViewModel.SelectedTrackTypeViewModel.TrackType)
        { }

        public TrackCurvedViewModel(TrackType trackType) : this(new TrackCurved(), trackType)
        { }

        public TrackCurvedViewModel(TrackCurved track, TrackType trackType) : base(track, trackType)
        {
            this.track = track;
        }

        public string Article
        {
            get { return this.track.Article; }
            set { this.track.Article = value.Trim(); NotifyPropertyChanged(nameof(Article)); }
        }
                
        public TrackNamedValueViewModel Radius
        {
            get { return GetRadius(this.track.RadiusId); }
            set { this.track.RadiusId = value.Id; NotifyPropertyChanged(nameof(Radius)); }
        }

        public TrackNamedValueViewModel Angle
        {
            get { return GetAngle(this.track.AngleId); }
            set { this.track.AngleId = value.Id; NotifyPropertyChanged(nameof(Angle)); }
        }

        public TrackExtras Extra
        {
            get { return this.track.Extra; }
            set { this.track.Extra = value; NotifyPropertyChanged(nameof(Extra)); }
        }
    }
}
