using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.TrackEditor.ViewModel
{
    public class TrackAdapterViewModel : TrackViewModel
    {
        private readonly TrackAdapter track;
        
        public TrackAdapterViewModel(TrackTypeViewModel trackTypeViewModel) : this(trackTypeViewModel, new TrackAdapter())
        { }

        public TrackAdapterViewModel(TrackTypeViewModel trackTypeViewModel, TrackAdapter track) : base(trackTypeViewModel, track)
        {
            this.track = track;
        }

        public string Article
        {
            get { return this.track.Article; }
            set { this.track.Article = value.Trim(); NotifyPropertyChanged(nameof(Article)); }
        }

        public TrackNamedValueViewModel Length
        {
            get { return GetLength(this.track.LengthId); }
            set { this.track.LengthId = value.Id; NotifyPropertyChanged(nameof(Length)); }
        }

        public string DockType
        {
            get { return this.track.DockType; }
            set { this.track.DockType = value.Trim(); NotifyPropertyChanged(nameof(DockType)); }
        }
    }
}
