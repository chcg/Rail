using Rail.Tracks;
using System;
using System.Collections.Generic;
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

        public string Article
        {
            get { return this.track.Article; }
            set { this.track.Article = value.Trim(); NotifyPropertyChanged(nameof(Article)); }
        }
    
    }
}
