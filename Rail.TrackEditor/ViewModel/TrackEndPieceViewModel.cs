using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.TrackEditor.ViewModel
{
    public class TrackEndPieceViewModel : TrackViewModel
    {
        private readonly TrackEndPiece track;

        public TrackEndPieceViewModel(TrackTypeViewModel trackTypeViewModel) : this(trackTypeViewModel, new TrackEndPiece())
        { }

        public TrackEndPieceViewModel(TrackTypeViewModel trackTypeViewModel, TrackEndPiece track) : base(trackTypeViewModel, track)
        {
            this.track = track;
        }

        public string Article
        {
            get { return this.track.Article; }
            set { this.track.Article = value.Trim(); NotifyPropertyChanged(nameof(Article)); }
        }

        public TrackNamedValueViewModel Length
        {
            get { return GetLength(this.track.LengthId); }
            set { this.track.LengthId = value.Id; NotifyPropertyChanged(nameof(Length)); }
        }

        public TrackEndType EndType
        {
            get { return this.track.EndType; }
            set { this.track.EndType = value; NotifyPropertyChanged(nameof(EndType)); }
        }
    }
}
