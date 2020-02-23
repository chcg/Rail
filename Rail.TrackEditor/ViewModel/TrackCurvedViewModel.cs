using Rail.Mvvm;
using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.TrackEditor.ViewModel
{
    public class TrackCurvedViewModel : TrackViewModel
    {
        private readonly TrackCurved track;

        public TrackCurvedViewModel() : this(new TrackCurved())
        { }

        public TrackCurvedViewModel(TrackCurved track) : base(track)
        {
            this.track = track;
        }

        public string Article
        {
            get { return this.track.Article; }
            set { this.track.Article = value; NotifyPropertyChanged(nameof(Article)); }
        }

        public double Radius
        {
            get { return this.track.Radius; }
            set { this.track.Radius = value; NotifyPropertyChanged(nameof(Radius)); }
        }

        public double Angle
        {
            get { return this.track.Angle; }
            set { this.track.Angle = value; NotifyPropertyChanged(nameof(Angle)); }
        }

        public TrackExtras[] Extras { get { return (TrackExtras[])Enum.GetValues(typeof(TrackExtras)); } }

        public TrackExtras Extra
        {
            get { return this.track.Extra; }
            set { this.track.Extra = value; NotifyPropertyChanged(nameof(Extra)); }
        }
    }
}
