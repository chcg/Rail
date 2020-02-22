using Rail.Mvvm;
using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.TrackEditor.ViewModel
{
    public class TrackLengthViewModel : BaseViewModel
    {
        private TrackLength trackLength;

        public TrackLengthViewModel()
        {
            this.trackLength = new TrackLength();
        }

        public TrackLengthViewModel(TrackLength trackLength)
        {
            this.trackLength = trackLength;
        }

        public string Name
        {
            get { return this.trackLength.Name; }
            set { this.trackLength.Name = value; NotifyPropertyChanged(nameof(Name)); }
        }

        public double Length
        {
            get { return this.trackLength.Length; }
            set { this.trackLength.Length = value; NotifyPropertyChanged(nameof(Length)); }
        }
    }
}
