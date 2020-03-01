using Rail.Mvvm;
using Rail.TrackEditor.Properties;
using Rail.TrackEditor.View;
using Rail.Tracks;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;

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

        protected void OnSave()
        {
            this.trackList.TrackTypes = this.TrackTypes.Select(t => t.GetTrackType()).ToList();
            this.trackList.Save();
        }

        protected override void OnOptions()
        {
            if (new OptionsView { DataContext = new OptionsViewModel() }.ShowDialog().Value)
            {
                CultureInfo newCultureInfo = string.IsNullOrEmpty(Settings.Default.Language) ? CultureInfo.InstalledUICulture : new CultureInfo(Settings.Default.Language);
                if (newCultureInfo.Name != CultureInfo.CurrentUICulture.Name)
                {
                    CultureInfo.CurrentUICulture = CultureInfo.CurrentCulture = newCultureInfo;

                    Window oldWindow = Application.Current.MainWindow;
                    Application.Current.MainWindow = new MainView
                    {
                        WindowState = oldWindow.WindowState,
                        Left = oldWindow.Left,
                        Top = oldWindow.Top,
                        Width = oldWindow.Width,
                        Height = oldWindow.Height,
                        DataContext = this
                    };
                    Application.Current.MainWindow.Show();
                    oldWindow.Close();
                }
            }
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
