﻿using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.TrackEditor.ViewModel
{
    public class TrackTurntableViewModel : TrackViewModel
    {
        private readonly TrackTurntable track;

        public TrackTurntableViewModel(TrackTypeViewModel trackTypeViewModel) : this(trackTypeViewModel, new TrackTurntable())
        { }

        public TrackTurntableViewModel(TrackTypeViewModel trackTypeViewModel, TrackTurntable track) : base(trackTypeViewModel, track)
        {
            this.track = track;
        }

        public string Article
        {
            get { return this.track.Article; }
            set { this.track.Article = value.Trim(); NotifyPropertyChanged(nameof(Article)); }
        }

        public double OuterRadius
        {
            get { return this.track.OuterRadius; }
            set { this.track.OuterRadius = value; NotifyPropertyChanged(nameof(OuterRadius)); }
        }

        public double InnerRadius
        {
            get { return this.track.InnerRadius; }
            set { this.track.InnerRadius = value; NotifyPropertyChanged(nameof(InnerRadius)); }
        }

        public TrackNamedValueViewModel Angle
        {
            get { return GetAngle( this.track.AngleId); }
            set { this.track.AngleId = value.Id; NotifyPropertyChanged(nameof(Angle)); }
        }

        public int RailNum
        {
            get { return this.track.RailNum; }
            set { this.track.RailNum = value; NotifyPropertyChanged(nameof(RailNum)); }
        }

    }
}
