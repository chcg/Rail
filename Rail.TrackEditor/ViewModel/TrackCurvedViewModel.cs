using Rail.Mvvm;
using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.TrackEditor.ViewModel
{
    public class TrackCurvedViewModel : TrackViewModel
    {
        private readonly TrackCurved track;

        public TrackCurvedViewModel()
        {
            this.track = new TrackCurved();
        }

        public TrackCurvedViewModel(TrackCurved track)
        {
            this.track = track;
        }

        public string Name { get { return this.track.Name; } }
    }
}
