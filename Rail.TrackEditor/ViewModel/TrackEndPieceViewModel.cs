using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.TrackEditor.ViewModel
{
    public class TrackEndPieceViewModel : TrackViewModel
    {
        private readonly TrackEndPiece track;

        public TrackEndPieceViewModel()
        {
            this.track = new TrackEndPiece();
        }

        public TrackEndPieceViewModel(TrackEndPiece track)
        {
            this.track = track;
        }

        public string Name { get { return this.track.Name; } }

        public string Article
        {
            get { return this.track.Article; }
            set { this.track.Article = value; NotifyPropertyChanged(nameof(Article)); }
        }
    }
}
