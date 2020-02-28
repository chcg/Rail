using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.TrackEditor.ViewModel
{
    public class TrackAdapterViewModel : TrackViewModel
    {
        private readonly TrackAdapter track;

        public TrackAdapterViewModel() : this(new TrackAdapter(), MainViewModel.SelectedTrackTypeViewModel.TrackType)
        { }

        public TrackAdapterViewModel(TrackType trackType) : this(new TrackAdapter(), trackType)
        { }

        public TrackAdapterViewModel(TrackAdapter track, TrackType trackType) : base(track, trackType)
        {
            this.track = track;
        }

        public string Article
        {
            get { return this.track.Article; }
            set { this.track.Article = value.Trim(); NotifyPropertyChanged(nameof(Article)); }
        }

        public string Length
        {
            get { return this.track.LengthName; }
            set { this.track.LengthName = value; NotifyPropertyChanged(nameof(Length)); NotifyPropertyChanged(nameof(Track)); }
        }

        public string DockType
        {
            get { return this.track.DockType; }
            set { this.track.DockType = value.Trim(); NotifyPropertyChanged(nameof(DockType)); NotifyPropertyChanged(nameof(Track)); }
        }
    }
}
