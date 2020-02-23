using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.TrackEditor.ViewModel
{
    public class TrackDoubleTurnoutViewModel : TrackViewModel
    {
        private readonly TrackDoubleTurnout track;

        public TrackDoubleTurnoutViewModel() : this(new TrackDoubleTurnout())
        { }

        public TrackDoubleTurnoutViewModel(TrackDoubleTurnout track) : base(track)
        {
            this.track = track;
        }

        public string Article
        {
            get { return this.track.Article; }
            set { this.track.Article = value; NotifyPropertyChanged(nameof(Article)); }
        }
    }
}
