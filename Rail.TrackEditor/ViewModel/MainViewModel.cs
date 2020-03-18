using Rail.Mvvm;
using Rail.TrackEditor.Properties;
using Rail.TrackEditor.View;
using Rail.Tracks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Rail.TrackEditor.ViewModel
{
    public class MainViewModel : AppViewModel
    {
        private TrackList trackList;

        private CollectionViewSource dockTypeSource;

        private readonly List<TrackNameViewModel> nullList = new List<TrackNameViewModel> { TrackNameViewModel.Null };

        public DelegateCommand SaveCommand { get; }
        public DelegateCommand NewTrackTypeCommand { get; }
        public DelegateCommand<TrackTypeViewModel> DeleteTrackTypeCommand { get; }

        public MainViewModel()
        {
            this.SaveCommand = new DelegateCommand(OnSave);
            this.NewTrackTypeCommand = new DelegateCommand(OnNewTrackType);
            this.DeleteTrackTypeCommand = new DelegateCommand<TrackTypeViewModel>(OnDeleteTrackType);
            this.dockTypeSource = new CollectionViewSource();
        }

        public override void OnStartup()
        {
            this.trackList = TrackList.Load();
            this.DockTypes = new ObservableCollection<TrackNameViewModel>(this.trackList.DockTypes.Select(t => new TrackNameViewModel(t)));
            this.TrackTypes = new ObservableCollection<TrackTypeViewModel>(this.trackList.TrackTypes.Select(t => new TrackTypeViewModel(t)));

            this.DockTypes.CollectionChanged += (o, i) => { NotifyPropertyChanged(nameof(DockTypesSource)); NotifyPropertyChanged(nameof(DockTypesAndNullSource)); };
            NotifyPropertyChanged(nameof(DockTypesSource));
        }

        protected void OnSave()
        {
            this.trackList.DockTypes = this.DockTypes.Select(t => t.TrackName).ToList();
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

        private ObservableCollection<TrackNameViewModel> dockTypes;
        public ObservableCollection<TrackNameViewModel> DockTypes
        {
            get { return this.dockTypes; }
            set { this.dockTypes = value; NotifyPropertyChanged(nameof(DockTypes)); }
        }

        //public ICollectionView DockTypesView { get { return this.dockTypeSource.View; } }

        public IEnumerable<TrackNameViewModel> DockTypesSource { get { return this.DockTypes?.ToList(); } }
        public IEnumerable<TrackNameViewModel> DockTypesAndNullSource { get { return this.DockTypes.Concat(nullList).ToList(); } }

        private ObservableCollection<TrackTypeViewModel> trackTypes;
        public ObservableCollection<TrackTypeViewModel> TrackTypes
        {
            get { return this.trackTypes; }
            set 
            { 
                this.trackTypes = value; 
                NotifyPropertyChanged(nameof(TrackTypes));
                this.SelectedTrackType = this.trackTypes.FirstOrDefault();
            }
        }

        private TrackTypeViewModel selectedTtrackType;
        public TrackTypeViewModel SelectedTrackType
        {
            get { return this.selectedTtrackType; }
            set { this.selectedTtrackType = value; NotifyPropertyChanged(nameof(SelectedTrackType)); }
        }
        
        private void OnNewTrackType()
        {
            this.TrackTypes.Add(new TrackTypeViewModel(TrackType.New()));
        }

        private void OnDeleteTrackType(TrackTypeViewModel trackType)
        {
            this.TrackTypes.Remove(trackType);
        }
    }
}
