using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.TrackEditor.ViewModel
{
    public class TrackCrossingViewModel : TrackViewModel
    {
        private readonly TrackCrossing track;

        public TrackCrossingViewModel() : this(new TrackCrossing())
        { }

        public TrackCrossingViewModel(TrackCrossing track) : base(track)
        {
            this.track = track;
        }

        public string Article
        {
            get { return this.track.Article; }
            set { this.track.Article = value; NotifyPropertyChanged(nameof(Article)); }
        }

        public string LengthA
        {
            get { return this.track.LengthAName; }
            set { this.track.LengthAName = value; NotifyPropertyChanged(nameof(LengthA)); }
        }
        
        public string LengthB
        {
            get { return this.track.LengthBName; }
            set { this.track.LengthBName = value; NotifyPropertyChanged(nameof(LengthB)); }
        }
        
        public string CrossingAngle
        {
            get { return this.track.CrossingAngleName; }
            set { this.track.CrossingAngleName = value; NotifyPropertyChanged(nameof(CrossingAngle)); }
        }


    }
}
