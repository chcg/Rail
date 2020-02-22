using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.TrackEditor.ViewModel
{
    public class TrackDoubleSlipSwitchViewModel : TrackViewModel
    {
        private readonly TrackDoubleSlipSwitch track;

        public TrackDoubleSlipSwitchViewModel()
        {
            this.track = new TrackDoubleSlipSwitch();
        }

        public TrackDoubleSlipSwitchViewModel(TrackDoubleSlipSwitch track)
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
