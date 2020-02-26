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

        public string InnerLength
        {
            get { return this.track.InnerLengthName; }
            set { this.track.InnerLengthName = value; NotifyPropertyChanged(nameof(InnerLength)); }
        }

        public string InnerRadius
        {
            get { return this.track.InnerRadiusName; }
            set { this.track.InnerRadiusName = value; NotifyPropertyChanged(nameof(InnerRadius)); }
        }

        public string InnerAngle
        {
            get { return this.track.InnerAngleName; }
            set { this.track.InnerAngleName = value; NotifyPropertyChanged(nameof(InnerAngle)); }
        }

        public string OuterLength
        {
            get { return this.track.OuterLengthName; }
            set { this.track.OuterLengthName = value; NotifyPropertyChanged(nameof(OuterLength)); }
        }

        public string OuterRadius
        {
            get { return this.track.OuterRadiusName; }
            set { this.track.OuterRadiusName = value; NotifyPropertyChanged(nameof(OuterRadius)); }
        }

        public string OuterAngle
        {
            get { return this.track.OuterAngleName; }
            set { this.track.OuterAngleName = value; NotifyPropertyChanged(nameof(OuterAngle)); }
        }

        public TrackDirection TurnoutDirection
        {
            get { return this.track.TurnoutDirection; }
            set { this.track.TurnoutDirection = value; NotifyPropertyChanged(nameof(TurnoutDirection)); }
        }

        public TrackDrive TurnoutDrive
        {
            get { return this.track.TurnoutDrive; }
            set { this.track.TurnoutDrive = value; NotifyPropertyChanged(nameof(TurnoutDrive)); }
        }
    }
}
