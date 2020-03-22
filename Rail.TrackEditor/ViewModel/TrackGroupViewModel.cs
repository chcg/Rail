using Rail.Mvvm;
using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Rail.TrackEditor.ViewModel
{
    public class TrackGroupViewModel : TrackViewModel
    {
        private readonly TrackGroup track;

        public TrackGroupViewModel(TrackTypeViewModel trackTypeViewModel) : this(trackTypeViewModel, new TrackGroup())
        { }

        public TrackGroupViewModel(TrackTypeViewModel trackTypeViewModel, TrackGroup track) : base(trackTypeViewModel, track)
        {
            this.LoadGroupCommand = new DelegateCommand(OnLoadGroup);
            this.track = track;
            this.Names = new ObservableCollection<TrackTypeNameViewModel>(this.track.GroupName.LanguageDictionary.Select(n => new TrackTypeNameViewModel(n)));
        }

        public TrackGroup GetTrackGroup()
        {
            this.track.GroupName.LanguageDictionary = this.Names.ToDictionary(n => n.Language, n => n.Name);
            return this.track;
        }

        public static TrackViewModel CreateNew(TrackTypeViewModel trackTypeViewModel)
        {
            TrackGroup trackGroup = new TrackGroup
            {
               
            };
            return new TrackGroupViewModel(trackTypeViewModel, trackGroup);
        }

        public ObservableCollection<TrackTypeNameViewModel> Names { get; }

        public DelegateCommand LoadGroupCommand { get; }

        private void OnLoadGroup()
        { }
    }
}
