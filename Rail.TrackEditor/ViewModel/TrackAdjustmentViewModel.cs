using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.TrackEditor.ViewModel
{
    public class TrackAdjustmentViewModel : TrackViewModel
    {
        private readonly TrackAdjustment track;
        
        public TrackAdjustmentViewModel(TrackTypeViewModel trackTypeViewModel) : this(trackTypeViewModel, new TrackAdjustment())
        { }

        public TrackAdjustmentViewModel(TrackTypeViewModel trackTypeViewModel, TrackAdjustment track) : base(trackTypeViewModel, track)
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
