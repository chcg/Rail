using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.TrackEditor.ViewModel
{
    public class TrackBumperViewModel : TrackViewModel
    {
        private readonly TrackBumper track;
               
        public TrackBumperViewModel(TrackTypeViewModel trackTypeViewModel) : this(trackTypeViewModel, new TrackBumper())
        { }

        public TrackBumperViewModel(TrackTypeViewModel trackTypeViewModel, TrackBumper track) : base(trackTypeViewModel, track)
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

        public bool Lantern
        {
            get { return this.track.Lantern; }
            set { this.track.Lantern = value; NotifyPropertyChanged(nameof(Lantern)); }
        }
    }
}
