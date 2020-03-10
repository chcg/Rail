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
            base.NotifyPropertyChanged(propertyName);
            //UpdateTrack();
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
                nameof(TrackThreeWayTurnout) => new TrackThreeWayTurnoutViewModel((TrackThreeWayTurnout)track, trackType),
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

        protected TrackNamedValueViewModel GetLength(Guid lengthId)
        {
            TrackTypeViewModel ttvm = MainViewModel.SelectedTrackTypeViewModel;
            return ttvm.Lengths.FirstOrDefault(i => i.Id == lengthId);
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
            TrackTypeViewModel ttvm = MainViewModel.SelectedTrackTypeViewModel;
            return ttvm.Angles.FirstOrDefault(i => i.Id == angleId);
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
            TrackTypeViewModel ttvm = MainViewModel.SelectedTrackTypeViewModel;
            return ttvm.Radii.FirstOrDefault(i => i.Id == radiusId);
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
