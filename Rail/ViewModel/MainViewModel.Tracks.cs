using Rail.Enums;
using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;

namespace Rail.ViewModel
{
    public sealed partial class MainViewModel
    {
        private TrackList trackList;
        private Dictionary<Guid, TrackBase> trackDict;

        private void LoadTrackList()
        {
            DependencyObject dep = new DependencyObject();
            if (!DesignerProperties.GetIsInDesignMode(dep))
            {

                try
                {
                    this.trackList = TrackList.Load();
                    this.trackDict = trackList.TrackTypes.SelectMany(t => t.Tracks).ToDictionary(t => t.Id, t => t);
                    this.TrackTypeManufacturers = this.trackList.TrackTypes.Select(t => t.Parameter.Manufacturer).Distinct().OrderBy(t => t).ToList();
                    this.SelectedTrackTypeManufacturer = this.TrackTypeManufacturers.FirstOrDefault();
                    FillTrackTypes();
                }
                catch (Exception ex)
                {
                    Exception e = ex;
                    string message = $"Error in File Tracks.xml\r\n{ex.Message}";
                    while (e.InnerException != null)
                    {
                        e = e.InnerException;
                        message += $"\r\n{e.Message}";
                    }
                    MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Debugger.Break();

                    throw ex;
                }
            }
        }

        
        private TrackTypeFilterType trackTypeFilterType = TrackTypeFilterType.All;
        public TrackTypeFilterType TrackTypeFilterType
        {
            get
            {
                return this.trackTypeFilterType;
            }
            set
            {
                this.trackTypeFilterType = value;
                NotifyPropertyChanged(nameof(TrackTypeFilterType));
                FillTrackTypes();
            }
        }
        
        public List<TrackGauge> TrackTypeGauges { get { return Enum.GetValues(typeof(TrackGauge)).Cast<TrackGauge>().ToList(); } }
        
        public TrackGauge selectedTrackTypeGauge = TrackGauge.Gauge_H0;
        public TrackGauge SelectedTrackTypeGauge
        {
            get
            {
                return this.selectedTrackTypeGauge;
            }
            set
            {
                this.selectedTrackTypeGauge = value;
                NotifyPropertyChanged(nameof(SelectedTrackTypeGauge));
                FillTrackTypes();
            }
        }

        public List<string> TrackTypeManufacturers { get; private set; }

        public string selectedTrackTypeManufacturer;
        public string SelectedTrackTypeManufacturer
        {
            get
            {
                return this.selectedTrackTypeManufacturer;
            }
            set
            {
                this.selectedTrackTypeManufacturer = value;
                NotifyPropertyChanged(nameof(SelectedTrackTypeManufacturer));
                FillTrackTypes();
            }
        }

        

        private void FillTrackTypes()
        {
            this.TrackTypes = this.TrackTypeFilterType switch
            {
                TrackTypeFilterType.All => this.trackList.TrackTypes,
                TrackTypeFilterType.Gauge => this.trackList.TrackTypes.Where(t => t.Parameter.Gauge == SelectedTrackTypeGauge).ToList(),
                TrackTypeFilterType.Manufacturer => this.trackList.TrackTypes.Where(t => t.Parameter.Manufacturer == SelectedTrackTypeManufacturer).ToList(),
                _ => null
            };
            NotifyPropertyChanged(nameof(TrackTypes));
            this.SelectedTrackType = this.TrackTypes?.FirstOrDefault();
        }

        public List<TrackType> trackTypes;
        public List<TrackType> TrackTypes
        {
            get
            {
                return this.trackTypes;
            }
            set
            {
                this.trackTypes = value;
                NotifyPropertyChanged(nameof(TrackTypes));
            }
        }

        private TrackType selectedTrackType;
        public TrackType SelectedTrackType
        {
            get
            {
                return this.selectedTrackType;
            }
            set
            {
                this.selectedTrackType = value;
                NotifyPropertyChanged(nameof(SelectedTrackType));
                FillTracks();
            }
        }

        private TrackFilterType trackFilterType = TrackFilterType.Single;
        public TrackFilterType TrackFilterType
        {
            get
            {
                return this.trackFilterType;
            }
            set
            {
                this.trackFilterType = value;
                NotifyPropertyChanged(nameof(TrackFilterType));
                FillTrackTypes();
            }
        }

        private void FillTracks()
        {
            this.Tracks = this.TrackFilterType switch
            {
                TrackFilterType.Single => this.selectedTrackType?.Tracks,
                TrackFilterType.Group => this.selectedTrackType?.Groups.Cast<TrackBase>().ToList(),
                TrackFilterType.Customer => new List<TrackBase>(),
                _ => null
            };
            this.SelectedTrack = this.Tracks?.FirstOrDefault();
        }

        public List<TrackBase> tracks;
        public List<TrackBase> Tracks
        {
            get
            {
                return this.tracks;
            }
            set
            {
                this.tracks = value;
                NotifyPropertyChanged(nameof(Tracks));
            }
        }

        private TrackBase selectedTrack;
        public TrackBase SelectedTrack
        {
            get
            {
                return this.selectedTrack;
            }
            set
            {
                this.selectedTrack = value;
                NotifyPropertyChanged(nameof(SelectedTrack));
                //Invalidate();
            }
        }

    }
}
