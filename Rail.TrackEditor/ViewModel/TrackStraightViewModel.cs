﻿using Rail.Mvvm;
using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rail.TrackEditor.ViewModel
{
    public class TrackStraightViewModel : TrackViewModel
    {
        private readonly TrackStraight track;

        public TrackStraightViewModel() : this(new TrackStraight(), MainViewModel.SelectedTrackTypeViewModel.TrackType)
        { }

        public TrackStraightViewModel(TrackType trackType) : this(new TrackStraight(), trackType)
        { }

        public TrackStraightViewModel(TrackStraight track, TrackType trackType) : base(track, trackType)
        {
            this.track = track;
        }
                
        public string Article
        {
            get { return this.track.Article; }
            set { this.track.Article = value.Trim(); NotifyPropertyChanged(nameof(Article)); }
        }

        public string LengthName
        {
            get { return this.track.LengthName; }
            set { this.track.LengthName = value;  NotifyPropertyChanged(nameof(LengthName)); }
        }
               

        public TrackNamedValueViewModel Length
        {
            get 
            {
                if (Guid.TryParse(this.track.LengthName, out Guid guid))
                {
                    TrackTypeViewModel ttvm = MainViewModel.SelectedTrackTypeViewModel;
                    return ttvm.Lengths.FirstOrDefault(i => i.Id == guid);
                }
                else
                {
                    TrackTypeViewModel ttvm = MainViewModel.SelectedTrackTypeViewModel;
                    return ttvm.Lengths.FirstOrDefault(i => i.Name == this.track.LengthName);
                }
            }
            set 
            {
                if (value != null)
                {
                    this.track.LengthName = value.Id.ToString();
                    NotifyPropertyChanged(nameof(LengthName));
                }
            }
        }
        public TrackExtras[] Extras {  get { return (TrackExtras[])Enum.GetValues(typeof(TrackExtras));  } }
        
        public TrackExtras Extra
        {
            get { return this.track.Extra; }
            set { this.track.Extra = value; NotifyPropertyChanged(nameof(Extra));  }
        }
    }
}
