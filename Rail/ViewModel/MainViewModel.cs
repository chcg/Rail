using Rail.Controls;
using Rail.Model;
using Rail.Mvvm;
using Rail.Plugin;
using Rail.Properties;
using Rail.Tracks;
using Rail.View;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Rail.ViewModel
{
    public partial class MainViewModel : FileViewModel, IRailControl, IRailPlan
    {
        
        //private RailPlan railPlan;

        public DelegateCommand RailPlanCommand { get; private set; }
        public DelegateCommand PrintCommand { get; }
        public DelegateCommand PrintPreviewCommand { get; }

        private double zoomFactor = 1.0;

        private readonly double copyPositionDrift = 50;

        private RailPlan railPlan;
        public event EventHandler RailChanged;

        private readonly Pen plateFramePen = new Pen(TrackBrushes.PlateFrame, 1);

        public DelegateCommand CreateGroupCommand { get; }
        public DelegateCommand ResolveGroupCommand { get; }
        public DelegateCommand SaveAsGroupCommand { get; }

        public DelegateCommand CreateRampCommand { get; }
        public DelegateCommand DeleteRampCommand { get; }
        public DelegateCommand EditRampCommand { get; }

        public DelegateCommand CreateHelixCommand { get; }
        public DelegateCommand DeleteHelixCommand { get; }
        public DelegateCommand EditHelixCommand { get; }

        //public DelegateCommand UndoCommand { get; }
        //public DelegateCommand RedoCommand { get; }
        public DelegateCommand CopyCommand { get; }
        public DelegateCommand CutCommand { get; }
        public DelegateCommand PasteCommand { get; }
        public DelegateCommand DeleteCommand { get; }
        public DelegateCommand DuplicateCommand { get; }
        public DelegateCommand SelectAllCommand { get; }

        public DelegateCommand AnchorCommand { get; }
        public DelegateCommand UnanchorCommand { get; }

        public MainViewModel()
        {
            this.CreateGroupCommand = new DelegateCommand(OnCreateGroup, OnCanCreateGroup);
            this.ResolveGroupCommand = new DelegateCommand(OnResolveGroup, OnCanResolveGroup);
            this.SaveAsGroupCommand = new DelegateCommand(OnSaveAsGroup, OnCanSaveAsGroup);

            this.CreateRampCommand = new DelegateCommand(OnCreateRamp, OnCanCreateRamp);
            this.DeleteRampCommand = new DelegateCommand(OnDeleteRamp, OnCanDeleteRamp);
            this.EditRampCommand = new DelegateCommand(OnEditRamp, OnCanEditRamp);

            this.CreateHelixCommand = new DelegateCommand(OnCreateHelix, OnCanCreateHelix);
            this.DeleteHelixCommand = new DelegateCommand(OnDeleteHelix, OnCanDeleteHelix);
            this.EditHelixCommand = new DelegateCommand(OnEditHelix, OnCanEditHelix);

            //this.UndoCommand = new DelegateCommand(OnUndo, OnCanUndo);
            //this.RedoCommand = new DelegateCommand(OnRedo, OnCanRedo);
            this.CopyCommand = new DelegateCommand(OnCopy, OnCanCopy);
            this.CutCommand = new DelegateCommand(OnCut, OnCanCut);
            this.PasteCommand = new DelegateCommand(OnPaste, OnCanPaste);
            this.DeleteCommand = new DelegateCommand(OnDelete, OnCanDelete);
            this.DuplicateCommand = new DelegateCommand(OnDuplicate, OnCanDuplicate);
            this.SelectAllCommand = new DelegateCommand(OnSelectAll, OnCanSelectAll);

            this.AnchorCommand = new DelegateCommand(OnAnchor, OnCanAnchor);
            this.UnanchorCommand = new DelegateCommand(OnUnanchor, OnCanUnanchor);

            this.DefaultFileExt = "*.rail";
            this.FileFilter = "Rail Project|*.rail|All Files|*.*";

            this.RailPlanCommand = new DelegateCommand(OnRailPlan);
            this.PrintCommand = new DelegateCommand(OnPrint);
            this.PrintPreviewCommand = new DelegateCommand(OnPrintPreview);

            //this.Gradients = (new double[] { 0.0, 0.5, 1.0, 1.5, 2.0, 2.5, 3.0, 3.5, 4.0, 4.5, 5.0, 5.5, 6.0, 6.5, 7.0, 7.5, 8.0 }).Select(d => d.ToString("F2")).ToList(); 

            // load track list
            LoadTrackList();

            OnCreate();

            //Update3D();
            //this.SelectedSelectionIndex = 0;

        }

        public override void OnStartup()
        {
           base.OnStartup();

           // this.RailPlan = RailPlan.Create();

           // Update3D();
        }

        #region properties






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


        // TODO delete
        public RailPlan RailPlan
        {
            get
            {
                return this.railPlan;
            }
            set
            {
                this.railPlan = value;
                NotifyPropertyChanged(nameof(RailPlan));
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

            this.MaterialList = this.RailPlan.Rails.SelectMany(r => r.Materials).GroupBy(m => m.Id).Select(g => { var m = g.First(); m.Number = g.Select(i => i.Number).Sum(); return m; }).ToList();
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
                    //Update3D();
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
        public double[] GridLinesDistances { get { return new double[] { 0, 10, 25, 50, 100, 250, 500 }; } }

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
        
        public List<string> Gradients { get; private set; }


        public double? selectedRailsX;
        public double? SelectedRailsX
        {
            get
            {
                return selectedRailsX;
            }
            set
            {
                selectedRailsX = value;
                NotifyPropertyChanged(nameof(SelectedRailsX));
            }
        }

        private double? selectedRailsY;
        public double? SelectedRailsY
        {
            get
            {
                return selectedRailsY;
            }
            set
            {
                selectedRailsY = value;
                NotifyPropertyChanged(nameof(SelectedRailsY));
            }
        }

        private double? selectedRailsAngle;
        public double? SelectedRailsAngle
        {
            get
            {
                return selectedRailsAngle;
            }
            set
            {
                selectedRailsAngle = value;
                NotifyPropertyChanged(nameof(SelectedRailsAngle));
            }
        }

        private Guid selectedRailsLayer;
        public Guid SelectedRailsLayer
        {
            get
            {
                return selectedRailsLayer;
            }
            set
            {
                selectedRailsLayer = value;
                NotifyPropertyChanged(nameof(SelectedRailsLayer));
            }
        }

        #endregion

        #region methods

        public override void OnCreate()
        {
            this.RailPlan = RailPlan.Create();
            this.FileChanged = true;
            this.FilePath = null;

            this.InsertLayer = this.RailPlan.Layers.FirstOrDefault();
        }

        public override void OnLoad(string path)
        {
            this.RailPlan = RailPlan.Load(path, this.trackDict);
            this.FileChanged = true;

            this.InsertLayer = this.RailPlan.Layers.FirstOrDefault();
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
                    //Rail = this.RailPlan
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
            //Update3D();
            base.OnRefresh();
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

        #endregion
    }
}
