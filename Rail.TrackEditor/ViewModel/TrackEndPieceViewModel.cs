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
    }
}
