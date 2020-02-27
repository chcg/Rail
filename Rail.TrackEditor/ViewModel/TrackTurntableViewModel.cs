using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.TrackEditor.ViewModel
{
    public class TrackTurntableViewModel : TrackViewModel
    {
        private readonly TrackTurntable track;

        public TrackTurntableViewModel() : this(new TrackTurntable())
        { }

        public TrackTurntableViewModel(TrackTurntable track) : base(track)
        {
            this.track = track;
        }

        public string Article
        {
            get { return this.track.Article; }
            set { this.track.Article = value.Trim(); NotifyPropertyChanged(nameof(Article)); }
        }

        public string OuterRadius
        {
            get { return this.track.OuterRadiusName; }
            set { this.track.OuterRadiusName = value; NotifyPropertyChanged(nameof(OuterRadius)); }
        }

        public string InnerRadius
        {
            get { return this.track.InnerRadiusName; }
            set { this.track.InnerRadiusName = value; NotifyPropertyChanged(nameof(InnerRadius)); }
        }

        public string Angle
        {
            get { return this.track.AngleName; }
            set { this.track.AngleName = value; NotifyPropertyChanged(nameof(Angle)); }
        }

        public int RailNum
        {
            get { return this.track.RailNum; }
            set { this.track.RailNum = value; NotifyPropertyChanged(nameof(RailNum)); }
        }

    }
}
