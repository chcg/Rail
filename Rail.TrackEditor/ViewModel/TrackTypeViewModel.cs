using Rail.Mvvm;
using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Rail.TrackEditor.ViewModel
{
    public class TrackTypeViewModel : BaseViewModel
    {
        private readonly TrackType trackType;
        private readonly List<TrackNamedValueViewModel> nullList = new List<TrackNamedValueViewModel> { TrackNamedValueViewModel.Null };
        
        public DelegateCommand<TrackTypes> NewTrackCommand { get; }
        public DelegateCommand<TrackViewModel> CloneTrackCommand { get; }
        public DelegateCommand<TrackViewModel> DeleteTrackCommand { get; }

        public TrackTypeViewModel() : this(new TrackType())
        { }

        public TrackTypeViewModel(TrackType trackType)
        {
            this.NewTrackCommand = new DelegateCommand<TrackTypes>(OnNewTrack);
            this.CloneTrackCommand = new DelegateCommand<TrackViewModel>(OnCloneTrack);
            this.DeleteTrackCommand = new DelegateCommand<TrackViewModel>(OnDeleteTrack);

            this.trackType = trackType;
            this.Tracks = new ObservableCollection<TrackViewModel>(trackType.Tracks.Select(t => TrackViewModel.Create(this, t)));
            this.Names = new MultilanguageStringViewModel(this.trackType.Name);

            this.Lengths = new ObservableCollection<TrackNamedValueViewModel>(this.trackType.Lengths.Select(v => new TrackNamedValueViewModel(v)).OrderBy(l => l.Value));
            this.Radii = new ObservableCollection<TrackNamedValueViewModel>(this.trackType.Radii.Select(v => new TrackNamedValueViewModel(v)).OrderBy(l => l.Value));
            this.Angles = new ObservableCollection<TrackNamedValueViewModel>(this.trackType.Angles.Select(v => new TrackNamedValueViewModel(v)).OrderBy(l => l.Value));
            this.Lengths.CollectionChanged += (o, i) =>
            {
                this.trackType.Lengths = this.Lengths.OrderBy(l => l.Value).Select(l => l.NamedValue).ToList();
                NotifyPropertyChanged(nameof(LengthsSource)); 
                NotifyPropertyChanged(nameof(LengthsAndNullSource)); 
            };
            this.Radii.CollectionChanged += (o, i) => 
            {
                this.trackType.Radii = this.Radii.OrderBy(l => l.Value).Select(l => l.NamedValue).ToList();
                NotifyPropertyChanged(nameof(RadiiSource)); 
                NotifyPropertyChanged(nameof(RadiiAndNullSource)); 
            };
            this.Angles.CollectionChanged += (o, i) =>
            {
                this.trackType.Angles = this.Angles.OrderBy(l => l.Value).Select(l => l.NamedValue).ToList();
                NotifyPropertyChanged(nameof(AnglesSource)); 
                NotifyPropertyChanged(nameof(AnglesAndNullSource)); 
            };
        }

        public TrackType TrackType { get { return this.trackType; } }

        public TrackType GetTrackType()
        {
            this.trackType.Tracks = this.Tracks.Select(t => t.Track).OrderBy(t => t.TrackType).ToList();
            //this.trackType.Name.LanguageDictionary = this.Names.ToDictionary(n => n.Language, n => n.Name);
            this.trackType.Lengths = this.Lengths.OrderBy(l => l.Value).Select(l => l.NamedValue).ToList();
            this.trackType.Radii = this.Radii.OrderBy(l => l.Value).Select(l => l.NamedValue).ToList();
            this.trackType.Angles = this.Angles.OrderBy(l => l.Value).Select(l => l.NamedValue).ToList();
            return this.trackType;
        }

        public MultilanguageStringViewModel Names { get; }

        public ObservableCollection<TrackNamedValueViewModel> Lengths { get; }

        public ObservableCollection<TrackNamedValueViewModel> Radii { get; }

        public ObservableCollection<TrackNamedValueViewModel> Angles { get; }

        public IEnumerable<TrackNamedValueViewModel> LengthsSource { get { return this.Lengths.ToList(); } }
        public IEnumerable<TrackNamedValueViewModel> LengthsAndNullSource { get { return this.Lengths.Concat(nullList).ToList(); } }
        public IEnumerable<TrackNamedValueViewModel> RadiiSource { get { return this.Radii.ToList(); } }
        public IEnumerable<TrackNamedValueViewModel> RadiiAndNullSource { get { return this.Radii.Concat(nullList).ToList(); } }
        public IEnumerable<TrackNamedValueViewModel> AnglesSource { get { return this.Angles.ToList(); } }
        public IEnumerable<TrackNamedValueViewModel> AnglesAndNullSource { get { return this.Angles.Concat(nullList).ToList(); } }


       
        public string Manufacturer
        {
            get { return this.trackType.Parameter.Manufacturer; }
            set { this.trackType.Parameter.Manufacturer = value.Trim(); NotifyPropertyChanged(nameof(Manufacturer)); }
        }

        public TrackGauge[] Gauges { get { return Enum.GetValues(typeof(TrackGauge)).Cast<TrackGauge>().ToArray(); } }

        public TrackGauge Gauge
        {
            get { return this.trackType.Parameter.Gauge; }
            set { this.trackType.Parameter.Gauge = value; NotifyPropertyChanged(nameof(Gauge));  }
        }

        public TrackNameViewModel DockType
        {
            get
            {
                MainViewModel mainViewModel = (MainViewModel)Application.Current.MainWindow.DataContext;
                return mainViewModel.DockTypes.FirstOrDefault(d => d.Id == this.trackType.Parameter.DockType);
            }
            set 
            { 
                this.trackType.Parameter.DockType = value == null ? Guid.Empty : value.Id; 
                NotifyPropertyChanged(nameof(DockType)); 
            }
        }

        public TrackRailType[] RailTypes { get { return Enum.GetValues(typeof(TrackRailType)).Cast<TrackRailType>().ToArray(); } }

        public TrackRailType RailType
        {
            get { return this.trackType.Parameter.RailType; }
            set { this.trackType.Parameter.RailType = value; NotifyPropertyChanged(nameof(RailType)); UpdateAllTracks(); }
        }

        public TrackSleeperType[] SleeperTypes { get { return Enum.GetValues(typeof(TrackSleeperType)).Cast<TrackSleeperType>().ToArray(); } }

        public TrackSleeperType SleeperType
        {
            get { return this.trackType.Parameter.SleeperType; }
            set { this.trackType.Parameter.SleeperType = value; NotifyPropertyChanged(nameof(SleeperType)); UpdateAllTracks(); }
        }

        public double SleeperWidth
        {
            get { return this.trackType.Parameter.SleeperWidth; }
            set { this.trackType.Parameter.SleeperWidth = value; NotifyPropertyChanged(nameof(SleeperWidth)); UpdateAllTracks(); }
        }

        public TrackBallastType[] BallastTypes { get { return Enum.GetValues(typeof(TrackBallastType)).Cast<TrackBallastType>().ToArray(); } }

        public TrackBallastType BallastType
        {
            get { return this.trackType.Parameter.BallastType; }
            set { this.trackType.Parameter.BallastType = value; NotifyPropertyChanged(nameof(BallastType)); UpdateAllTracks(); }
        }

        public double BallastWidth
        {
            get { return this.trackType.Parameter.BallastWidth; }
            set { this.trackType.Parameter.BallastWidth = value; NotifyPropertyChanged(nameof(BallastWidth)); UpdateAllTracks(); }
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
        
        public double WagonMaxBogieFrontDistance
        {
            get { return this.trackType.Parameter.WagonMaxBogieFrontDistance; }
            set { this.trackType.Parameter.WagonMaxBogieFrontDistance = value; NotifyPropertyChanged(nameof(WagonMaxBogieFrontDistance)); }
        }

        private ObservableCollection<TrackViewModel> tracks;
        public ObservableCollection<TrackViewModel> Tracks
        {
            get { return tracks; }
            private set
            {
                this.tracks = value;
                NotifyPropertyChanged(nameof(Tracks));
                this.SelectedTrack = this.Tracks.FirstOrDefault();
            }
        }


        private TrackViewModel selectedTrack;

        public TrackViewModel SelectedTrack
        {
            get { return this.selectedTrack; }
            set { this.selectedTrack = value; NotifyPropertyChanged(nameof(SelectedTrack)); }
        }

        public double LabelWidth { get; set; }

        private void UpdateAllTracks()
        {
            this.Tracks.ToList().ForEach(t => t.UpdateTrack());
        }
        private void OnNewTrack(TrackTypes type)
        {
            TrackViewModel track = type switch
            {
                TrackTypes.Straight => TrackStraightViewModel.CreateNew(this),
                TrackTypes.Curved => TrackCurvedViewModel.CreateNew(this),
                TrackTypes.Crossing => TrackCrossingViewModel.CreateNew(this),
                TrackTypes.EndPiece => TrackEndPieceViewModel.CreateNew(this),
                TrackTypes.Flex => TrackFlexViewModel.CreateNew(this),

                TrackTypes.Turnout => TrackTurnoutViewModel.CreateNew(this),
                TrackTypes.CurvedTurnout => TrackCurvedTurnoutViewModel.CreateNew(this),
                TrackTypes.DoubleSlipSwitch => TrackDoubleSlipSwitchViewModel.CreateNew(this),
                TrackTypes.DoubleCrossover => TrackDoubleCrossoverViewModel.CreateNew(this),
                
                TrackTypes.Table => TrackTableViewModel.CreateNew(this),
                
                TrackTypes.Group => TrackGroupViewModel.CreateNew(this),
                _ => null
            };
            track.UpdateTrack();
            this.Tracks.Add(track);
            this.SelectedTrack = track;
        }

        private void OnCloneTrack(TrackViewModel track)
        {
            var clone = track.Clone();
            this.Tracks.Add(clone);
            this.SelectedTrack = clone;
        }

        private void OnDeleteTrack(TrackViewModel track)
        {
            this.Tracks.Remove(track);
            this.SelectedTrack = this.Tracks.LastOrDefault();
        }
    }
}
