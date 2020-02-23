using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.TrackEditor.ViewModel
{
    public class TrackGroupViewModel : TrackViewModel
    {
        private readonly TrackGroup track;

        public TrackGroupViewModel() : this(new TrackGroup())
        { }

        public TrackGroupViewModel(TrackGroup track) : base(track)
        {
            this.track = track;
        }
                
    }
}
