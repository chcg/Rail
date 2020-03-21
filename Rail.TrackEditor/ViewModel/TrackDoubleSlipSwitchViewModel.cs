using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rail.TrackEditor.ViewModel
{
    public class TrackDoubleSlipSwitchViewModel : TrackViewModel
    {
        private readonly TrackDoubleSlipSwitch track;

        public TrackDoubleSlipSwitchViewModel(TrackTypeViewModel trackTypeViewModel) : this(trackTypeViewModel, new TrackDoubleSlipSwitch())
        { }

        public TrackDoubleSlipSwitchViewModel(TrackTypeViewModel trackTypeViewModel, TrackDoubleSlipSwitch track) : base(trackTypeViewModel, track)
        {
            this.track = track;
        }

        public static TrackViewModel CreateNew(TrackTypeViewModel trackTypeViewModel)
        {
            TrackDoubleSlipSwitch trackDoubleSlipSwitch = new TrackDoubleSlipSwitch
            {
                Article = string.Empty,
                LengthId = trackTypeViewModel.Lengths.First().Id
            };
            return new TrackDoubleSlipSwitchViewModel(trackTypeViewModel, trackDoubleSlipSwitch);
        }

        public string Article
        {
            get { return this.track.Article; }
            set { this.track.Article = value.Trim(); NotifyPropertyChanged(nameof(Article)); }
        }

        public TrackNamedValueViewModel Length
        {
            get { return GetLength(this.track.LengthId); }
            set { this.track.LengthId = value.Id; NotifyPropertyChanged(nameof(Length)); }
        }

        public TrackNamedValueViewModel CrossingAngle
        {
            get { return GetAngle(this.track.CrossingAngleId); }
            set { this.track.CrossingAngleId = value.Id; NotifyPropertyChanged(nameof(CrossingAngle)); }
        }

        public TrackNamedValueViewModel SlipRadius
        {
            get { return GetRadius(this.track.SlipRadiusId); }
            set { this.track.SlipRadiusId = value.Id; NotifyPropertyChanged(nameof(SlipRadius)); }
        }
        public TrackDrive TurnoutDrive
        {
            get { return this.track.TurnoutDrive; }
            set { this.track.TurnoutDrive = value; NotifyPropertyChanged(nameof(TurnoutDrive)); }
        }
    }
}
