using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.TrackEditor.ViewModel
{
    public class TrackBumperViewModel : TrackViewModel
    {
        private readonly TrackBumper track;

        public TrackBumperViewModel()
        {
            this.track = new TrackBumper();
        }

        public TrackBumperViewModel(TrackBumper track)
        {
            this.track = track;
        }

        public string Name { get { return this.track.Name; } }

        public string Article
        {
            get { return this.track.Article; }
            set { this.track.Article = value; NotifyPropertyChanged(nameof(Article)); }
        }
    }
}
