using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.TrackEditor.ViewModel
{
    public class TrackEndPieceViewModel : TrackViewModel
    {
        private readonly TrackEndPiece track;

        public TrackEndPieceViewModel() : this(new TrackEndPiece())
        { }

        public TrackEndPieceViewModel(TrackEndPiece track) : base(track)
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
