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

        public override TrackViewModel Clone()
        {
            return new TrackTurnoutViewModel(this.trackTypeViewModel, (TrackTurnout)this.track.Clone());
        }

        public string Article
        {
            get { return this.track.Article; }
            set { this.track.Article = value.Trim(); NotifyPropertyChanged(nameof(Article)); }
        }

        public TrackTurnoutType TurnoutType
        {
            get { return this.track.TurnoutType; }
            set { this.track.TurnoutType = value; NotifyPropertyChanged(nameof(TurnoutType)); }
        }

        public TrackDrive TurnoutDrive
        {
            get { return this.track.TurnoutDrive; }
            set { this.track.TurnoutDrive = value; NotifyPropertyChanged(nameof(TurnoutDrive)); }
        }

        public TrackNamedValueViewModel StraightLength
        {
            get { return GetLength(this.track.StraightLengthId); }
            set { this.track.StraightLengthId = value.Id; NotifyPropertyChanged(nameof(StraightLength)); }
        }

        public TrackNamedValueViewModel LeftTurnoutInnerLength
        {
            get { return GetLength(this.track.LeftTurnoutInnerLengthId); }
            set { this.track.LeftTurnoutInnerLengthId = value.Id; NotifyPropertyChanged(nameof(LeftTurnoutInnerLength)); }
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

        public TrackNamedValueViewModel LeftTurnoutOuterLength
        {
            get { return GetLength(this.track.LeftTurnoutOuterLengthId); }
            set { this.track.LeftTurnoutOuterLengthId = value.Id; NotifyPropertyChanged(nameof(LeftTurnoutOuterLength)); }
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

        public TrackNamedValueViewModel RightTurnoutInnerLength
        {
            get { return GetLength(this.track.RightTurnoutInnerLengthId); }
            set { this.track.RightTurnoutInnerLengthId = value.Id; NotifyPropertyChanged(nameof(RightTurnoutInnerLength)); }
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

        public TrackNamedValueViewModel RightTurnoutOuterLength
        {
            get { return GetLength(this.track.RightTurnoutOuterLengthId); }
            set { this.track.RightTurnoutOuterLengthId = value.Id; NotifyPropertyChanged(nameof(RightTurnoutOuterLength)); }
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
    }
}
