using Rail.Mvvm;
using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.TrackEditor.ViewModel
{
    public class TrackStraightViewModel : TrackViewModel
    {
        private readonly TrackStraight track;

        public TrackStraightViewModel()
        {
            this.track = new TrackStraight();
        }

        public TrackStraightViewModel(TrackStraight track)
        {
            this.track = track;
        }

        public string Name {  get { return this.track.Name; } }

        public string Article
        {
            get { return this.track.Article; }
            set { this.track.Article = value; NotifyPropertyChanged(nameof(Article)); }
        }

        public double Length
        {
            get { return this.track.Length; }
            set { this.track.Length = value; NotifyPropertyChanged(nameof(Length)); }
        }
    }
}
