﻿using Rail.Mvvm;
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

        public string Article
        {
            get { return this.track.Article; }
            set { this.track.Article = value; NotifyPropertyChanged(nameof(Article)); }
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
    }
}
