using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rail.TrackEditor.ViewModel
{
    public class TrackTurnoutViewModel : TrackViewModel
    {
        private readonly TrackTurnout track;

        

        public TrackTurnoutViewModel(TrackTypeViewModel trackTypeViewModel) : this(trackTypeViewModel, new TrackTurnout())
        { }

        public TrackTurnoutViewModel(TrackTypeViewModel trackTypeViewModel, TrackTurnout track) : base(trackTypeViewModel, track)
        {
            this.track = track;
        }

        public static TrackViewModel CreateNew(TrackTypeViewModel trackTypeViewModel)
        {
            TrackTurnout trackTurnout = new TrackTurnout
            {
                Article = string.Empty,
                LeftTurnoutRadiusId = trackTypeViewModel.Radii.First().Id,
                LeftTurnoutAngleId = trackTypeViewModel.Angles.First().Id,
                RightTurnoutRadiusId = trackTypeViewModel.Radii.First().Id,
                RightTurnoutAngleId = trackTypeViewModel.Angles.First().Id,
                StraightLengthId = trackTypeViewModel.Lengths.First().Id
            };
            return new TrackTurnoutViewModel(trackTypeViewModel, trackTurnout);
        }

        public string Article
        {
            get { return this.track.Article; }
            set { this.track.Article = value.Trim(); NotifyPropertyChanged(nameof(Article)); }
        }

        public TrackTurnoutDirection TurnoutDirection
        {
            get { return this.track.TurnoutDirection; }
            set { this.track.TurnoutDirection = value; NotifyPropertyChanged(nameof(TurnoutDirection)); }
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

        public TrackNamedValueViewModel LeftCounterCurveRadius
        {
            get { return GetRadius(this.track.LeftCounterCurveRadiusId); }
            set { this.track.LeftCounterCurveRadiusId = value.Id; NotifyPropertyChanged(nameof(LeftCounterCurveRadius)); }
        }

        public TrackNamedValueViewModel LeftCounterCurveAngle
        {
            get { return GetAngle(this.track.LeftCounterCurveAngleId); }
            set { this.track.LeftCounterCurveAngleId = value.Id; NotifyPropertyChanged(nameof(LeftCounterCurveAngle)); }
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

        public TrackNamedValueViewModel RightCounterCurveRadius
        {
            get { return GetRadius(this.track.RightCounterCurveRadiusId); }
            set { this.track.RightCounterCurveRadiusId = value.Id; NotifyPropertyChanged(nameof(RightCounterCurveRadius)); }
        }

        public TrackNamedValueViewModel RightCounterCurveAngle
        {
            get { return GetAngle(this.track.RightCounterCurveAngleId); }
            set { this.track.RightCounterCurveAngleId = value.Id; NotifyPropertyChanged(nameof(RightCounterCurveAngle)); }
        }

        public TrackDrive TurnoutDrive
        {
            get { return this.track.TurnoutDrive; }
            set { this.track.TurnoutDrive = value; NotifyPropertyChanged(nameof(TurnoutDrive)); }
        }
    }
}
