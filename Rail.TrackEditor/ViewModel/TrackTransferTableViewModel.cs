using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public static TrackViewModel CreateNew(TrackTypeViewModel trackTypeViewModel)
        {
            TrackTransferTable trackTransferTable = new TrackTransferTable
            {
                Article = string.Empty,
                LengthId = trackTypeViewModel.Lengths.First().Id,
                Width = 100,
                Height = 100

            };
            return new TrackTransferTableViewModel(trackTypeViewModel, trackTransferTable);
        }

        public string Article
        {
            get { return this.track.Article; }
            set { this.track.Article = value.Trim(); NotifyPropertyChanged(nameof(Article)); }
        }

    }
}
