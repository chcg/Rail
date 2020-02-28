using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.TrackEditor.ViewModel
{
    public class TrackEndPieceViewModel : TrackViewModel
    {
        private readonly TrackEndPiece track;

        public TrackEndPieceViewModel() : this(new TrackEndPiece(), MainViewModel.SelectedTrackTypeViewModel.TrackType)
        { }

        public TrackEndPieceViewModel(TrackType trackType) : this(new TrackEndPiece(), trackType)
        { }

        public TrackEndPieceViewModel(TrackEndPiece track, TrackType trackType) : base(track, trackType)
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
