using Rail.Mvvm;
using Rail.Tracks;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Rail.TrackEditor.ViewModel
{
    public class TrackTypeViewModel : BaseViewModel
    {
        private readonly TrackType trackType;

        public TrackTypeViewModel()
        { }

        public TrackTypeViewModel(TrackType trackType)
        {
            this.trackType = trackType;
            this.Tracks = new ObservableCollection<TrackViewModel>(trackType.Tracks.Select(t => new TrackViewModel(t)));
        }

        public string Name { get { return this.trackType.Name; } }

        public string Manufacturer
        {
            get { return this.trackType.Parameter.Manufacturer; }
            set { this.trackType.Parameter.Manufacturer = value; NotifyPropertyChanged(nameof(Manufacturer)); }
        }

        public TrackGauge[] Gauges { get { return Enum.GetValues(typeof(TrackGauge)).Cast<TrackGauge>().ToArray(); } }

        public TrackGauge Gauge
        {
            get { return this.trackType.Parameter.Gauge; }
            set { this.trackType.Parameter.Gauge = value; NotifyPropertyChanged(nameof(Gauge)); }
        }

        public string DockType
        {
            get { return this.trackType.Parameter.DockType; }
            set { this.trackType.Parameter.DockType = value; NotifyPropertyChanged(nameof(DockType)); }
        }

        public TrackRailType[] RailTypes { get { return Enum.GetValues(typeof(TrackRailType)).Cast<TrackRailType>().ToArray(); } }

        public TrackRailType RailType
        {
            get { return this.trackType.Parameter.RailType; }
            set { this.trackType.Parameter.RailType = value; NotifyPropertyChanged(nameof(RailType)); }
        }

        public double RailWidth
        {
            get { return this.trackType.Parameter.RailWidth; }
            set { this.trackType.Parameter.RailWidth = value; NotifyPropertyChanged(nameof(RailWidth)); }
        }

        public TrackSleeperType[] SleeperTypes { get { return Enum.GetValues(typeof(TrackSleeperType)).Cast<TrackSleeperType>().ToArray(); } }

        public TrackSleeperType SleeperType
        {
            get { return this.trackType.Parameter.SleeperType; }
            set { this.trackType.Parameter.SleeperType = value; NotifyPropertyChanged(nameof(SleeperType)); }
        }

        public double SleeperWidth
        {
            get { return this.trackType.Parameter.SleeperWidth; }
            set { this.trackType.Parameter.SleeperWidth = value; NotifyPropertyChanged(nameof(SleeperWidth)); }
        }

        public TrackBallastType[] BallastTypes { get { return Enum.GetValues(typeof(TrackBallastType)).Cast<TrackBallastType>().ToArray(); } }

        public TrackBallastType BallastType
        {
            get { return this.trackType.Parameter.BallastType; }
            set { this.trackType.Parameter.BallastType = value; NotifyPropertyChanged(nameof(BallastType)); }
        }

        public double BallastWidth
        {
            get { return this.trackType.Parameter.BallastWidth; }
            set { this.trackType.Parameter.BallastWidth = value; NotifyPropertyChanged(nameof(BallastWidth)); }
        }

        public double WagonMaxLength
        {
            get { return this.trackType.Parameter.WagonMaxLength; }
            set { this.trackType.Parameter.WagonMaxLength = value; NotifyPropertyChanged(nameof(WagonMaxLength)); }
        }

        public double WagonMaxWidth
        {
            get { return this.trackType.Parameter.WagonMaxWidth; }
            set { this.trackType.Parameter.WagonMaxWidth = value; NotifyPropertyChanged(nameof(WagonMaxWidth)); }
        }

        public double WagonMaxBogieDistance
        {
            get { return this.trackType.Parameter.WagonMaxBogieDistance; }
            set { this.trackType.Parameter.WagonMaxBogieDistance = value; NotifyPropertyChanged(nameof(WagonMaxBogieDistance)); }
        }

        public ObservableCollection<TrackViewModel> Tracks { get; private set; }
    }
}
