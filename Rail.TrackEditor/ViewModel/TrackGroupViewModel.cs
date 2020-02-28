using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.TrackEditor.ViewModel
{
    public class TrackGroupViewModel : TrackViewModel
    {
        private readonly TrackGroup track;

        public TrackGroupViewModel() : this(new TrackGroup(), MainViewModel.SelectedTrackTypeViewModel.TrackType)
        { }

        public TrackGroupViewModel(TrackType trackType) : this(new TrackGroup(), trackType)
        { }

        public TrackGroupViewModel(TrackGroup track, TrackType trackType) : base(track, trackType)
        {
            this.track = track;
        }
                
    }
}
