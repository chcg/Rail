using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.TrackEditor.ViewModel
{
    public class TrackFlexViewModel : TrackViewModel
    {
        private readonly TrackFlex track;

        public TrackFlexViewModel() : this(new TrackFlex())
        { }

        public TrackFlexViewModel(TrackFlex track) : base(track)
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
