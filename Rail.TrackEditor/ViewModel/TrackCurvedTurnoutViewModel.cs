using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.TrackEditor.ViewModel
{
    public class TrackCurvedTurnoutViewModel : TrackViewModel
    {
        private readonly TrackCurvedTurnout track;

        public TrackCurvedTurnoutViewModel() : this(new TrackCurvedTurnout())
        { }

        public TrackCurvedTurnoutViewModel(TrackCurvedTurnout track) : base(track)
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
