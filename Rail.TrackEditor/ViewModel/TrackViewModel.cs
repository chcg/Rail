using Rail.Mvvm;
using Rail.TrackEditor.Controls;
using Rail.Tracks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Rail.TrackEditor.ViewModel
{
    public abstract class TrackViewModel : BaseViewModel
    {
        private readonly TrackBase trackBase;
        protected readonly TrackType trackType;

        public TrackViewModel(TrackBase trackBase, TrackType trackType)
        {
            this.trackBase = trackBase;
            this.trackType = trackType;
        }

        public string Name { get { return this.trackBase.Name; } }

        public TrackBase Track { get { return this.trackBase; } }

        public ICommand RailInvalidateCommand { get; set; }
        
        protected override void NotifyPropertyChanged(string propertyName)
        {
            UpdateTrack();
            base.NotifyPropertyChanged(propertyName);
        }

        public static TrackViewModel Create(TrackBase track, TrackType trackType)
        {
            string typeName = track.GetType().Name;
            return typeName switch
            {
                nameof(TrackStraight) => new TrackStraightViewModel((TrackStraight)track, trackType),
                nameof(TrackCurved) => new TrackCurvedViewModel((TrackCurved)track, trackType),
                nameof(TrackTurnout) => new TrackTurnoutViewModel((TrackTurnout)track, trackType),
                nameof(TrackCurvedTurnout) => new TrackCurvedTurnoutViewModel((TrackCurvedTurnout)track, trackType),
                nameof(TrackDoubleSlipSwitch) => new TrackDoubleSlipSwitchViewModel((TrackDoubleSlipSwitch)track, trackType),
                nameof(TrackDoubleTurnout) => new TrackDoubleTurnoutViewModel((TrackDoubleTurnout)track, trackType),
                nameof(TrackYTurnout) => new TrackYTurnoutViewModel((TrackYTurnout)track, trackType),
                nameof(TrackCrossing) => new TrackCrossingViewModel((TrackCrossing)track, trackType),
                nameof(TrackBumper) => new TrackBumperViewModel((TrackBumper)track, trackType),
                nameof(TrackAdapter) => new TrackAdapterViewModel((TrackAdapter)track, trackType),
                nameof(TrackTurntable) => new TrackTurntableViewModel((TrackTurntable)track, trackType),
                nameof(TrackTransferTable) => new TrackTransferTableViewModel((TrackTransferTable)track, trackType),
                nameof(TrackEndPiece) => new TrackEndPieceViewModel((TrackEndPiece)track, trackType),
                nameof(TrackStraightAdjustment) => new TrackStraightAdjustmentViewModel((TrackStraightAdjustment)track, trackType),
                nameof(TrackDoubleCrossover) => new TrackDoubleCrossoverViewModel((TrackDoubleCrossover)track, trackType),
                nameof(TrackFlex) => new TrackFlexViewModel((TrackFlex)track, trackType),
                nameof(TrackGroup) => new TrackGroupViewModel((TrackGroup)track, trackType),
                _ => null
            };
        }

        public void UpdateTrack()
        {
            this.trackBase.Update(this.trackType);
            VisualHelper.InvalidateAll(typeof(TrackControl));
        }
    }
}
