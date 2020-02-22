using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.TrackEditor.ViewModel
{
    public class TrackDoubleTurnoutViewModel : TrackViewModel
    {
        private readonly TrackDoubleTurnout track;

        public TrackDoubleTurnoutViewModel()
        {
            this.track = new TrackDoubleTurnout();
        }

        public TrackDoubleTurnoutViewModel(TrackDoubleTurnout track)
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
