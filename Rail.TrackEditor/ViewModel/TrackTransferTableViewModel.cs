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
                DeckLengthId = trackTypeViewModel.Lengths.First().Id,
                ConnectionLengthId = trackTypeViewModel.Lengths.First().Id,
                ConnectionDistanceId = trackTypeViewModel.Lengths.First().Id
            };
            return new TrackTransferTableViewModel(trackTypeViewModel, trackTransferTable);
        }

        public string Article
        {
            get { return this.track.Article; }
            set { this.track.Article = value.Trim(); NotifyPropertyChanged(nameof(Article)); }
        }

        public TrackTransferTableType TransferTableType
        {
            get { return this.track.TransferTableType; }
            set { this.track.TransferTableType = value; NotifyPropertyChanged(nameof(TransferTableType)); }
        }

        public TrackNamedValueViewModel DeckLength
        {
            get { return GetLength(this.track.DeckLengthId); }
            set { this.track.DeckLengthId = value.Id; NotifyPropertyChanged(nameof(DeckLength)); }
        }

        public TrackNamedValueViewModel ConnectionLength
        {
            get { return GetLength(this.track.ConnectionLengthId); }
            set { this.track.ConnectionLengthId = value.Id; NotifyPropertyChanged(nameof(ConnectionLength)); }
        }

        public TrackNamedValueViewModel ConnectionDistance
        {
            get { return GetLength(this.track.ConnectionDistanceId); }
            set { this.track.ConnectionDistanceId = value.Id; NotifyPropertyChanged(nameof(ConnectionDistance)); }
        }

    }
}
