using Rail.Mvvm;
using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;

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

        public static TrackViewModel CreateNew(TrackTypeViewModel trackTypeViewModel)
        {
            TrackStraight trackStraight = new TrackStraight 
            { 
                Article = string.Empty, 
                LengthId = trackTypeViewModel.Lengths.First().Id 
            };
            return new TrackStraightViewModel(trackTypeViewModel, trackStraight);
        }

        public override TrackViewModel Clone()
        {
            return new TrackStraightViewModel(this.trackTypeViewModel, (TrackStraight)this.track.Clone());
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
                    //Debugger.Break();
                }
            }
        }

        public TrackNameViewModel DockType
        {
            get 
            {
                MainViewModel mainViewModel = (MainViewModel)Application.Current.MainWindow.DataContext;    
                return mainViewModel.DockTypes.FirstOrDefault(d => d.Id == this.track.DockType); 
            }
            set { this.track.DockType = value.Id; NotifyPropertyChanged(nameof(DockType)); }
        }

        public TrackExtras Extra
        {
            get { return this.track.Extra; }
            set { this.track.Extra = value; NotifyPropertyChanged(nameof(Extra));  }
        }
    }
}
