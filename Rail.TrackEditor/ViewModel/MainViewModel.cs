using Rail.Mvvm;
using Rail.Tracks;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace Rail.TrackEditor.ViewModel
{
    public class MainViewModel : AppViewModel
    {
        private TrackList trackList;

        public DelegateCommand SaveCommand { get; private set; }
        public DelegateCommand NewTrackTypeCommand { get; private set; }
        public DelegateCommand<TrackTypeViewModel> DeleteTrackTypeCommand { get; private set; }

        public MainViewModel()
        {
            this.SaveCommand = new DelegateCommand(OnSave);
            this.NewTrackTypeCommand = new DelegateCommand(OnNewTrackType);
            this.DeleteTrackTypeCommand = new DelegateCommand<TrackTypeViewModel>(OnDeleteTrackType);
        }

        public override void OnStartup()
        {
            this.trackList = TrackList.Load();
            this.TrackTypes = new ObservableCollection<TrackTypeViewModel>(this.trackList.TrackTypes.Select(t => new TrackTypeViewModel(t)));
        }

        public void OnSave()
        {
            this.trackList.TrackTypes = this.TrackTypes.Select(t => t.GetTrackType()).ToList();
            this.trackList.Save();
        }


        private ObservableCollection<TrackTypeViewModel> trackTypes;
        public ObservableCollection<TrackTypeViewModel> TrackTypes
        {
            get { return this.trackTypes; }
            set { this.trackTypes = value; NotifyPropertyChanged(nameof(TrackTypes)); } 
        }

        public static TrackTypeViewModel SelectedTrackTypeViewModel { get; set; }

        private void OnNewTrackType()
        {
            this.TrackTypes.Add(new TrackTypeViewModel());
        }

        private void OnDeleteTrackType(TrackTypeViewModel trackType)
        {
            this.TrackTypes.Remove(trackType);
        }
    }
}
