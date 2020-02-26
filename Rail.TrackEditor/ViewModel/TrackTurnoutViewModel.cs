using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.TrackEditor.ViewModel
{
    public class TrackTurnoutViewModel : TrackViewModel
    {
        private readonly TrackTurnout track;

        public TrackTurnoutViewModel() : this(new TrackTurnout())
        { }

        public TrackTurnoutViewModel(TrackTurnout track) : base(track)
        {
            this.track = track;
        }

        public string Article
        {
            get { return this.track.Article; }
            set { this.track.Article = value; NotifyPropertyChanged(nameof(Article)); }
        }

        public string StraightLength
        {
            get { return this.track.StraightLengthName; }
            set { this.track.StraightLengthName = value; NotifyPropertyChanged(nameof(StraightLength)); }
        }

        public string TurnoutLength
        {
            get { return this.track.TurnoutLengthName; }
            set { this.track.TurnoutLengthName = value; NotifyPropertyChanged(nameof(TurnoutLength)); }
        }

        public string TurnoutRadius
        {
            get { return this.track.TurnoutRadiusName; }
            set { this.track.TurnoutRadiusName = value; NotifyPropertyChanged(nameof(TurnoutRadius)); }
        }

        public string TurnoutAngle
        {
            get { return this.track.TurnoutAngleName; }
            set { this.track.TurnoutAngleName = value; NotifyPropertyChanged(nameof(TurnoutAngle)); }
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
