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

        public TrackViewModel(TrackBase trackBase)
        {
            this.trackBase = trackBase;
        }

        public string Name { get { return this.trackBase.Name; } }

        public TrackBase Track { get { return this.trackBase; } }

        public ICommand RailInvalidateCommand { get; set; }

        public static TrackViewModel Create(TrackBase track)
        {
            string typeName = track.GetType().Name;
            return typeName switch
            {
                nameof(TrackStraight) => new TrackStraightViewModel((TrackStraight)track),
                nameof(TrackCurved) => new TrackCurvedViewModel((TrackCurved)track),
                nameof(TrackTurnout) => new TrackTurnoutViewModel((TrackTurnout)track),
                nameof(TrackCurvedTurnout) => new TrackCurvedTurnoutViewModel((TrackCurvedTurnout)track),
                nameof(TrackDoubleSlipSwitch) => new TrackDoubleSlipSwitchViewModel((TrackDoubleSlipSwitch)track),
                nameof(TrackDoubleTurnout) => new TrackDoubleTurnoutViewModel((TrackDoubleTurnout)track),
                nameof(TrackYTurnout) => new TrackYTurnoutViewModel((TrackYTurnout)track),
                nameof(TrackCrossing) => new TrackCrossingViewModel((TrackCrossing)track),
                nameof(TrackBumper) => new TrackBumperViewModel((TrackBumper)track),
                nameof(TrackAdapter) => new TrackAdapterViewModel((TrackAdapter)track),
                nameof(TrackTurntable) => new TrackTurntableViewModel((TrackTurntable)track),
                nameof(TrackTransferTable) => new TrackTransferTableViewModel((TrackTransferTable)track),
                nameof(TrackEndPiece) => new TrackEndPieceViewModel((TrackEndPiece)track),
                nameof(TrackStraightAdjustment) => new TrackStraightAdjustmentViewModel((TrackStraightAdjustment)track),
                nameof(TrackDoubleCrossover) => new TrackDoubleCrossoverViewModel((TrackDoubleCrossover)track),
                nameof(TrackFlex) => new TrackFlexViewModel((TrackFlex)track),
                nameof(TrackGroup) => new TrackGroupViewModel((TrackGroup)track),
                _ => null
            };
        }

        public void UpdateTrack(TrackType trackType)
        {
            this.trackBase.Update(trackType);
            NotifyPropertyChanged(nameof(Track));

            VisualHelper.InvalidateAll(typeof(TrackControl));
        }

    }
}
