using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.TrackEditor.ViewModel
{
    public class TrackCrossingViewModel : TrackViewModel
    {
        private readonly TrackCrossing track;

        public TrackCrossingViewModel(TrackTypeViewModel trackTypeViewModel) : this(trackTypeViewModel, new TrackCrossing())
        { }

        public TrackCrossingViewModel(TrackTypeViewModel trackTypeViewModel, TrackCrossing track) : base(trackTypeViewModel, track)
        {
            this.track = track;
        }

        public string Article
        {
            get { return this.track.Article; }
            set { this.track.Article = value.Trim(); NotifyPropertyChanged(nameof(Article)); }
        }

        public TrackNamedValueViewModel LengthA
        {
            get { return GetLength(this.track.LengthAId); }
            set { this.track.LengthAId = value.Id; NotifyPropertyChanged(nameof(LengthA)); }
        }
        
        public TrackNamedValueViewModel LengthB
        {
            get { return GetLength(this.track.LengthBId); }
            set { this.track.LengthBId = value.Id; NotifyPropertyChanged(nameof(LengthB)); }
        }
        
        public TrackNamedValueViewModel CrossingAngle
        {
            get { return GetAngle(this.track.CrossingAngleId); }
            set { this.track.CrossingAngleId = value.Id; NotifyPropertyChanged(nameof(CrossingAngle)); }
        }
    }
}
