using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.TrackEditor.ViewModel
{
    public class TrackDoubleSlipSwitchViewModel : TrackViewModel
    {
        private readonly TrackDoubleSlipSwitch track;

        public TrackDoubleSlipSwitchViewModel() : this(new TrackDoubleSlipSwitch())
        { }

        public TrackDoubleSlipSwitchViewModel(TrackDoubleSlipSwitch track) : base(track)
        {
            this.track = track;
        }

        public string Article
        {
            get { return this.track.Article; }
            set { this.track.Article = value; NotifyPropertyChanged(nameof(Article)); }
        }

        public string Length
        {
            get { return this.track.LengthName; }
            set { this.track.LengthName = value; NotifyPropertyChanged(nameof(Length)); }
        }

        public string CrossingAngle
        {
            get { return this.track.CrossingAngleName; }
            set { this.track.CrossingAngleName = value; NotifyPropertyChanged(nameof(CrossingAngle)); }
        }

        public string SlipRadius
        {
            get { return this.track.SlipRadiusName; }
            set { this.track.SlipRadiusName = value; NotifyPropertyChanged(nameof(SlipRadius)); }
        }
        public TrackDrive TurnoutDrive
        {
            get { return this.track.TurnoutDrive; }
            set { this.track.TurnoutDrive = value; NotifyPropertyChanged(nameof(TurnoutDrive)); }
        }
    }
}
