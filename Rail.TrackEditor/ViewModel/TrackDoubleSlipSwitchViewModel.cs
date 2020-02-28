using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.TrackEditor.ViewModel
{
    public class TrackDoubleSlipSwitchViewModel : TrackViewModel
    {
        private readonly TrackDoubleSlipSwitch track;

        public TrackDoubleSlipSwitchViewModel() : this(new TrackDoubleSlipSwitch(), MainViewModel.SelectedTrackTypeViewModel.TrackType)
        { }

        public TrackDoubleSlipSwitchViewModel(TrackType trackType) : this(new TrackDoubleSlipSwitch(), trackType)
        { }

        public TrackDoubleSlipSwitchViewModel(TrackDoubleSlipSwitch track, TrackType trackType) : base(track, trackType)
        {
            this.track = track;
        }

        public string Article
        {
            get { return this.track.Article; }
            set { this.track.Article = value.Trim(); NotifyPropertyChanged(nameof(Article)); }
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
