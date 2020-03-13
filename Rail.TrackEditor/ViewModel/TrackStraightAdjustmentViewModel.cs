using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.TrackEditor.ViewModel
{
    public class TrackStraightAdjustmentViewModel : TrackViewModel
    {
        private readonly TrackStraightAdjustment track;
        
        public TrackStraightAdjustmentViewModel(TrackTypeViewModel trackTypeViewModel) : this(trackTypeViewModel, new TrackStraightAdjustment())
        { }

        public TrackStraightAdjustmentViewModel(TrackTypeViewModel trackTypeViewModel, TrackStraightAdjustment track) : base(trackTypeViewModel, track)
        {
            this.track = track;
        }

        public string Article
        {
            get { return this.track.Article; }
            set { this.track.Article = value.Trim(); NotifyPropertyChanged(nameof(Article)); }
        }
    }
}
