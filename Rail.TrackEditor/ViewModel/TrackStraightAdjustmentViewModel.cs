using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.TrackEditor.ViewModel
{
    public class TrackStraightAdjustmentViewModel : TrackViewModel
    {
        private readonly TrackStraightAdjustment track;

        public TrackStraightAdjustmentViewModel() : this(new TrackStraightAdjustment(), MainViewModel.SelectedTrackTypeViewModel.TrackType)
        { }

        public TrackStraightAdjustmentViewModel(TrackType trackType) : this(new TrackStraightAdjustment(), trackType)
        { }

        public TrackStraightAdjustmentViewModel(TrackStraightAdjustment track, TrackType trackType) : base(track, trackType)
        {
            this.track = track;
        }

        public string Article
        {
            get { return this.track.Article; }
            set { this.track.Article = value.Trim(); NotifyPropertyChanged(nameof(Article)); }
        }
    }
}
