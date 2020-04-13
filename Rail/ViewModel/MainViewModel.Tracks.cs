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

                this.TrackTypeFilters = this.trackTypeFilterType switch
                {
                    TrackTypeFilterType.All => null,
                    TrackTypeFilterType.Gauge => this.trackList.TrackTypes.Select(t => t.Parameter.Gauge.ToString()).Distinct().OrderBy(t => t).ToList(),
                    TrackTypeFilterType.Manufacturer => this.trackList.TrackTypes.Select(t => t.Parameter.Manufacturer).Distinct().OrderBy(t => t).ToList(),
                    _ => null
                };
                NotifyPropertyChanged(nameof(TrackTypeFilters));
                this.SelectedTrackTypeFilter = this.TrackTypeFilters?.FirstOrDefault();
            }
        }

        public List<string> TrackTypeFilters { get; private set; }

        public string selectedTrackTypeFilter;
        public string SelectedTrackTypeFilter
        {
            get
            {
                return this.selectedTrackTypeFilter;
            }
            set
            {
                this.selectedTrackTypeFilter = value;
                NotifyPropertyChanged(nameof(SelectedTrackTypeFilter));

                this.TrackTypes = this.trackTypeFilterType switch
                {
                    TrackTypeFilterType.All => this.trackList.TrackTypes,
                    TrackTypeFilterType.Gauge => this.trackList.TrackTypes.Where(t => t.Parameter.Gauge.ToString() == selectedTrackTypeFilter).ToList(),
                    TrackTypeFilterType.Manufacturer => this.trackList.TrackTypes.Where(t => t.Parameter.Manufacturer == selectedTrackTypeFilter).ToList(),
                    _ => null
                };
                NotifyPropertyChanged(nameof(TrackTypes));
                this.SelectedTrackType = this.TrackTypes.FirstOrDefault();
            }
        }

        public List<TrackType> TrackTypes { get; private set; }

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

        private void FillTracks()
        {
            switch (this.SelectedGroupIndex)
            {
            case 0:
                this.Tracks = this.selectedTrackType?.Tracks;
                break;
            case 1:
                this.Tracks = this.selectedTrackType?.Groups.Cast<TrackBase>().ToList();
                break;
            case 2:
                this.Tracks = new List<TrackBase>();
                break;
            }

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
