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

        public DelegateCommand NewTrackTypeCommand { get; private set; }
        public DelegateCommand<TrackTypeViewModel> DeleteTrackTypeCommand { get; private set; }

        public MainViewModel()
        {
            this.NewTrackTypeCommand = new DelegateCommand(OnNewTrackType);
            this.DeleteTrackTypeCommand = new DelegateCommand<TrackTypeViewModel>(OnDeleteTrackType);
        }

        public override void OnStartup()
        {
            string path = System.AppDomain.CurrentDomain.BaseDirectory;
            this.trackList = TrackList.Load(Path.Combine(path, "Tracks.xml"));
            this.TrackTypes = new ObservableCollection<TrackTypeViewModel>(this.trackList.TrackTypes.Select(t => new TrackTypeViewModel(t)));
        }


        private ObservableCollection<TrackTypeViewModel> trackTypes;
        public ObservableCollection<TrackTypeViewModel> TrackTypes
        {
            get { return this.trackTypes; }
            set { this.trackTypes = value; NotifyPropertyChanged(nameof(TrackTypes)); } 
        }

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
