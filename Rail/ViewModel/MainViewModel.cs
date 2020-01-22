using Rail.Controls;
using Rail.Misc;
using Rail.Model;
using Rail.Mvvm;
using Rail.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Rail.ViewModel
{
    public partial class MainViewModel : FileViewModel
    {
        private readonly TrackList trackList;
        private readonly Dictionary<string, TrackBase> trackDict;
        private RailPlan railPlan;

        public DelegateCommand RailPlanCommand { get; private set; }
        public DelegateCommand PrintCommand { get; private set; }
        public DelegateCommand PrintPreviewCommand { get; private set; }

        public DelegateCommand CreateGroupCommand { get; private set; }
        public DelegateCommand ResolveGroupCommand { get; private set; }
        public DelegateCommand SaveAsGroupCommand { get; private set; }
        
        public DelegateCommand CreateRampCommand { get; private set; }
        public DelegateCommand DeleteRampCommand { get; private set; }

        private double zoomFactor = 1.0;
       
        public MainViewModel()
        {
            this.DefaultFileExt = "*.rail";
            this.FileFilter = "Rail Project|*.rail|All Files|*.*";

            this.RailPlanCommand = new DelegateCommand(OnRailPlan);
            this.PrintCommand = new DelegateCommand(OnPrint);
            this.PrintPreviewCommand = new DelegateCommand(OnPrintPreview);

            this.CreateGroupCommand = new DelegateCommand(OnCreateGroup, OnCanCreateGroup);
            this.ResolveGroupCommand = new DelegateCommand(OnResolveGroup, OnCanResolveGroup);
            this.SaveAsGroupCommand = new DelegateCommand(OnSaveAsGroup, OnCanSaveAsGroup);

            this.CreateRampCommand = new DelegateCommand(OnCreateRamp, OnCanCreateRamp);
            this.DeleteRampCommand = new DelegateCommand(OnDeleteRamp, OnCanDeleteRamp);

            this.Gradients = (new double[] { 0.0, 0.5, 1.0, 1.5, 2.0, 2.5, 3.0, 3.5, 4.0, 4.5, 5.0, 5.5, 6.0, 6.5, 7.0, 7.5, 8.0 }).Select(d => d.ToString("F2")).ToList(); 
            
            // load track list
            DependencyObject dep = new DependencyObject();
            if (!DesignerProperties.GetIsInDesignMode(dep))
            {

                try
                {
                    string path = System.AppDomain.CurrentDomain.BaseDirectory;
                    this.trackList = TrackList.Load(Path.Combine(path, "Tracks.xml"));
                    this.trackDict = trackList.TrackTypes.SelectMany(t => t.Tracks).ToDictionary(t => t.Id, t => t);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error in File Tracks.xml\r\n" + ex.Message);
                    throw ex;
                }
            }

            this.RailPlan = RailPlan.Create();

            Update3D();
            this.SelectedSelectionIndex = 0;
        }

        #region properties

        private int selectedSelectionIndex = 0;

        public int SelectedSelectionIndex
        {
            get
            {
                return this.selectedSelectionIndex;
            }
            set
            {
                this.selectedSelectionIndex = value;
                NotifyPropertyChanged(nameof(SelectedSelectionIndex));
                
                this.TrackSelects = this.selectedSelectionIndex switch
                {
                    0 => null,
                    1 => this.trackList.TrackTypes.Select(t => t.Gauge).Distinct().OrderBy(t => t).ToList(),
                    2 => this.trackList.TrackTypes.Select(t => t.Manufacturer).Distinct().OrderBy(t => t).ToList(),
                    _ => null
                };
                NotifyPropertyChanged(nameof(TrackSelects));
                this.SelectedTrackSelect = this.TrackSelects?.FirstOrDefault();
            }
        }

        public List<string> TrackSelects { get; private set; }

        public string selectedTrackSelect;
        public string SelectedTrackSelect
        {
            get
            {
                return this.selectedTrackSelect;
            }
            set
            {
                this.selectedTrackSelect = value;
                NotifyPropertyChanged(nameof(SelectedTrackSelect));

                this.TrackTypes = this.selectedSelectionIndex switch
                {
                    0 => this.trackList.TrackTypes,
                    1 => this.trackList.TrackTypes.Where(t => t.Gauge == SelectedTrackSelect).ToList(),
                    2 => this.trackList.TrackTypes.Where(t => t.Manufacturer == SelectedTrackSelect).ToList(),
                    _ => null
                };
                NotifyPropertyChanged(nameof(TrackTypes));
                this.SelectedTrackType = this.TrackTypes.FirstOrDefault();
            }
        }

        public List<TrackType> TrackTypes { get; private set; } // { return this.trackList.TrackTypes; } }

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

        private int selectedGroupIndex = 0;
        public int SelectedGroupIndex
        {
            get
            {
                return this.selectedGroupIndex;
            }
            set
            {
                this.selectedGroupIndex = value;
                NotifyPropertyChanged(nameof(SelectedGroupIndex));
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

        private TrackBase selectedTracke;
        public TrackBase SelectedTrack
        {
            get
            {
                return this.selectedTracke;
            }
            set
            {
                this.selectedTracke = value;
                NotifyPropertyChanged(nameof(SelectedTrack));
            }
        }

        public RailPlan RailPlan
        {
            get
            {
                return this.railPlan;
            }
            set
            {
                if (this.railPlan != null)
                {
                    this.railPlan.Rails.CollectionChanged -= OnRailsChanged;
                }
                this.railPlan = value;
                NotifyPropertyChanged(nameof(RailPlan));
                if (this.railPlan != null)
                {
                    this.railPlan.Rails.CollectionChanged += OnRailsChanged;
                }
            }
        }

        private void OnRailsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            CreateMaterialList();
        }

        private void CreateMaterialList()
        {
            //var l1 = this.RailPlan.Rails.SelectMany(r => r.Track.Materials).GroupBy(m => m.Id).ToList();
            //var l3 = l1.Select(g => new { num = g.Select(m => m.Number).Sum(), material = g.First() }).ToList();
            //this.MaterialList = l3.Select(i => { i.material.Number = i.num; return i.material; }).ToList();

            this.MaterialList = this.RailPlan.Rails.SelectMany(r => r.Track.Materials).GroupBy(m => m.Id).Select(g => { var m = g.First(); m.Number = g.Select(i => i.Number).Sum(); return m; }).ToList();
        }

        private List<TrackMaterial> materialList;
        public List<TrackMaterial> MaterialList
        {
            get
            {
                return this.materialList;
            }
            set
            {
                this.materialList = value;
                NotifyPropertyChanged(nameof(MaterialList));
            }
        }

        private bool showMaterialList = true;
        public bool ShowMaterialList
        {
            get
            {
                return this.showMaterialList;
            }
            set
            {
                this.showMaterialList = value;
                NotifyPropertyChanged(nameof(ShowMaterialList));
            }
        }

        private RailViewMode viewMode;
        public RailViewMode ViewMode
        {
            get
            {
                return this.viewMode;
            }
            set
            {
                this.viewMode = value;
                if (this.viewMode == RailViewMode.Terrain)
                {
                    Update3D();
                }
                NotifyPropertyChanged(nameof(ViewMode));
            }
        }

        private bool showDockingPoints = true;
        public bool ShowDockingPoints
        {
            get
            {
                return this.showDockingPoints;
            }
            set
            {
                this.showDockingPoints = value;
                NotifyPropertyChanged(nameof(ShowDockingPoints));
            }
        }

        public double ZoomFactor
        {
            get
            {
                return this.zoomFactor;
            }
            set
            {
                this.zoomFactor = value;
                NotifyPropertyChanged(nameof(ZoomFactor));
                this.ZoomFactor3D = value * 0.005;
                NotifyPropertyChanged(nameof(ZoomFactor3D));
            }
        }

        public double ZoomFactor3D { get; private set; }

        public double[] SnapInDistances { get { return new double[] { 0, 100, 1000, 10000, 100000 }; } }
        public double[] SnapInAngels { get { return new double[] { 0, 30, 15, 7.5 }; } }
        public double[] GridLinesDistances { get { return new double[] { 0, 100, 1000, 10000, 100000 }; } }

        private double snapInDistance = 0;
        public double SnapInDistance
        {
            get
            {
                return this.snapInDistance;
            }
            set
            {
                this.snapInDistance = value;
                NotifyPropertyChanged(nameof(SnapInDistance));
            }
        }

        private double snapInAngel = 0;
        public double SnapInAngel
        {
            get
            {
                return this.snapInAngel;
            }
            set
            {
                this.snapInAngel = value;
                NotifyPropertyChanged(nameof(SnapInAngel));
            }
        }

        private double gridLinesDistance = 0;
        public double GridLinesDistance
        {
            get
            {
                return this.gridLinesDistance;
            }
            set
            {
                this.gridLinesDistance = value;
                NotifyPropertyChanged(nameof(GridLinesDistance));
            }
        }

        private Point mousePosition = new Point(0, 0);
        public Point MousePosition
        {
            get
            {
                return this.mousePosition;
            }
            set
            {
                this.mousePosition = value;
                NotifyPropertyChanged(nameof(MousePosition));
            }
        }

        public IEnumerable<RailLayer> Layers
        {
            get { return this.RailPlan.Layers.Reverse(); }
        }

       

        private RailLayer selectedInsertLayer = null;
        public RailLayer SelectedInsertLayer
        {
            get
            {
                return this.selectedInsertLayer;
            }
            set
            {
                this.selectedInsertLayer = value;
                NotifyPropertyChanged(nameof(SelectedInsertLayer));
            }
        }

        public List<string> Gradients { get; private set; }  

        #endregion

        #region methods

        public override void OnCreate()
        {
            this.RailPlan = RailPlan.Create();
            this.FileChanged = true;
            this.FilePath = null;
        }

        public override void OnLoad(string path)
        {
            this.RailPlan = RailPlan.Load(path);
            foreach (RailItem railItem in this.RailPlan.Rails)
            {
                // set track
                railItem.Track = this.trackDict[railItem.TrackId];
                // set dock points
                railItem.DockPoints.ForEach((railDockPoint, index) =>
                {
                    railDockPoint.Update(railItem, railItem.Track.DockPoints[index]);
                });
            }
            // link dock points
            var list = this.RailPlan.Rails.SelectMany(r => r.DockPoints).Where(dp => dp.DockedWithId != Guid.Empty).ToList();
            foreach (RailDockPoint dp in list)
            {
                RailDockPoint x = list.Single(i => i.Id == dp.DockedWithId);
                x.Dock(dp);

            }
            this.FileChanged = true;
        }

        public override void OnStore(string path)
        {
            this.RailPlan.Save(path);
            this.FileChanged = true;
        }

        private void OnRailPlan()
        {
            RailPlanView view = new RailPlanView()
            {
                DataContext = new RailPlanViewModel(this.railPlan)
            };
            if (view.ShowDialog().GetValueOrDefault() == true)
            {
                this.FileChanged = true;
            }
        }

        private void OnPrint()
        {
            double pageMargin = 80;

            PrintDialog printDialog = new PrintDialog();

            if (printDialog.ShowDialog() == true)
            {
                PrintCapabilities capabilities = printDialog.PrintQueue.GetPrintCapabilities(printDialog.PrintTicket);

                RailPlanControl ctrl = new RailPlanControl
                {
                    Background = new SolidColorBrush(Colors.White),
                    RailPlan = this.RailPlan
                };
                ctrl.ZoomFactor = Math.Min(capabilities.PageImageableArea.ExtentWidth / (ctrl.Width + pageMargin * 2), capabilities.PageImageableArea.ExtentHeight / (ctrl.Height + pageMargin * 2));
                
                printDialog.PrintVisual(ctrl, "Rail Plan");
            }
        }

        private void OnPrintPreview()
        {
            //ObservableCollection
            
        }

        protected override void OnRefresh()
        {
            Update3D();
            base.OnRefresh();
        }

        protected override void OnOptions()
        {
            (new OptionsView { DataContext = new OptionsViewModel() }).ShowDialog();
        }

        #endregion

        #region Group

        private void OnCreateGroup()
        { }

        private bool OnCanCreateGroup()
        {
            return true;
        }

        private void OnResolveGroup()
        { }

        private bool OnCanResolveGroup()
        {
            return true;
        }

        private void OnSaveAsGroup()
        { }

        private bool OnCanSaveAsGroup()
        {
            return true;
        }
        
        #endregion

        #region Ramp

        private void OnCreateRamp()
        { }

        private bool OnCanCreateRamp()
        {
            return true;
        }

        private void OnDeleteRamp()
        { }

        private bool OnCanDeleteRamp()
        {
            return true;
        }

        #endregion
    }
}
