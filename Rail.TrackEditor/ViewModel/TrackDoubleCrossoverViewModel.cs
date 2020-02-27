using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.TrackEditor.ViewModel
{
    public class TrackDoubleCrossoverViewModel : TrackViewModel
    {
        private readonly TrackDoubleCrossover track;

        public TrackDoubleCrossoverViewModel() : this(new TrackDoubleCrossover())
        { }

        public TrackDoubleCrossoverViewModel(TrackDoubleCrossover track) : base(track)
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
