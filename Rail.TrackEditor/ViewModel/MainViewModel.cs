using Rail.Model;
using Rail.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace Rail.TrackEditor.ViewModel
{
    public class MainViewModel : AppViewModel
    {
        private TrackList trackList;
        public MainViewModel()
        {
        }

        public override void OnStartup()
        {
            string path = System.AppDomain.CurrentDomain.BaseDirectory;
            this.trackList = TrackList.Load(Path.Combine(path, "Tracks.xml"));
            this.TrackTypes = new ObservableCollection<TrackType>(this.trackList.TrackTypes);

        }


        private ObservableCollection<TrackType> trackTypes;
        public ObservableCollection<TrackType> TrackTypes
        {
            get { return this.trackTypes; }
            set { this.trackTypes = value; NotifyPropertyChanged(nameof(TrackTypes)); } 
        }
    }
}
