using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rail.TrackEditor.ViewModel
{
    public class TrackCurvedTurnoutViewModel : TrackViewModel
    {
        private readonly TrackCurvedTurnout track;

        public TrackCurvedTurnoutViewModel(TrackTypeViewModel trackTypeViewModel) : this(trackTypeViewModel, new TrackCurvedTurnout())
        { }

        public TrackCurvedTurnoutViewModel(TrackTypeViewModel trackTypeViewModel, TrackCurvedTurnout track) : base(trackTypeViewModel, track)
        {
            this.track = track;
        }

        public static TrackViewModel CreateNew(TrackTypeViewModel trackTypeViewModel)
        {
            TrackCurvedTurnout trackCurvedTurnout = new TrackCurvedTurnout
            {
                Article = string.Empty,
                InnerRadiusId = trackTypeViewModel.Radii.First().Id,
                InnerAngleId = trackTypeViewModel.Angles.First().Id,
                OuterRadiusId = trackTypeViewModel.Radii.First().Id,
                OuterAngleId = trackTypeViewModel.Angles.First().Id
            };
            return new TrackCurvedTurnoutViewModel(trackTypeViewModel, trackCurvedTurnout);
        }

        public override TrackViewModel Clone()
        {
            return new TrackCurvedTurnoutViewModel(this.trackTypeViewModel, (TrackCurvedTurnout)this.track.Clone());
        }

        public string Article
        {
            get { return this.track.Article; }
            set { this.track.Article = value.Trim(); NotifyPropertyChanged(nameof(Article)); }
        }

        public TrackNamedValueViewModel InnerLength
        {
            get { return GetLength(this.track.InnerLengthId); }
            set { this.track.InnerLengthId = value.Id; NotifyPropertyChanged(nameof(InnerLength)); }
        }

        public TrackNamedValueViewModel InnerRadius
        {
            get { return GetRadius(this.track.InnerRadiusId); }
            set { this.track.InnerRadiusId = value.Id; NotifyPropertyChanged(nameof(InnerRadius)); }
        }

        public TrackNamedValueViewModel InnerAngle
        {
            get { return GetAngle(this.track.InnerAngleId); }
            set { this.track.InnerAngleId = value.Id; NotifyPropertyChanged(nameof(InnerAngle)); }
        }

        public TrackNamedValueViewModel OuterLength
        {
            get { return GetLength(this.track.OuterLengthId); }
            set { this.track.OuterLengthId = value.Id; NotifyPropertyChanged(nameof(OuterLength)); }
        }

        public TrackNamedValueViewModel OuterRadius
        {
            get { return GetRadius(this.track.OuterRadiusId); }
            set { this.track.OuterRadiusId = value.Id; NotifyPropertyChanged(nameof(OuterRadius)); }
        }

        public TrackNamedValueViewModel OuterAngle
        {
            get { return GetAngle(this.track.OuterAngleId); }
            set { this.track.OuterAngleId = value.Id; NotifyPropertyChanged(nameof(OuterAngle)); }
        }

        public TrackTurnoutDirection TurnoutDirection
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
