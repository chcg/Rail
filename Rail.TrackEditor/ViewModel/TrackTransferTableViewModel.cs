using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.TrackEditor.ViewModel
{
    public class TrackTransferTableViewModel : TrackViewModel
    {
        private readonly TrackTransferTable track;

        public TrackTransferTableViewModel() : this(new TrackTransferTable(), MainViewModel.SelectedTrackTypeViewModel.TrackType)
        { }

        public TrackTransferTableViewModel(TrackType trackType) : this(new TrackTransferTable(), trackType)
        { }

        public TrackTransferTableViewModel(TrackTransferTable track, TrackType trackType) : base(track, trackType)
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
