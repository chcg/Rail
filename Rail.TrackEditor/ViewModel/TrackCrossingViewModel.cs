using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public static TrackViewModel CreateNew(TrackTypeViewModel trackTypeViewModel)
        {
            TrackCrossing trackCrossing = new TrackCrossing
            {
                Article = string.Empty,
                CrossingType = TrackCrossingType.Simple,
                LengthId = trackTypeViewModel.Lengths.First().Id,
                LengthBId = trackTypeViewModel.Lengths.First().Id,
                CrossingAngleId = trackTypeViewModel.Angles.First().Id
            };
            return new TrackCrossingViewModel(trackTypeViewModel, trackCrossing);
        }

        public override TrackViewModel Clone()
        {
            return new TrackCrossingViewModel(this.trackTypeViewModel, (TrackCrossing)this.track.Clone());
        }

        public string Article
        {
            get { return this.track.Article; }
            set { this.track.Article = value.Trim(); NotifyPropertyChanged(nameof(Article)); }
        }

        public TrackCrossingType CrossingType
        {
            get { return this.track.CrossingType; }
            set { this.track.CrossingType = value; NotifyPropertyChanged(nameof(CrossingType)); }
        }

        public TrackNamedValueViewModel Length
        {
            get { return GetLength(this.track.LengthId); }
            set { this.track.LengthId = value.Id; NotifyPropertyChanged(nameof(Length)); }
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
