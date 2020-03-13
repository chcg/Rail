using Rail.Mvvm;
using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Rail.TrackEditor.ViewModel
{
    public class TrackStraightViewModel : TrackViewModel
    {
        private readonly TrackStraight track;

        public TrackStraightViewModel(TrackTypeViewModel trackTypeViewModel) : this(trackTypeViewModel, new TrackStraight())
        { }

        public TrackStraightViewModel(TrackTypeViewModel trackTypeViewModel, TrackStraight track) : base(trackTypeViewModel, track)
        {
            this.track = track;
        }
                
        public string Article
        {
            get { return this.track.Article; }
            set { this.track.Article = value.Trim(); NotifyPropertyChanged(nameof(Article)); }
        }

        public TrackNamedValueViewModel Length
        {
            get { return GetLength(this.track.LengthId); }
            set {

                if (value != null)
                {
                    this.track.LengthId = value.Id; NotifyPropertyChanged(nameof(Length));
                }
                else
                {
                    Debugger.Break();
                }
            }
        }
        
        public TrackExtras Extra
        {
            get { return this.track.Extra; }
            set { this.track.Extra = value; NotifyPropertyChanged(nameof(Extra));  }
        }
    }
}
