using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rail.TrackEditor.ViewModel
{
    public class TrackTableViewModel : TrackViewModel
    {
        private readonly TrackTable track;

        public TrackTableViewModel(TrackTypeViewModel trackTypeViewModel) : this(trackTypeViewModel, new TrackTable())
        { }

        public TrackTableViewModel(TrackTypeViewModel trackTypeViewModel, TrackTable track) : base(trackTypeViewModel, track)
        {
            this.track = track;
        }

        public static TrackViewModel CreateNew(TrackTypeViewModel trackTypeViewModel)
        {
            TrackTable trackTable = new TrackTable
            {
                Article = string.Empty,
                DeckLengthId = trackTypeViewModel.Lengths.First().Id,
                ConnectionLengthId = trackTypeViewModel.Lengths.First().Id,
                ConnectionDistanceId = trackTypeViewModel.Lengths.First().Id
            };
            return new TrackTableViewModel(trackTypeViewModel, trackTable);
        }

        public override TrackViewModel Clone()
        {
            return new TrackTableViewModel(this.trackTypeViewModel, (TrackTable)this.track.Clone());
        }

        public string Article
        {
            get { return this.track.Article; }
            set { this.track.Article = value.Trim(); NotifyPropertyChanged(nameof(Article)); }
        }

        public TrackTableType TableType
        {
            get { return this.track.TableType; }
            set { this.track.TableType = value; NotifyPropertyChanged(nameof(TableType)); }
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
