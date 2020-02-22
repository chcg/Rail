using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.TrackEditor.ViewModel
{
    public class TrackGroupViewModel : TrackViewModel
    {
        private readonly TrackGroup track;

        public TrackGroupViewModel()
        {
            this.track = new TrackGroup();
        }

        public TrackGroupViewModel(TrackGroup track)
        {
            this.track = track;
        }

        public string Name { get { return this.track.Name; } }
                
    }
}
