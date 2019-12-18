using Rail.Controls;
using Rail.Model;
using Rail.Mvvm;
using Rail.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Xps;
using Rail.Misc;
using System.Collections.Specialized;

namespace Rail.ViewModel
{
    public partial class MainViewModel : FileViewModel
    {
        private TrackList trackList;
        private RailPlan railPlan;
        private Dictionary<string, TrackBase> trackDict;

        public DelegateCommand RailPlanCommand { get; private set; }
        public DelegateCommand PrintCommand { get; private set; }
        public DelegateCommand PrintPreviewCommand { get; private set; }

        private double zoomFactor = 1.0;
        //private double groundWidth = 2000.0;
        //private double groundHeight = 1000.0;

        public MainViewModel()
        {
            this.DefaultFileExt = "*.rail";
            this.FileFilter = "Rail Project|*.rail|All Files|*.*";

            this.RailPlanCommand = new DelegateCommand(OnRailPlan);
            this.PrintCommand = new DelegateCommand(OnPrint);
            this.PrintPreviewCommand = new DelegateCommand(OnPrintPreview);

            this.ShowLayers = new List<ushort> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            this.InsertLayers = new List<ushort> { 1, 2, 3, 4, 5, 6, 7, 8 };

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
        }

        #region properties

        public List<TrackType> TrackTypes { get { return this.trackList.TrackTypes; } }

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
                NotifyPropertyChanged("SelectedTrackType");

                this.Tracks = this.selectedTrackType?.Tracks;
                this.SelectedTrack = this.Tracks.FirstOrDefault();
            }
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
                NotifyPropertyChanged("Tracks");
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
                NotifyPropertyChanged("SelectedTrack");
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
                NotifyPropertyChanged("RailPlan");
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
            List<MaterialViewModel> list = new List<MaterialViewModel>();
            if (this.railPlan != null)
            {
                this.RailPlan.Rails.ForEach(r =>
                    {
                        var item = list.FirstOrDefault(i => i.Article == r.Id);
                        if (item != null)
                        {
                            item.Number += 1;
                        }
                        else
                        {
                            list.Add(new MaterialViewModel { Number = 1, Article = r.Id, Name = r.Track.Name });
                        }
                    }
                );
            }
            this.MaterialList = list;
        }

        private List<MaterialViewModel> materialList;
        public List<MaterialViewModel> MaterialList
        {
            get
            {
                return this.materialList;
            }
            set
            {
                this.materialList = value;
                NotifyPropertyChanged("MaterialList");
            }
        }

        private bool showMaterialList;
        public bool ShowMaterialList
        {
            get
            {
                return this.showMaterialList;
            }
            set
            {
                this.showMaterialList = value;
                NotifyPropertyChanged("ShowMaterialList");
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
                NotifyPropertyChanged("ViewMode");
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
                NotifyPropertyChanged("ShowDockingPoints");
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
                NotifyPropertyChanged("ZoomFactor");
            }
        }

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
                NotifyPropertyChanged("SnapInDistance");
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
                NotifyPropertyChanged("SnapInAngel");
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
                NotifyPropertyChanged("GridLinesDistance");
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
                NotifyPropertyChanged("MousePosition");
            }
        }

        public List<ushort> ShowLayers { get; private set; }
        public List<ushort> InsertLayers { get; private set; }

        private ushort selectedShowLayer = 0;
        public ushort SelectedShowLayer
        {
            get
            {
                return this.selectedShowLayer;
            }
            set
            {
                this.selectedShowLayer = value;
                NotifyPropertyChanged(nameof(SelectedShowLayer));
            }
        }

        private ushort selectedInsertLayer = 1;
        public ushort SelectedInsertLayer
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
            this.RailPlan.Rails.ForEach(r => r.Track = this.trackDict[r.Id]);
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

                RailPlanControl ctrl = new RailPlanControl();
                ctrl.Background = new SolidColorBrush(Colors.White);
                ctrl.RailPlan = this.RailPlan;
                ctrl.ZoomFactor = Math.Min(capabilities.PageImageableArea.ExtentWidth / (ctrl.Width + pageMargin * 2), capabilities.PageImageableArea.ExtentHeight / (ctrl.Height + pageMargin * 2));
                
                printDialog.PrintVisual(ctrl, "Rail Plan");
            }
        }

        private void OnPrintPreview()
        {
            
            
        }
        #endregion
    }
}
