using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.TrackEditor.ViewModel
{
    public class TrackBumperViewModel : TrackViewModel
    {
        private readonly TrackBumper track;

        public TrackBumperViewModel() : this(new TrackBumper())
        { }

        public TrackBumperViewModel(TrackBumper track) : base(track)
        {
            this.track = track;
        }

        public string Article
        {
            get { return this.track.Article; }
            set { this.track.Article = value; NotifyPropertyChanged(nameof(Article)); }
        }

        public string Length
        {
            get { return this.track.LengthName; }
            set { this.track.LengthName = value; NotifyPropertyChanged(nameof(Length)); }
        }

        public bool Lantern
        {
            get { return this.track.Lantern; }
            set { this.track.Lantern = value; NotifyPropertyChanged(nameof(Lantern)); }
        }
    }
}
