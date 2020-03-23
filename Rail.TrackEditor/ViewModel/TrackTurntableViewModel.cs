using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rail.TrackEditor.ViewModel
{
    public class TrackTurntableViewModel : TrackViewModel
    {
        private readonly TrackTurntable track;

        public TrackTurntableViewModel(TrackTypeViewModel trackTypeViewModel) : this(trackTypeViewModel, new TrackTurntable())
        { }

        public TrackTurntableViewModel(TrackTypeViewModel trackTypeViewModel, TrackTurntable track) : base(trackTypeViewModel, track)
        {
            this.track = track;
        }

        public static TrackViewModel CreateNew(TrackTypeViewModel trackTypeViewModel)
        {
            TrackTurntable trackTurntable = new TrackTurntable
            {
                Article = string.Empty,
                DeckLengthId = trackTypeViewModel.Lengths.First().Id,
                ConnectionLengthId = trackTypeViewModel.Lengths.First().Id
            };
            return new TrackTurntableViewModel(trackTypeViewModel, trackTurntable);
        }

        public string Article
        {
            get { return this.track.Article; }
            set { this.track.Article = value.Trim(); NotifyPropertyChanged(nameof(Article)); }
        }

        public TrackTurntableRailNum RailNum
        {
            get { return this.track.RailNum; }
            set { this.track.RailNum = value; NotifyPropertyChanged(nameof(RailNum)); }
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
    }
}
