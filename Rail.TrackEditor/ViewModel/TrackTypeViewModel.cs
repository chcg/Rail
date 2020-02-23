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

        public DelegateCommand<TrackTypes> NewTrackCommand { get; private set; }
        public DelegateCommand<TrackViewModel> DeleteTrackCommand { get; private set; }

        public TrackTypeViewModel() : this(new TrackType())
        {
            //this.NewTrackCommand = new DelegateCommand<TrackTypes>(OnNewTrack);
            //this.DeleteTrackCommand = new DelegateCommand<TrackViewModel>(OnDeleteNewTrack);

            //this.trackType = new TrackType();
            //this.Tracks = new ObservableCollection<TrackViewModel>(this.trackType.Tracks.Select(t => TrackViewModel.Create(t)));
        }

        public TrackTypeViewModel(TrackType trackType)
        {
            this.NewTrackCommand = new DelegateCommand<TrackTypes>(OnNewTrack);
            this.DeleteTrackCommand = new DelegateCommand<TrackViewModel>(OnDeleteNewTrack);

            this.trackType = trackType;
            this.Tracks = new ObservableCollection<TrackViewModel>(trackType.Tracks.Select(t => TrackViewModel.Create(t)));
            this.Names = new ObservableCollection<TrackTypeNameViewModel>(this.trackType.Name.LanguageDictionary.Select(n => new TrackTypeNameViewModel(n)));
            this.Lengths = new ObservableCollection<TrackLengthViewModel>(this.trackType.Lengths.Select(l => new TrackLengthViewModel(l)));
            this.Radii = new ObservableCollection<TrackLengthViewModel>(this.trackType.Radii.Select(l => new TrackLengthViewModel(l)));
        }

        public string Name { get { return this.trackType.Name; } }

        public ObservableCollection<TrackTypeNameViewModel> Names { get; private set; }

        public ObservableCollection<TrackLengthViewModel> Lengths { get; private set; }

        public ObservableCollection<TrackLengthViewModel> Radii { get; private set; }


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

        private void OnNewTrack(TrackTypes type)
        {
            this.Tracks.Add(type switch
            {
                TrackTypes.Straight => new TrackStraightViewModel(),
                TrackTypes.Curved => new TrackCurvedViewModel(),
                TrackTypes.Turnout => new TrackTurnoutViewModel(),
                //TrackTypes.CurvedTurnout,
                //TrackTypes.DoubleSlipSwitch,
                //TrackTypes.DoubleTurnout,
                //TrackTypes.YTurnout,
                //TrackTypes.Crossing,
                //TrackTypes.Bumper,
                //TrackTypes.Adapter,
                //TrackTypes.Turntable,
                //TrackTypes.TransferTable,
                //TrackTypes.EndPiece,
                //TrackTypes.CurvedCircuit,
                //TrackTypes.StraightCircuit,
                //TrackTypes.StraightContact,
                //TrackTypes.StraightUncouple,
                //TrackTypes.StraightIsolatin,
                //TrackTypes.StraightFeeder,
                //TrackTypes.StraightAdjustme,
                //TrackTypes.DoubleCrossover,
                //TrackTypes.Flex,
                //TrackTypes.Group,
                _ => null
            });   
        }

        private void OnDeleteNewTrack(TrackViewModel track)
        {
            this.Tracks.Remove(track);
        }
    }
}
