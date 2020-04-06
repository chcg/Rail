using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rail.TrackEditor.ViewModel
{
    public class TrackFlexViewModel : TrackViewModel
    {
        private readonly TrackFlex track;

        public TrackFlexViewModel(TrackTypeViewModel trackTypeViewModel) : this(trackTypeViewModel, new TrackFlex())
        { }

        public TrackFlexViewModel(TrackTypeViewModel trackTypeViewModel, TrackFlex track) : base(trackTypeViewModel, track)
        {
            this.track = track;
        }

        public static TrackViewModel CreateNew(TrackTypeViewModel trackTypeViewModel)
        {
            TrackFlex trackFlex = new TrackFlex
            {
                Article = string.Empty,
                MinLengthId = trackTypeViewModel.Lengths.First().Id,
                MaxLengthId = trackTypeViewModel.Lengths.First().Id
            };
            return new TrackFlexViewModel(trackTypeViewModel, trackFlex);
        }

        public override TrackViewModel Clone()
        {
            return new TrackFlexViewModel(this.trackTypeViewModel, (TrackFlex)this.track.Clone());
        }

        public string Article
        {
            get { return this.track.Article; }
            set { this.track.Article = value.Trim(); NotifyPropertyChanged(nameof(Article)); }
        }

        public TrackFlexType FlexType
        {
            get { return this.track.FlexType; }
            set { this.track.FlexType = value; NotifyPropertyChanged(nameof(FlexType)); }
        }

        public TrackNamedValueViewModel MinLength
        {
            get { return GetLength(this.track.MinLengthId); }
            set { this.track.MinLengthId = value.Id; NotifyPropertyChanged(nameof(MinLength)); }
        }

        public TrackNamedValueViewModel MaxLength
        {
            get { return GetLength(this.track.MaxLengthId); }
            set { this.track.MaxLengthId = value.Id; NotifyPropertyChanged(nameof(MaxLength)); }
        }
    }
}
