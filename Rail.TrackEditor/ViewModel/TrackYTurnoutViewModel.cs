using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.TrackEditor.ViewModel
{
    public class TrackYTurnoutViewModel : TrackViewModel
    {
        private readonly TrackYTurnout track;

        public TrackYTurnoutViewModel() : this(new TrackYTurnout())
        { }

        public TrackYTurnoutViewModel(TrackYTurnout track) : base(track)
        {
            this.track = track;
        }

        public string Article
        {
            get { return this.track.Article; }
            set { this.track.Article = value.Trim(); NotifyPropertyChanged(nameof(Article)); }
        }

        public string LeftTurnoutLength
        {
            get { return this.track.LeftTurnoutLengthName; }
            set { this.track.LeftTurnoutLengthName = value; NotifyPropertyChanged(nameof(LeftTurnoutLength)); }
        }

        public string LeftTurnoutRadius
        {
            get { return this.track.LeftTurnoutRadiusName; }
            set { this.track.LeftTurnoutRadiusName = value; NotifyPropertyChanged(nameof(LeftTurnoutRadius)); }
        }

        public string LeftTurnoutAngle
        {
            get { return this.track.LeftTurnoutAngleName; }
            set { this.track.LeftTurnoutAngleName = value; NotifyPropertyChanged(nameof(LeftTurnoutAngle)); }
        }

        public string RightTurnoutLength
        {
            get { return this.track.RightTurnoutLengthName; }
            set { this.track.RightTurnoutLengthName = value; NotifyPropertyChanged(nameof(RightTurnoutLength)); }
        }

        public string RightTurnoutRadius
        {
            get { return this.track.RightTurnoutRadiusName; }
            set { this.track.RightTurnoutRadiusName = value; NotifyPropertyChanged(nameof(RightTurnoutRadius)); }
        }

        public string RightTurnoutAngle
        {
            get { return this.track.RightTurnoutAngleName; }
            set { this.track.RightTurnoutAngleName = value; NotifyPropertyChanged(nameof(RightTurnoutAngle)); }
        }

        public TrackDrive TurnoutDrive
        {
            get { return this.track.TurnoutDrive; }
            set { this.track.TurnoutDrive = value; NotifyPropertyChanged(nameof(TurnoutDrive)); }
        }
    }
}
