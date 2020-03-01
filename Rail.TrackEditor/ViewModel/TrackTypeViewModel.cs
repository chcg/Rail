using Rail.Mvvm;
using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Rail.TrackEditor.ViewModel
{
    public class TrackTypeViewModel : BaseViewModel
    {
        private readonly TrackType trackType;

        public DelegateCommand<TrackTypes> NewTrackCommand { get; private set; }
        public DelegateCommand<TrackViewModel> DeleteTrackCommand { get; private set; }

        public TrackTypeViewModel() : this(new TrackType())
        { }

        public TrackTypeViewModel(TrackType trackType)
        {
            this.NewTrackCommand = new DelegateCommand<TrackTypes>(OnNewTrack);
            this.DeleteTrackCommand = new DelegateCommand<TrackViewModel>(OnDeleteNewTrack);

            this.trackType = trackType;
            this.Tracks = new ObservableCollection<TrackViewModel>(trackType.Tracks.Select(t => TrackViewModel.Create(t, trackType)));
            this.Names = new ObservableCollection<TrackTypeNameViewModel>(this.trackType.Name.LanguageDictionary.Select(n => new TrackTypeNameViewModel(n)));
            this.Lengths = new ObservableCollection<TrackNamedValueViewModel>(this.trackType.Lengths.Select(v => new TrackNamedValueViewModel(v)));
            this.Radii = new ObservableCollection<TrackNamedValueViewModel>(this.trackType.Radii.Select(v => new TrackNamedValueViewModel(v)));
            this.Angles = new ObservableCollection<TrackNamedValueViewModel>(this.trackType.Angles.Select(v => new TrackNamedValueViewModel(v)));

            this.Lengths.CollectionChanged += (o, i) => NotifyPropertyChanged(nameof(LengthNames));
            this.Radii.CollectionChanged += (o, i) => NotifyPropertyChanged(nameof(RadiusNames));
            this.Angles.CollectionChanged += (o, i) => NotifyPropertyChanged(nameof(AngleNames));
        }

        public TrackType TrackType { get { return this.trackType; } }

        public TrackType GetTrackType()
        {
            this.trackType.Tracks = this.Tracks.Select(t => t.Track).ToList();
            this.trackType.Name.LanguageDictionary = this.Names.ToDictionary(n => n.Language, n => n.Name);
            this.trackType.Lengths = this.Lengths.Select(l => l.NamedValue).ToList();
            this.trackType.Radii = this.Radii.Select(l => l.NamedValue).ToList();
            this.trackType.Angles = this.Angles.Select(l => l.NamedValue).ToList();
            return this.trackType;
        }

        public string Name { get { return this.trackType.Name; } }

        public ObservableCollection<TrackTypeNameViewModel> Names { get; private set; }

        public ObservableCollection<TrackNamedValueViewModel> Lengths { get; private set; }

        public ObservableCollection<TrackNamedValueViewModel> LengthsWithNull { get; private set; }

        public ObservableCollection<TrackNamedValueViewModel> Radii { get; private set; }

        public ObservableCollection<TrackNamedValueViewModel> Angles { get; private set; }


        public List<string> LengthNames { get { return this.Lengths.Select(l => l.Name).ToList(); } }
        public List<string> RadiusNames { get { return this.Radii.Select(l => l.Name).ToList(); } }
        public List<string> AngleNames { get { return this.Angles.Select(l => l.Name).ToList(); } }

        public List<string> LengthNamesAndNull { get { 
                
            var x= new List<string> { "0" }.Concat(this.LengthNames).ToList();
                return x;
            
            } }

        public TrackDirection[] TurnoutDirections { get { return (TrackDirection[])Enum.GetValues(typeof(TrackDirection)); } }
        public TrackDrive[] TurnoutDrives { get { return (TrackDrive[])Enum.GetValues(typeof(TrackDrive)); } }

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

        public string DockType
        {
            get { return this.trackType.Parameter.DockType; }
            set { this.trackType.Parameter.DockType = value.Trim(); NotifyPropertyChanged(nameof(DockType)); }
        }

        public TrackRailType[] RailTypes { get { return Enum.GetValues(typeof(TrackRailType)).Cast<TrackRailType>().ToArray(); } }

        public TrackRailType RailType
        {
            get { return this.trackType.Parameter.RailType; }
            set { this.trackType.Parameter.RailType = value; NotifyPropertyChanged(nameof(RailType)); UpdateAllTracks(); }
        }

        public double RailWidth
        {
            get { return this.trackType.Parameter.RailWidth; }
            set { this.trackType.Parameter.RailWidth = value; NotifyPropertyChanged(nameof(RailWidth)); UpdateAllTracks(); }
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

        public ObservableCollection<TrackViewModel> Tracks { get; private set; }

        public double LabelWidth { get; set; }

        private void UpdateAllTracks()
        {
            this.Tracks.ToList().ForEach(t => t.UpdateTrack());
        }
        private void OnNewTrack(TrackTypes type)
        {
            this.Tracks.Add(type switch
            {
                TrackTypes.Straight => new TrackStraightViewModel(this.trackType),
                TrackTypes.Curved => new TrackCurvedViewModel(this.trackType),
                TrackTypes.Turnout => new TrackTurnoutViewModel(this.trackType),
                TrackTypes.CurvedTurnout => new TrackCurvedTurnoutViewModel(this.trackType),
                TrackTypes.DoubleSlipSwitch => new TrackDoubleSlipSwitchViewModel(this.trackType),
                TrackTypes.ThreeWayTurnout => new TrackThreeWayTurnoutViewModel(this.trackType),
                TrackTypes.YTurnout => new TrackYTurnoutViewModel(this.trackType),
                TrackTypes.Crossing => new TrackCrossingViewModel(this.trackType),
                TrackTypes.Bumper => new TrackBumperViewModel(this.trackType),
                TrackTypes.Adapter => new TrackAdapterViewModel(this.trackType),
                TrackTypes.Turntable => new TrackTurntableViewModel(this.trackType),
                TrackTypes.TransferTable => new TrackTransferTableViewModel(this.trackType),
                TrackTypes.EndPiece => new TrackEndPieceViewModel(this.trackType),                
                TrackTypes.StraightAdjustment => new TrackStraightAdjustmentViewModel(this.trackType),
                TrackTypes.DoubleCrossover => new TrackDoubleCrossoverViewModel(this.trackType),
                TrackTypes.Flex => new TrackFlexViewModel(this.trackType),
                TrackTypes.Group => new TrackGroupViewModel(this.trackType),
                _ => null
            });   
        }

        private void OnDeleteNewTrack(TrackViewModel track)
        {
            this.Tracks.Remove(track);
        }
    }
}
