using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.TrackEditor.ViewModel
{
    public class TrackStraightAdjustmentViewModel : TrackViewModel
    {
        private readonly TrackStraightAdjustment track;

        public TrackStraightAdjustmentViewModel() : this(new TrackStraightAdjustment())
        { }

        public TrackStraightAdjustmentViewModel(TrackStraightAdjustment track) : base(track)
        {
            this.track = track;
        }

        public string Article
        {
            get { return this.track.Article; }
            set { this.track.Article = value; NotifyPropertyChanged(nameof(Article)); }
        }
    }
}
