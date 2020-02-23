using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.TrackEditor.ViewModel
{
    public class TrackYTurnoutViewModel : TrackViewModel
    {
        private readonly TrackYTurnout track;

        public TrackYTurnoutViewModel() : this(new TrackYTurnout())
        { }

        public TrackYTurnoutViewModel(TrackYTurnout track) : base(track)
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
