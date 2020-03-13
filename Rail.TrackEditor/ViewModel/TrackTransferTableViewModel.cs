using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.TrackEditor.ViewModel
{
    public class TrackTransferTableViewModel : TrackViewModel
    {
        private readonly TrackTransferTable track;

        public TrackTransferTableViewModel(TrackTypeViewModel trackTypeViewModel) : this(trackTypeViewModel, new TrackTransferTable())
        { }

        public TrackTransferTableViewModel(TrackTypeViewModel trackTypeViewModel, TrackTransferTable track) : base(trackTypeViewModel, track)
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
