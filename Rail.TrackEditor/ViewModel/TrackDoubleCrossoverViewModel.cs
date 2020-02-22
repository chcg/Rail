using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.TrackEditor.ViewModel
{
    public class TrackDoubleCrossoverViewModel : TrackViewModel
    {
        private readonly TrackDoubleCrossover track;

        public TrackDoubleCrossoverViewModel()
        {
            this.track = new TrackDoubleCrossover();
        }

        public TrackDoubleCrossoverViewModel(TrackDoubleCrossover track)
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
