using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.TrackEditor.ViewModel
{
    public class TrackTurnoutViewModel : TrackViewModel
    {
        private readonly TrackTurnout track;

        public TrackTurnoutViewModel()
        {
            this.track = new TrackTurnout();
        }

        public TrackTurnoutViewModel(TrackTurnout track)
        {
            this.track = track;
        }

        public string Name { get { return this.track.Name; } }

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

        public double Radius
        {
            get { return this.track.Radius; }
            set { this.track.Radius = value; NotifyPropertyChanged(nameof(Radius)); }
        }

        public double Angle
        {
            get { return this.track.Angle; }
            set { this.track.Angle = value; NotifyPropertyChanged(nameof(Angle)); }
        }

        public TrackDirection Direction
        {
            get { return this.track.Direction; }
            set { this.track.Direction = value; NotifyPropertyChanged(nameof(Direction)); }
        }
    }
}
