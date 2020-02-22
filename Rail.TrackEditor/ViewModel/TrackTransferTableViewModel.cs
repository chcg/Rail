using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.TrackEditor.ViewModel
{
    public class TrackTransferTableViewModel : TrackViewModel
    {
        private readonly TrackTransferTable track;

        public TrackTransferTableViewModel()
        {
            this.track = new TrackTransferTable();
        }

        public TrackTransferTableViewModel(TrackTransferTable track)
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
