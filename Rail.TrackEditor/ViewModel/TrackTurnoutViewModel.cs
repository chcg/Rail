using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.TrackEditor.ViewModel
{
    public class TrackTurnoutViewModel : TrackViewModel
    {
        private readonly TrackTurnout track;

        public TrackTurnoutViewModel() : this(new TrackTurnout(), MainViewModel.SelectedTrackTypeViewModel.TrackType)
        { }

        public TrackTurnoutViewModel(TrackType trackType) : this(new TrackTurnout(), trackType)
        { }

        public TrackTurnoutViewModel(TrackTurnout track, TrackType trackType) : base(track, trackType)
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

        public TrackNamedValueViewModel TurnoutLength
        {
            get { return GetLength(this.track.TurnoutLengthId); }
            set { this.track.TurnoutLengthId = value.Id; NotifyPropertyChanged(nameof(TurnoutLength)); }
        }

        public TrackNamedValueViewModel TurnoutRadius
        {
            get { return GetRadius(this.track.TurnoutRadiusId); }
            set { this.track.TurnoutRadiusId = value.Id; NotifyPropertyChanged(nameof(TurnoutRadius)); }
        }

        public TrackNamedValueViewModel TurnoutAngle
        {
            get { return GetAngle(this.track.TurnoutAngleId); }
            set { this.track.TurnoutAngleId = value.Id; NotifyPropertyChanged(nameof(TurnoutAngle)); }
        }

        public TrackNamedValueViewModel CounterCurveRadius
        {
            get { return GetRadius(this.track.CounterCurveRadiusId); }
            set { this.track.CounterCurveRadiusId = value.Id; NotifyPropertyChanged(nameof(CounterCurveRadius)); }
        }

        public TrackNamedValueViewModel CounterCurveAngle
        {
            get { return GetAngle(this.track.CounterCurveAngleId); }
            set { this.track.CounterCurveAngleId = value.Id; NotifyPropertyChanged(nameof(CounterCurveAngle)); }
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
