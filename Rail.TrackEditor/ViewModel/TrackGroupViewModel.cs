﻿using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.TrackEditor.ViewModel
{
    public class TrackGroupViewModel : TrackViewModel
    {
        private readonly TrackGroup track;

        public TrackGroupViewModel(TrackTypeViewModel trackTypeViewModel) : this(trackTypeViewModel, new TrackGroup())
        { }

        public TrackGroupViewModel(TrackTypeViewModel trackTypeViewModel, TrackGroup track) : base(trackTypeViewModel, track)
        {
            this.track = track;
        }
                
    }
}
