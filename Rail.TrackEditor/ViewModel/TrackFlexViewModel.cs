using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.TrackEditor.ViewModel
{
    public class TrackFlexViewModel : TrackViewModel
    {
        private readonly TrackFlex track;

        public TrackFlexViewModel() : this(new TrackFlex(), MainViewModel.SelectedTrackTypeViewModel.TrackType)
        { }

        public TrackFlexViewModel(TrackType trackType) : this(new TrackFlex(), trackType)
        { }

        public TrackFlexViewModel(TrackFlex track, TrackType trackType) : base(track, trackType)
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
