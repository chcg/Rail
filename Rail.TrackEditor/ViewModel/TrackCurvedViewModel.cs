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

        public string RadiusName
        {
            get { return this.track.RadiusName; }
            set { this.track.RadiusName = value; NotifyPropertyChanged(nameof(RadiusName)); }
        }

        public string AngleName
        {
            get { return this.track.AngleName; }
            set { this.track.AngleName = value; NotifyPropertyChanged(nameof(AngleName)); }
        }

        public TrackExtras[] Extras { get { return (TrackExtras[])Enum.GetValues(typeof(TrackExtras)); } }

        public TrackExtras Extra
        {
            get { return this.track.Extra; }
            set { this.track.Extra = value; NotifyPropertyChanged(nameof(Extra)); }
        }
    }
}
