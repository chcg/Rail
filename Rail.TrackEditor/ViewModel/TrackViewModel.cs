using Rail.Mvvm;
using Rail.TrackEditor.Controls;
using Rail.Tracks;
using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace Rail.TrackEditor.ViewModel
{
    public abstract class TrackViewModel : BaseViewModel
    {
        protected readonly TrackTypeViewModel trackTypeViewModel;

        public TrackViewModel(TrackTypeViewModel trackTypeViewModel, TrackBase trackBase)
        {
            this.Track = trackBase;
            this.trackTypeViewModel = trackTypeViewModel;
        }

        public string Name { get { return this.Track.Name; } }

        public TrackBase Track { get; } // { get { return this.trackBase; } }

        //public ICommand RailInvalidateCommand { get; set; }
        
        protected override void NotifyPropertyChanged(string propertyName)
        {
            base.NotifyPropertyChanged(propertyName);
            UpdateTrack();
        }

        public static TrackViewModel Create(TrackTypeViewModel trackTypeViewModel, TrackBase track)
        {
            string typeName = track.GetType().Name;
            return typeName switch
            {
                nameof(TrackStraight) => new TrackStraightViewModel(trackTypeViewModel, (TrackStraight)track),
                nameof(TrackCurved) => new TrackCurvedViewModel(trackTypeViewModel, (TrackCurved)track),
                nameof(TrackCrossing) => new TrackCrossingViewModel(trackTypeViewModel, (TrackCrossing)track),
                nameof(TrackEndPiece) => new TrackEndPieceViewModel(trackTypeViewModel, (TrackEndPiece)track),
                nameof(TrackFlex) => new TrackFlexViewModel(trackTypeViewModel, (TrackFlex)track),

                nameof(TrackTurnout) => new TrackTurnoutViewModel(trackTypeViewModel, (TrackTurnout)track),
                nameof(TrackCurvedTurnout) => new TrackCurvedTurnoutViewModel(trackTypeViewModel, (TrackCurvedTurnout)track),
                nameof(TrackDoubleSlipSwitch) => new TrackDoubleSlipSwitchViewModel(trackTypeViewModel, (TrackDoubleSlipSwitch)track),
                nameof(TrackDoubleCrossover) => new TrackDoubleCrossoverViewModel(trackTypeViewModel, (TrackDoubleCrossover)track),
                
                nameof(TrackTable) => new TrackTableViewModel(trackTypeViewModel, (TrackTable)track),

                nameof(TrackGroup) => new TrackGroupViewModel(trackTypeViewModel, (TrackGroup)track),
                _ => null
            };
        }

        public TrackExtras[] Extras { get { return (TrackExtras[])Enum.GetValues(typeof(TrackExtras)); } }
        public TrackTurnoutDirection[] TurnoutDirections { get { return (TrackTurnoutDirection[])Enum.GetValues(typeof(TrackTurnoutDirection)); } }
        public TrackTurnoutType[] TurnoutTypes { get { return (TrackTurnoutType[])Enum.GetValues(typeof(TrackTurnoutType)); } }
        public TrackDrive[] TurnoutDrives { get { return (TrackDrive[])Enum.GetValues(typeof(TrackDrive)); } }
        public TrackEndType[] EndTypes { get { return (TrackEndType[])Enum.GetValues(typeof(TrackEndType)); } }
        public TrackFlexType[] FlexTypes { get { return (TrackFlexType[])Enum.GetValues(typeof(TrackFlexType)); } }
        public TrackTableType[] TableTypes { get { return (TrackTableType[])Enum.GetValues(typeof(TrackTableType)); } }
        public int[] StarNumbers { get { return new int[] { 2, 3, 4 }; } }

        public abstract TrackViewModel Clone();

        public void UpdateTrack()
        {
            this.Track.Update(this.trackTypeViewModel.TrackType);
            base.NotifyPropertyChanged(nameof(Name));
            VisualHelper.InvalidateAll(typeof(TrackControl));
        }

        protected TrackNamedValueViewModel GetLength(Guid lengthId)
        {

            var l = this.trackTypeViewModel.LengthsAndNullSource.FirstOrDefault(i => i.Id == lengthId);
            return l;
        }

        public Guid SetLength(TrackNamedValueViewModel value)
        {
            if (value != null)
            {
                return value.Id;
            }
            return Guid.Empty;
        }

        protected TrackNamedValueViewModel GetAngle(Guid angleId)
        {
            return this.trackTypeViewModel.AnglesAndNullSource.FirstOrDefault(i => i.Id == angleId);
        }

        public Guid SetAngle(TrackNamedValueViewModel value)
        {
            if (value != null)
            {
                return value.Id;
            }
            return Guid.Empty;
        }

        protected TrackNamedValueViewModel GetRadius(Guid radiusId)
        {
            return this.trackTypeViewModel.RadiiAndNullSource.FirstOrDefault(i => i.Id == radiusId);
        }

        public Guid SetRadius(TrackNamedValueViewModel value)
        {
            if (value != null)
            {
                return value.Id;
            }
            return Guid.Empty;
        }
    }
}
