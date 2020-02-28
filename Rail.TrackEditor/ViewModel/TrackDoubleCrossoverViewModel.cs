using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.TrackEditor.ViewModel
{
    public class TrackDoubleCrossoverViewModel : TrackViewModel
    {
        private readonly TrackDoubleCrossover track;

        public TrackDoubleCrossoverViewModel() : this(new TrackDoubleCrossover(), MainViewModel.SelectedTrackTypeViewModel.TrackType)
        { }

        public TrackDoubleCrossoverViewModel(TrackType trackType) : this(new TrackDoubleCrossover(), trackType)
        { }

        public TrackDoubleCrossoverViewModel(TrackDoubleCrossover track, TrackType trackType) : base(track, trackType)
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
