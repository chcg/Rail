using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.TrackEditor.ViewModel
{
    public class TrackThreeWayTurnoutViewModel : TrackViewModel
    {
        private readonly TrackThreeWayTurnout track;

        public TrackThreeWayTurnoutViewModel() : this(new TrackThreeWayTurnout(), MainViewModel.SelectedTrackTypeViewModel.TrackType)
        { }

        public TrackThreeWayTurnoutViewModel(TrackType trackType) : this(new TrackThreeWayTurnout(), trackType)
        { }

        public TrackThreeWayTurnoutViewModel(TrackThreeWayTurnout track, TrackType trackType) : base(track, trackType)
        {
            this.track = track;
        }

        public string Article
        {
            get { return this.track.Article; }
            set { this.track.Article = value.Trim(); NotifyPropertyChanged(nameof(Article)); }
        }

        public TrackNamedValueViewModel StraightLength
        {
            get { return GetLength(this.track.StraightLengthId); }
            set { this.track.StraightLengthId = value.Id; NotifyPropertyChanged(nameof(StraightLength)); }
        }

        public TrackNamedValueViewModel LeftTurnoutLength
        {
            get { return GetLength(this.track.LeftTurnoutLengthId); }
            set { this.track.LeftTurnoutLengthId = value.Id; NotifyPropertyChanged(nameof(LeftTurnoutLength)); }
        }

        public TrackNamedValueViewModel LeftTurnoutRadius
        {
            get { return GetRadius(this.track.LeftTurnoutRadiusId); }
            set { this.track.LeftTurnoutRadiusId = value.Id; NotifyPropertyChanged(nameof(LeftTurnoutRadius)); }
        }

        public TrackNamedValueViewModel LeftTurnoutAngle
        {
            get { return GetAngle(this.track.LeftTurnoutAngleId); }
            set { this.track.LeftTurnoutAngleId = value.Id; NotifyPropertyChanged(nameof(LeftTurnoutAngle)); }
        }

        public TrackNamedValueViewModel RightTurnoutLength
        {
            get { return GetLength(this.track.RightTurnoutLengthId); }
            set { this.track.RightTurnoutLengthId = value.Id; NotifyPropertyChanged(nameof(RightTurnoutLength)); }
        }

        public TrackNamedValueViewModel RightTurnoutRadius
        {
            get { return GetRadius(this.track.RightTurnoutRadiusId); }
            set { this.track.RightTurnoutRadiusId = value.Id; NotifyPropertyChanged(nameof(RightTurnoutRadius)); }
        }

        public TrackNamedValueViewModel RightTurnoutAngle
        {
            get { return GetAngle(this.track.RightTurnoutAngleId); }
            set { this.track.RightTurnoutAngleId = value.Id; NotifyPropertyChanged(nameof(RightTurnoutAngle)); }
        }

        public TrackDrive TurnoutDrive
        {
            get { return this.track.TurnoutDrive; }
            set { this.track.TurnoutDrive = value; NotifyPropertyChanged(nameof(TurnoutDrive)); }
        }
    }
}
