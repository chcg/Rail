using Rail.Misc;
using Rail.Model;
using Rail.Mvvm;
using Rail.Trigonometry;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Rail.Properties;
using System.Collections;
using System.ComponentModel;
using Rail.View;
using Rail.ViewModel;

namespace Rail.Controls
{
    public class RailPlanControl : Control
    {
        private readonly double copyPositionDrift = 50;
        private double margin = 0;
        private readonly Pen plateFramePen = new Pen(TrackBrushes.PlateFrame, 1);
        private readonly Pen selectFramePen1 = new Pen(Brushes.Black, 2);
        private readonly Pen selectFramePen2 = new Pen(Brushes.White, 2) { DashStyle = DashStyles.Dot };

        // mouse operation variables
        private Point lastMousePosition; 
        private RailAction actionType;
        private RailBase actionRailItem;
        private bool hasMoved;
        private List<RailBase> actionSubgraph;
        //private Angle debugRotationAngle;
        private Point selectRecStart;

        private bool selectedChangeIntern = false;
        private RailBase selectedRail;
        private List<RailBase> selectedRails;

        //public DelegateCommand<RailBase> DeleteRailItemCommand { get; private set; }
        //public DelegateCommand<RailBase> RotateRailItemCommand { get; private set; }
        //public DelegateCommand<RailBase> PropertiesRailItemCommand { get; private set; }

        public DelegateCommand CreateGroupCommand { get; private set; }
        public DelegateCommand ResolveGroupCommand { get; private set; }
        public DelegateCommand SaveAsGroupCommand { get; private set; }

        public DelegateCommand CreateRampCommand { get; private set; }
        public DelegateCommand DeleteRampCommand { get; private set; }
        public DelegateCommand EditRampCommand { get; private set; }

        public DelegateCommand CreateHelixCommand { get; private set; }
        public DelegateCommand DeleteHelixCommand { get; private set; }
        public DelegateCommand EditHelixCommand { get; private set; }

        public DelegateCommand UndoCommand { get; private set; }
        public DelegateCommand RedoCommand { get; private set; }
        public DelegateCommand CopyCommand { get; private set; }
        public DelegateCommand CutCommand { get; private set; }
        public DelegateCommand PasteCommand { get; private set; }
        public DelegateCommand DeleteCommand { get; private set; }
        public DelegateCommand DuplicateCommand { get; private set; }
        public DelegateCommand SelectAllCommand { get; private set; }




        //public static readonly RoutedCommand RefreshCommand = new RoutedCommand("Refresh", typeof(RailPlanControl));

        protected enum RailAction
        {
            None,
            /// <summary>
            /// Move only one rail and undock if it is docked
            /// </summary>
            MoveSingle,
            /// <summary>
            /// Move graph without finding new docks
            /// </summary>
            MoveSimple,
            /// <summary>
            /// Move graph with finding new docks
            /// </summary>
            MoveGraph,
            /// <summary>
            /// Rotage graph
            /// </summary>
            Rotate,
            /// <summary>
            /// show a select rectangle
            /// </summary>
            SelectRect
        }

        static RailPlanControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RailPlanControl), new FrameworkPropertyMetadata(typeof(RailPlanControl)));
        }

        public RailPlanControl()
        {
            //this.DeleteRailItemCommand = new DelegateCommand<RailBase>(OnDeleteRailItem);
            //this.RotateRailItemCommand = new DelegateCommand<RailBase>(OnRotateRailItem);
            //this.PropertiesRailItemCommand = new DelegateCommand<RailBase>(OnPropertiesRailItem);

            this.CreateGroupCommand = new DelegateCommand(OnCreateGroup, OnCanCreateGroup);
            this.ResolveGroupCommand = new DelegateCommand(OnResolveGroup, OnCanResolveGroup);
            this.SaveAsGroupCommand = new DelegateCommand(OnSaveAsGroup, OnCanSaveAsGroup);

            this.CreateRampCommand = new DelegateCommand(OnCreateRamp, OnCanCreateRamp);
            this.DeleteRampCommand = new DelegateCommand(OnDeleteRamp, OnCanDeleteRamp);
            this.EditRampCommand = new DelegateCommand(OnEditRamp, OnCanEditRamp);

            this.CreateHelixCommand = new DelegateCommand(OnCreateHelix, OnCanCreateHelix);
            this.DeleteHelixCommand = new DelegateCommand(OnDeleteHelix, OnCanDeleteHelix);
            this.EditHelixCommand = new DelegateCommand(OnEditHelix, OnCanEditHelix);

                this.UndoCommand = new DelegateCommand(OnUndo, OnCanUndo);
            this.RedoCommand = new DelegateCommand(OnRedo, OnCanRedo);
            this.CopyCommand = new DelegateCommand(OnCopy, OnCanCopy);
            this.CutCommand = new DelegateCommand(OnCut, OnCanCut);
            this.PasteCommand = new DelegateCommand(OnPaste, OnCanPaste);
            this.DeleteCommand = new DelegateCommand(OnDelete, OnCanDelete);
            this.DuplicateCommand = new DelegateCommand(OnDuplicate, OnCanDuplicate);
            this.SelectAllCommand =  new DelegateCommand(OnSelectAll, OnCanSelectAll);

            //CommandBinding commandBinding = new CommandBinding(RefreshCommand);
            //commandBinding.Executed += OnRefresh;
            //commandBinding.CanExecute += OnCanRefresh;
            //CommandManager.RegisterClassCommandBinding(typeof(RailPlanControl), commandBinding);

            this.Loaded += OnLoaded;
        }
        

        //private void OnCanRefresh(object sender, CanExecuteRoutedEventArgs e)
        //{
            
        //}

        //private void OnRefresh(object target, ExecutedRoutedEventArgs e)
        //{
            
        //}

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            DependencyObject dep = new DependencyObject();
            if (!DesignerProperties.GetIsInDesignMode(dep))
            {
                var window = Window.GetWindow(this);
                window.KeyDown += OnKeyPress;
            }
        }
        
        #region ZoomFactor

        public static readonly DependencyProperty ZoomFactorProperty =
            DependencyProperty.Register("ZoomFactor", typeof(double), typeof(RailPlanControl),
                new FrameworkPropertyMetadata(1.0, new PropertyChangedCallback(OnZoomFactorChanged)));

        public double ZoomFactor
        {
            get
            {
                return (double)GetValue(ZoomFactorProperty);
            }
            set
            {
                SetValue(ZoomFactorProperty, value);
            }
        }

        private static void OnZoomFactorChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RailPlanControl railPlan = (RailPlanControl)o;
            railPlan.CalcGroundSize();
        }

        #endregion

        #region RailMargin

        public double RailMargin
        {
            get
            {
                return this.margin;
            }
            set
            {
                this.margin = value;
            }
        }

        #endregion

        #region SelectedTrack

        public static readonly DependencyProperty SelectedTrackProperty =
            DependencyProperty.Register("SelectedTrack", typeof(TrackBase), typeof(RailPlanControl));

        public TrackBase SelectedTrack
        {
            get
            {
                return (TrackBase)GetValue(SelectedTrackProperty);
            }
            set
            {
                SetValue(SelectedTrackProperty, value);
            }
        }

        #endregion

        #region RailPlan

        public static readonly DependencyProperty RailPlanProperty =
            DependencyProperty.Register("RailPlan", typeof(RailPlan), typeof(RailPlanControl),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnRailPlanPropertyChanged)));

        public RailPlan RailPlan
        {
            get
            {
                return (RailPlan)GetValue(RailPlanProperty);
            }
            set
            {
                SetValue(RailPlanProperty, value);
            }
        }

        private static void OnRailPlanPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RailPlanControl railPlan = (RailPlanControl)o;
            railPlan.CalcGroundSize();
        }

        #endregion

        #region MousePosition

        public static readonly DependencyProperty MousePositionProperty =
            DependencyProperty.Register("MousePosition", typeof(Point), typeof(RailPlanControl),
                new FrameworkPropertyMetadata(new Point(0, 0), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public Point MousePosition
        {
            get
            {
                return (Point)GetValue(MousePositionProperty);
            }
            set
            {
                SetValue(MousePositionProperty, value);
            }
        }

        #endregion

        #region ViewMode

        public static readonly DependencyProperty ViewModeProperty =
            DependencyProperty.Register("ViewMode", typeof(RailViewMode), typeof(RailPlanControl),
                new FrameworkPropertyMetadata(RailViewMode.Tracks, new PropertyChangedCallback(OnViewModeChanged)));

        public RailViewMode ViewMode
        {
            get
            {
                return (RailViewMode)GetValue(ViewModeProperty);
            }
            set
            {
                SetValue(ViewModeProperty, value);
            }
        }

        private static void OnViewModeChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RailPlanControl railPlan = (RailPlanControl)o;
            railPlan.InvalidateVisual();
        }

        #endregion

        #region ShowDockingPoints

        public static readonly DependencyProperty ShowDockingPointsProperty =
            DependencyProperty.Register("ShowDockingPoints", typeof(bool), typeof(RailPlanControl),
                new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnShowDockingPointsChanged)));

        public bool ShowDockingPoints
        {
            get
            {
                return (bool)GetValue(ShowDockingPointsProperty);
            }
            set
            {
                SetValue(ShowDockingPointsProperty, value);
            }
        }

        private static void OnShowDockingPointsChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RailPlanControl railPlan = (RailPlanControl)o;
            railPlan.InvalidateVisual();
        }

        #endregion

        #region InsertLayer

        public static readonly DependencyProperty InsertLayerProperty =
            DependencyProperty.Register("InsertLayer", typeof(RailLayer), typeof(RailPlanControl),
                new FrameworkPropertyMetadata((RailLayer)null));

        public RailLayer InsertLayer
        {
            get
            {
                return (RailLayer)GetValue(InsertLayerProperty);
            }
            set
            {
                SetValue(InsertLayerProperty, value);
            }
        }

        #endregion

        #region SelectedRails

        public static readonly DependencyProperty SelectedRailsProperty =
            DependencyProperty.Register("SelectedRails", typeof(IList), typeof(RailPlanControl), new FrameworkPropertyMetadata(null));

        public IList SelectedRails
        {
            get
            {
                return (IList)GetValue(SelectedRailsProperty);
            }
            set
            {
                SetValue(SelectedRailsProperty, value);
            }
        }

        private void UpdateSelectedRails()
        {
            if (this.SelectedRails != null)
            {
                this.SelectedRails.Clear();
                this.RailPlan.SelectedRails.ForEach(r => this.SelectedRails.Add(r));
            }
            
            var selectedRails = this.RailPlan.SelectedRails.ToList();
            switch (selectedRails.Count())
            {
            case 0:
                this.SelectedMode = RailSelectedMode.None;
                this.SelectedRailsX = null;
                this.SelectedRailsY = null;
                this.SelectedRailsAngle = null;
                this.SelectedRailsLayer = Guid.Empty;
                //this.SelectedRailsGradient = null;
                //this.SelectedRailsHeight = null;
                this.SelectedRamp = null;
                break;
            case 1:
                this.SelectedMode = RailSelectedMode.Single;
                this.selectedRail = selectedRails.Single();

                this.SelectedRailsX = this.selectedRail.Position.X;
                this.SelectedRailsY = this.selectedRail.Position.Y;
                this.SelectedRailsAngle = this.selectedRail.Angle;
                this.SelectedRailsLayer = this.selectedRail.Layer;
                //this.SelectedRailsGradient = this.selectedRail.Gradient;
                //this.SelectedRailsHeight = this.selectedRail.Height;
                this.SelectedRamp = this.selectedRail as RailRamp;
                break;
            default:
                this.SelectedMode = RailSelectedMode.Multi;
                this.selectedRails = selectedRails;

                this.SelectedRailsX = null;
                this.SelectedRailsY = null;
                this.SelectedRailsAngle = null;
                this.SelectedRailsLayer = selectedRails.Select(r => r.Layer).IdenticalOrDefault();
                //this.SelectedRailsGradient = selectedRails.Select(r => r.Gradient).IdenticalOrDefault();
                //this.SelectedRailsHeight = selectedRails.Select(r => r.Height).IdenticalOrDefault();
                this.SelectedRamp = null;
                break;
            }
           
        }

        #endregion

        #region SelectedMode

        public static readonly DependencyProperty SelectedModeProperty =
            DependencyProperty.Register("SelectedMode", typeof(RailSelectedMode), typeof(RailPlanControl), new FrameworkPropertyMetadata(RailSelectedMode.None, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public RailSelectedMode SelectedMode
        {
            get
            {
                return (RailSelectedMode)GetValue(SelectedModeProperty);
            }
            set
            {
                SetValue(SelectedModeProperty, value);
            }
        }

        #endregion

        #region SelectedRailsX

        public static readonly DependencyProperty SelectedRailsXProperty =
            DependencyProperty.Register("SelectedRailsX", typeof(double?), typeof(RailPlanControl),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnSelectedRailsXChanged)));

        public double? SelectedRailsX
        {
            get
            {
                return (double?)GetValue(SelectedRailsXProperty);
            }
            set
            {
                Debug.WriteLine($"RailPlanControl.SelectedRailsX new = {value}, old = {this.SelectedRailsX}");
                SetValue(SelectedRailsXProperty, value);
            }
        }

        private static void OnSelectedRailsXChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RailPlanControl railPlanControl = (RailPlanControl)o;
            railPlanControl.OnSelectedRailsXChanged((double?)e.NewValue, (double?)e.OldValue);
        }

        private void OnSelectedRailsXChanged(double? newValue, double? oldValue)
        {
            Debug.WriteLine($"RailPlanControl.OnSelectedRailsXChanged new = {newValue}, old = {oldValue}, var = {this.SelectedRailsX}");
            if (newValue.HasValue && oldValue.HasValue && newValue != oldValue && !this.selectedChangeIntern)
            {
                switch (this.SelectedMode)
                {
                case RailSelectedMode.Single:
                    var subgraph = this.selectedRail.FindSubgraph();
                    this.MoveRailItem(subgraph, new Vector(newValue.Value - oldValue.Value, 0));
                    this.InvalidateVisual();
                    break;
                case RailSelectedMode.Multi:
                    // only one can move
                    break;
                }
            }
        }

        #endregion

        #region SelectedRailsY

        public static readonly DependencyProperty SelectedRailsYProperty =
            DependencyProperty.Register("SelectedRailsY", typeof(double?), typeof(RailPlanControl),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnSelectedRailsYChanged)));

        public double? SelectedRailsY
        {
            get
            {
                return (double?)GetValue(SelectedRailsYProperty);
            }
            set
            {
                SetValue(SelectedRailsYProperty, value);
            }
        }

        private static void OnSelectedRailsYChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RailPlanControl railPlanControl = (RailPlanControl)o;
            railPlanControl.OnSelectedRailsYChanged((double?)e.NewValue);
        }

        private void OnSelectedRailsYChanged(double? newValue)
        {
            if (newValue.HasValue && SelectedRailsY.HasValue && newValue != SelectedRailsY && !this.selectedChangeIntern)
            {
                switch (this.SelectedMode)
                {
                case RailSelectedMode.Single:
                    var subgraph = this.selectedRail.FindSubgraph();
                    this.MoveRailItem(subgraph, new Vector(0, newValue.Value - SelectedRailsY.Value));
                    this.InvalidateVisual();
                    break;
                case RailSelectedMode.Multi:
                    // only one can move
                    break;
                }
            }
        }

        #endregion

        #region SelectedRailsAngle

        public static readonly DependencyProperty SelectedRailsAngleProperty =
            DependencyProperty.Register("SelectedRailsAngle", typeof(double?), typeof(RailPlanControl),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnSelectedRailsAngleChanged)));

        public double? SelectedRailsAngle
        {
            get
            {
                return (double?)GetValue(SelectedRailsAngleProperty);
            }
            set
            {
                SetValue(SelectedRailsAngleProperty, value);
            }
        }

        private static void OnSelectedRailsAngleChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RailPlanControl railPlanControl = (RailPlanControl)o;
            railPlanControl.OnSelectedRailsAngleChanged((double?)e.NewValue, (double?)e.OldValue);
        }

        private void OnSelectedRailsAngleChanged(double? newValue, double? oldValue)
        {
            if (newValue.HasValue && oldValue.HasValue && newValue != oldValue && !this.selectedChangeIntern)
            {
                switch (this.SelectedMode)
                {
                case RailSelectedMode.Single:
                    var subgraph = this.selectedRail.FindSubgraph();
                    this.RotateRailItem(subgraph, this.selectedRail.Position, new Rotation(newValue.Value) - new Rotation(oldValue.Value));
                    this.InvalidateVisual();
                    break;
                case RailSelectedMode.Multi:
                    // only one can rotated
                    break;
                }
            }
        }

        #endregion

        #region SelectedRailsLayer

        public static readonly DependencyProperty SelectedRailsLayerProperty =
            DependencyProperty.Register("SelectedRailsLayer", typeof(Guid), typeof(RailPlanControl),
                new FrameworkPropertyMetadata(Guid.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnSelectedRailsLayerChanged)));

        public Guid SelectedRailsLayer
        {
            get
            {
                return (Guid)GetValue(SelectedRailsLayerProperty);
            }
            set
            {
                SetValue(SelectedRailsLayerProperty, value);
            }
        }

        private static void OnSelectedRailsLayerChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RailPlanControl railPlanControl = (RailPlanControl)o;
            railPlanControl.OnSelectedRailsLayerChanged((Guid)e.NewValue, (Guid)e.OldValue);
        }

        private void OnSelectedRailsLayerChanged(Guid newValue, Guid oldValue)
        {
            if (newValue != Guid.Empty && newValue != oldValue)
            {
                switch (this.SelectedMode)
                {
                case RailSelectedMode.Single:
                    this.selectedRail.Layer = newValue;
                    this.InvalidateVisual();
                    break;
                case RailSelectedMode.Multi:
                    this.selectedRails.ForEach(r => r.Layer = newValue);
                    this.InvalidateVisual();
                    break;
                }
            }
        }

        #endregion

        #region SelectedRamp

        public static readonly DependencyProperty SelectedRampProperty =
            DependencyProperty.Register("SelectedRamp", typeof(RailRamp), typeof(RailPlanControl),
                new FrameworkPropertyMetadata(null));

        public RailRamp SelectedRamp
        {
            get
            {
                return (RailRamp)GetValue(SelectedRampProperty);
            }
            set
            {
                SetValue(SelectedRampProperty, value);
            }
        }

        #endregion

        #region Materials

        public static readonly DependencyProperty MaterialsProperty =
            DependencyProperty.Register("Materials", typeof(IList), typeof(RailPlanControl), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public IList Materials
        {
            get 
            {
                return (IList)GetValue(MaterialsProperty);
            }
            set
            {
                SetValue(MaterialsProperty, value);
            }
        }

        #endregion


        private void CalcGroundSize()
        {
            this.Width =  margin * 2 + this.RailPlan.Width  * this.ZoomFactor;
            this.Height = margin * 2 + this.RailPlan.Height * this.ZoomFactor;
            this.InvalidateMeasure();
            this.InvalidateVisual();
        }

        private void Invalidate()
        {
            this.UpdateSelectedRails();
            this.UpdateMaterials();
            
            this.InvalidateVisual();
        }

        private void UpdateMaterials()
        {
            if (this.Materials == null)
            {
                return;
            }
            
            this.Materials.Clear();

            var list = this.RailPlan.Rails.SelectMany(r => r.Materials).GroupBy(m => m.Id).Select(g => new TrackMaterial
            {
                Id = g.First().Id,
                Number = g.Select(i => i.Number).Sum(),
                Manufacturer = g.First().Manufacturer,
                Article = g.First().Article,
                Name = g.First().Name
            }).OrderBy(g => g.Article).ToList();

            list.ForEach(m => this.Materials.Add(m));
            //this.RailPlan.Rails.SelectMany(r => r.Materials).ForEach(m => this.Materials.Add(m));
        }

        #region render

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (this.RailPlan == null)
            {
                return;
            }

            // drawn background is needed for detecting mouse moves
            drawingContext.DrawRectangle(this.Background, null, new Rect(0, 0, this.Width, this.Height));

            TransformGroup transformGroup = new TransformGroup();
            transformGroup.Children.Add(new ScaleTransform(this.ZoomFactor, this.ZoomFactor));
            transformGroup.Children.Add(new TranslateTransform(margin, margin));
            drawingContext.PushTransform(transformGroup);

            // dray plate
            RenderPlate(drawingContext);

            // loop over all visible layers
            foreach (RailLayer railLayer in this.RailPlan.Layers.Where(l => l.Show))
            {
                // draw tracks
                var rails = this.RailPlan.Rails.Where(r => r.Layer == railLayer.Id).ToArray();
                rails.ForEach(r => r.DrawRailItem(drawingContext, this.ViewMode, this.RailPlan.Layers.FirstOrDefault(l => l.Id == r.Layer)));
                if (this.ShowDockingPoints)
                {
                    rails.ForEach(r => r.DrawDockPoints(drawingContext));
                }
            }

            // draw select rect
            if (this.actionType == RailAction.SelectRect)
            {
                drawingContext.DrawRectangle(null, selectFramePen1, new Rect(this.selectRecStart, this.lastMousePosition));
                drawingContext.DrawRectangle(null, selectFramePen2, new Rect(this.selectRecStart, this.lastMousePosition));
            }
            
            drawingContext.Pop();
            DebugText(drawingContext);
        }
        
              
        /// <summary>
        /// Render ground plate
        /// </summary>
        /// <param name="drawingContext">Drawing context</param>
        protected void RenderPlate(DrawingContext drawingContext)
        {
            drawingContext.DrawGeometry(TrackBrushes.Plate, plateFramePen, new PathGeometry(new PathFigureCollection
            {
                new PathFigure(this.RailPlan.PlatePoints.FirstOrDefault(), new PathSegmentCollection
                (
                    this.RailPlan.PlatePoints.Skip(1).Select(p => new LineSegment(p, true))
                ), true)
            }));
        }

        

        #endregion

        #region actions

        private RailBase FindRailItem(Point point)
        {
            RailBase track = this.RailPlan.Rails.Where(t => t.IsInside(point, this.ViewMode)).FirstOrDefault();
            return track;
        }

        public void InsertRailItem(Point pos)
        {
            Debug.WriteLine($"InsertRailItem at ({pos.X},{pos.Y})");
            
            // insert selected track at mouse position
            this.RailPlan.Rails.Add(new RailItem(this.SelectedTrack, pos, this.InsertLayer.Id));
        }

        public void InsertRailItem(RailDockPoint railDockPoint)
        {
            Debug.WriteLine($"InsertRailItem at DockPoint ({railDockPoint.DebugDockPointIndex},{railDockPoint.DebugDockPointIndex})");

            RailBase railItem = new RailItem(this.SelectedTrack, new Point(0, 0), this.InsertLayer.Id);
            Point pos = ((RailItem)railItem).Track.DockPoints.First().Position;
            //RailDockPoint newRailDockPoint = railItem.DockPoints.First();

            railItem.Move((Vector)railDockPoint.Position + (Vector)pos);

            this.RailPlan.Rails.Add(railItem);
            //FindDocking(this.actionTrack, this.dockedTracks);
        }

        public void DeleteRailItem(RailBase railItem)
        {
            // delete all docks of the item
            railItem.DockPoints.Where(dp => dp.IsDocked).ForEach(dp => dp.Undock());
            // remove the item
            this.RailPlan.Rails.Remove(railItem);
        }

        public void DeleteSelectedRailItems()
        {
            // delete all docks of the item
            var list = this.RailPlan.SelectedRails.ToList();
            // remove the item
            list.ForEach(r => DeleteRailItem(r));
        }

        public void SelectRailItem(RailBase railItem, bool addSelect)
        {
            if (addSelect)
            {
                railItem.IsSelected = !railItem.IsSelected;
            }
            else
            {
                this.RailPlan.Rails.ForEach(r => r.IsSelected = false);
                railItem.IsSelected = true;
            }
        }

        private void SelectRectange(Rect rec, bool addSelect)
        {
            if (!addSelect)
            {
                this.RailPlan.Rails.ForEach(r => r.IsSelected = false);
            }
            this.RailPlan.Rails.Where(r => r.IsInside(rec, this.ViewMode)).ForEach(r => r.IsSelected = true);
        }

        public void UnselectAllRailItems()
        {
            this.RailPlan.Rails.ForEach(r => r.IsSelected = false);
        }

        private void MoveRailItem(IEnumerable<RailBase> subgraph, Vector move)
        {
            //Debug.WriteLine($"MoveRailItem {railItem.DebugIndex} ({move.X:F2},{move.Y:F2}) with subgraph");

            subgraph.ForEach(t => t.Move(move));
        }

        private void RotateRailItem(IEnumerable<RailBase> subgraph, Point center, Rotation rotation)
        {
            subgraph.ForEach(t => t.Rotate(rotation, center));
        }



        //private RailItem FindDocking(RailItem railItem)
        //{
        //    if (this.RailPlan != null)
        //    {
        //        var dockPoints = railItem.DockPoints;
        //        var otherTracks = this.RailPlan.Rails.Where(t => t != railItem).ToList();

        //        foreach (var dockPoint in dockPoints)
        //        {
        //            foreach (var t in otherTracks)
        //            {
        //                foreach (var dp in t.DockPoints)
        //                {
        //                    //if (Math.Abs(dp.X - dockPoint.X) < dockDistance && Math.Abs(dp.Y - dockPoint.Y) < dockDistance)
        //                    if (dp.Distance(dockPoint) < dockDistance)
        //                    {
        //                        //railItem.Position += new Vector(dp.X - dockPoint.X, dp.Y - dockPoint.Y);
        //                        railItem.Position += dp.Position - dockPoint.Position;
        //                        return t;
        //                    }
        //                }
        //            }

        //        }
        //    }
        //    return null;
        //}


        private void FindDocking(RailBase railItem)
        {
            if (this.RailPlan.Rails != null)
            {
                var dockPoints = railItem.DockPoints.Where(dp => !dp.IsDocked).ToList();
                var otherTracks =
                    //docked != null ?
                    //this.RailPlan.Rails.Where(t => t != railItem).Where(t => !docked.Contains(t)).ToList() :
                    this.RailPlan.Rails.Where(t => t != railItem).ToList();

                DebugDockPoints(dockPoints);
                DebugRailItems(otherTracks);
                foreach (var dockPoint in dockPoints)
                {
                    foreach (RailBase t in otherTracks)
                    {
                        foreach (var dp in t.DockPoints.Where(dp => !dp.IsDocked))
                        {
                            //if (Math.Abs(dp.X - dockPoint.X) < dockDistance && Math.Abs(dp.Y - dockPoint.Y) < dockDistance)
                            if (dp.IsInside(dockPoint))
                            {
                                dockPoint.AdjustDock(dp);

                                this.actionType = RailAction.None;
                                //return t;
                            }
                        }
                    }

                }
            }
            //return null;
        }

        #endregion

        #region key handling

        private void OnKeyPress(object sender, KeyEventArgs e)
        {
            this.selectedChangeIntern = true;

            bool IsControlPressed = e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Control);
            bool IsShiftPressed = e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Shift);
            switch (e.Key)
            {
            // delete all selected
            case Key.Delete when !IsShiftPressed:
                this.OnDelete();
                break;
            // select all
            case Key.A when IsControlPressed:
                this.OnSelectAll();
                break;
            // copy all selected
            case Key.C when IsControlPressed:
            case Key.Insert when IsControlPressed:
                this.OnCopy();
                break;
            // cut all selected
            case Key.X when IsControlPressed:
            case Key.Delete when IsShiftPressed:
                this.OnCut();
                break;
            // paste all copied
            case Key.C when IsControlPressed:
            case Key.Insert when IsShiftPressed:
                this.OnPaste();
                break;
            // duplicate all selected
            case Key.D when IsControlPressed:
                this.OnDuplicate();
                break;
            }
            this.selectedChangeIntern = false;
        }

        //protected override void OnKeyDown(KeyEventArgs e)
        //{
        //    if (e.Key == Key.Delete)
        //    {
        //        DeleteSelectedRailItems();
        //        this.InvalidateVisual();
        //    }
        //    base.OnKeyUp(e);
        //}
        
        #endregion

        #region mouse handling

        private Point GetMousePosition(MouseEventArgs e)
        {
            return e.GetPosition(this).Move(-margin, -margin).Scale(1.0 / this.ZoomFactor);
        }
               
        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            Point pos = GetMousePosition(e);
            this.selectedChangeIntern = true;

            RailBase railItem = FindRailItem(pos);
            if (railItem != null)
            {
                RailDockPoint railDockPoint = railItem?.DockPoints.FirstOrDefault(d => d.IsInside(pos));
                if (railDockPoint != null)
                {
                    InsertRailItem(railDockPoint);
                    Invalidate();
                    StoreToHistory();
                }
                else
                {
                    if (railItem is RailRamp)
                    {
                        OnEditRamp();
                    }
                    else if (railItem is RailItem item)
                    {
                        if (item.Track is TrackTurntable)
                        {
                            TurntableView view = new TurntableView { DataContext = new TurntableViewModel(item) };
                            if (view.ShowDialog().Value)
                            {
                                Invalidate();
                            }
                        }
                        else if (item.Track is TrackTransferTable)
                        {
                            TransferTableView view = new TransferTableView { DataContext = new TransferTableViewModel(item) };
                            if (view.ShowDialog().Value)
                            {
                                Invalidate();
                            }
                        }
                    }
                }
            }
            else
            {
                InsertRailItem(pos);
                Invalidate();
                StoreToHistory();
            }
            this.selectedChangeIntern = false;
            base.OnMouseDoubleClick(e);
            DebugCheckDockings();
        }

        
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            Point pos = this.lastMousePosition = GetMousePosition(e);
            this.hasMoved = false;
            this.selectedChangeIntern = true;

            // click inside track
            if ((this.actionRailItem = FindRailItem(pos)) != null)
            {
                // click inside docking point
                RailDockPoint dp = this.actionRailItem.DockPoints?.FirstOrDefault(d => d.IsInside(pos));
                if (dp != null)
                {
                    this.actionType = RailAction.Rotate;
                    this.actionSubgraph = this.actionRailItem.FindSubgraph();
                }
                // click outside docking point
                else
                {
                    // CTRL pressed
                    if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
                    {
                        this.actionRailItem.UndockAll();
                        this.actionType = RailAction.MoveSingle;
                        this.actionSubgraph = null;
                    }
                    // SHIFT pressed
                    else if (Keyboard.Modifiers.HasFlag(ModifierKeys.Shift))
                    {
                        this.actionType = RailAction.MoveSimple;
                        this.actionSubgraph = this.actionRailItem.FindSubgraph();
                    }
                    else
                    {
                        this.actionType = RailAction.MoveGraph;
                        this.actionSubgraph = this.actionRailItem.FindSubgraph();
                    }
                }
            }
            else if(e.ClickCount == 1)
            {
                this.actionType = RailAction.SelectRect;
                this.selectRecStart = pos;
            }
            
            this.CaptureMouse();
            this.Invalidate();

            base.OnMouseLeftButtonDown(e);
            DebugCheckDockings();
            Keyboard.Focus(this);
        }

        

        protected override void OnMouseMove(MouseEventArgs e)
        {
            Point pos = GetMousePosition(e);
            this.MousePosition = pos;
            this.hasMoved = true;
                        
            switch (this.actionType)
            {
            case RailAction.MoveSingle:
                this.actionRailItem.Move(pos - this.lastMousePosition);
                Invalidate();
                break;
            case RailAction.MoveSimple:
                MoveRailItem(this.actionSubgraph, pos - this.lastMousePosition);
                // don't find docks
                Invalidate();
                break;
            case RailAction.MoveGraph:
                MoveRailItem(this.actionSubgraph, pos - this.lastMousePosition);
                FindDocking(this.actionRailItem);
                Invalidate();
                break;
            case RailAction.Rotate:
                Rotation rotation = Rotation.Calculate(this.actionRailItem.Position, this.lastMousePosition, pos);
                RotateRailItem(this.actionSubgraph, this.actionRailItem.Position, rotation);
                FindDocking(this.actionRailItem);
                Invalidate();
                break;
            case RailAction.SelectRect:
                Invalidate();
                break;
            }
            this.lastMousePosition = pos;

            base.OnMouseMove(e);
            DebugCheckDockings();
        }

        

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            Point pos = GetMousePosition(e);

            
            //Vector move = pos - this.lastMousePosition;
            //RailItem dockingTrack;

            bool addSelect = Keyboard.Modifiers.HasFlag(ModifierKeys.Shift) || Keyboard.Modifiers.HasFlag(ModifierKeys.Control);

            switch (this.actionType)
            {
            case RailAction.MoveSingle:
            case RailAction.MoveSimple:
            case RailAction.MoveGraph:
            case RailAction.Rotate:
                StoreToHistory();
                break;
            case RailAction.SelectRect:
                SelectRectange(new Rect(this.selectRecStart, pos), addSelect);
                break;
            }
           
            // handle select
            if (!this.hasMoved)
            {
                if (this.actionRailItem != null)
                {
                    SelectRailItem(this.actionRailItem, addSelect);
                }
                else
                {
                    UnselectAllRailItems();
                }
            }

            this.actionType = RailAction.None;
            this.actionSubgraph = null;
            this.actionRailItem = null;
            Invalidate();
            this.ReleaseMouseCapture();
            this.selectedChangeIntern = false;

            base.OnMouseLeftButtonUp(e);
            DebugCheckDockings();
        }

        // no right mouse button because Apple has no one

        //protected override void OnMouseRightButtonUp(MouseButtonEventArgs e)
        //{
        //    //Point pos = GetMousePosition(e);

        //    //var railItem = FindRailItem(pos);
        //    //if (railItem != null)
        //    //{
        //    //    ContextMenu contextMenu = new ContextMenu();
        //    //    //contextMenu.DataContext = railItem;
        //    //    contextMenu.Items.Add(new MenuItem() { Header = Rail.Properties.Resources.MenuDelete, Command = DeleteRailItemCommand, CommandParameter = railItem });
        //    //    contextMenu.Items.Add(new MenuItem() { Header = Rail.Properties.Resources.MenuRotate, Command = RotateRailItemCommand, CommandParameter = railItem, IsEnabled = railItem.HasOnlyOneDock });
        //    //    contextMenu.Items.Add(new Separator());
        //    //    contextMenu.Items.Add(new MenuItem() { Header = Rail.Properties.Resources.MenuProperties, Command = PropertiesRailItemCommand, CommandParameter = railItem });
        //    //    contextMenu.IsOpen = true;
        //    //}
        //    //base.OnMouseRightButtonUp(e);
        //}

        #endregion

        #region history

        private int historyIndex = -1;
        private readonly List<RailPlan> history = new List<RailPlan>();

        private void OnUndo()
        { 
            if (OnCanUndo())
            {
                this.RailPlan = history[--historyIndex];
            }
        }

        private bool OnCanUndo()
        {
            return historyIndex > 0;
        }
        
        private void OnRedo()
        {
            if (OnCanRedo())
            {
                this.RailPlan = history[++historyIndex];
            }
        }
        
        private bool OnCanRedo()
        {
            return historyIndex >= 0 && historyIndex < history.Count - 1; 
        }

        /// <summary>
        /// call always befor manipulating the RailPlan
        /// </summary>
        [Conditional("USERHISTORY")]
        private void StoreToHistory()
        {
            if (historyIndex >= 0 && historyIndex < history.Count - 1)
            {
                this.history.RemoveRange(historyIndex + 1, history.Count - 1 - historyIndex);
            }
            this.history.Add(this.RailPlan.Clone());
            historyIndex = this.history.Count - 1;
        }

        #endregion

        #region copy & past

        private List<RailBase> copy = null;
        private int copyFactor;
        
        public void Clone()
        {
            // clone tree
            this.RailPlan = this.RailPlan.Clone();
            // clone dock point links
            RailDockPoint.CloneDockPointLinks();
        }

        private void OnCopy()
        {
            if (OnCanCopy())
            {
                this.copy = this.RailPlan.SelectedRails.ToList();
                this.copyFactor = 1;
            }
        }

        private bool OnCanCopy()
        {
            return this.RailPlan.Rails.Any(r => r.IsSelected);
        }

        private void OnCut()
        {
            if (OnCanCut())
            {
                this.copy = this.RailPlan.SelectedRails.ToList();
                this.copy.ForEach(r => DeleteRailItem(r));
                this.copyFactor = 1;
                StoreToHistory();
                this.Invalidate();
            }
        }
        private bool OnCanCut()
        { 
            return this.RailPlan.Rails.Any(r => r.IsSelected); 
        }

        private void OnPaste()
        {
            if (OnCanPaste())
            {
                this.RailPlan.Rails.AddRange(copy.Select(r => r.Clone().Move(new Vector(copyPositionDrift * this.copyFactor, copyPositionDrift * this.copyFactor))));
                this.copyFactor++;
                // clone dock point links
                RailDockPoint.CloneDockPointLinks();

                StoreToHistory();
                this.Invalidate();
            }
        }

        private bool OnCanPaste()
        {
            return copy != null; 
        }

        private void OnDelete()
        {
            if (OnCanDelete())
            {
                var list = this.RailPlan.SelectedRails.ToList();
                list.ForEach(r => DeleteRailItem(r));
                StoreToHistory();
                this.Invalidate();
            }
        }
        
        private bool OnCanDelete()
        { 
            return this.RailPlan.Rails.Any(r => r.IsSelected); 
        }

        private void OnDuplicate()
        {
            if (OnCanDuplicate())
            {
                var selectedRails = this.RailPlan.SelectedRails.ToList();
                this.RailPlan.Rails.AddRange(selectedRails.Select(r => r.Clone().Move(new Vector(copyPositionDrift * this.copyFactor, copyPositionDrift * this.copyFactor))));
                this.copyFactor = 1;
                // clone dock point links
                RailDockPoint.CloneDockPointLinks();

                StoreToHistory();
                this.Invalidate();
            }
        }

        private bool OnCanDuplicate()
        { 
            return this.RailPlan.Rails.Any(r => r.IsSelected); 
        }
        
        private void OnSelectAll()
        {
            this.RailPlan.Rails.ForEach(r => r.IsSelected = true);
            this.Invalidate();
        }

        private bool OnCanSelectAll()
        {
            return this.RailPlan.Rails.Count() > 0; 
        }

        //private void OnUnselectAll()
        //{
        //    this.RailPlan.Rails.ForEach(r => r.IsSelected = false);
        //    this.Invalidate();
        //}

        #endregion

        #region group

        private bool OnCanCreateGroup()
        {
            return this.SelectedMode == RailSelectedMode.Multi && this.RailPlan.SelectedRails.All(r => r is RailItem);
        }

        private void OnCreateGroup()
        {
            // take all selected rails
            var selectedRails = this.RailPlan.SelectedRails.ToArray();

            // remove from Rails
            selectedRails.ForEach(r => this.RailPlan.Rails.Remove(r));

            // create rail group
            this.RailPlan.Rails.Add(new RailGroup(selectedRails));

            Invalidate();
        }

        private void OnResolveGroup()
        { 
            if (this.selectedRail is RailGroup railGroup)
            {
                this.RailPlan.Rails.AddRange(railGroup.Resolve());
                this.RailPlan.Rails.Remove(railGroup);
                Invalidate();
            }
        }

        private bool OnCanResolveGroup()
        {
            return this.SelectedMode == RailSelectedMode.Single && this.selectedRail is RailGroup;
        }

        private void OnSaveAsGroup()
        {
        }

        private bool OnCanSaveAsGroup()
        {
            return this.SelectedMode == RailSelectedMode.Single && this.selectedRail is RailGroup; 
        }

        #endregion

        #region ramp

        private bool OnCanCreateRamp()
        {
            return 
                this.SelectedMode == RailSelectedMode.Multi && 
                this.RailPlan.SelectedRails.All(r => r is RailItem) &&
                this.RailPlan.SelectedRails.Count() >= 8;
        }

        private void OnCreateRamp()
        {
            // take all selected rails
            var selectedRails = this.RailPlan.SelectedRails.ToArray();

            RailRamp railRamp = new RailRamp(selectedRails);

            RampView rampView = new RampView { DataContext = new RampViewModel { RailRamp = railRamp } };
            if (rampView.ShowDialog().Value)
            {
                // remove from Rails
                selectedRails.ForEach(r => this.RailPlan.Rails.Remove(r));
                // add rail group
                this.RailPlan.Rails.Add(railRamp);
                Invalidate();
            }
        }

        private void OnDeleteRamp()
        {
            if (OnCanDeleteRamp() && this.selectedRail is RailRamp railRamp)
            {
                this.RailPlan.Rails.AddRange(railRamp.Resolve());
                this.RailPlan.Rails.Remove(railRamp);
                Invalidate();
            }
        }

        private bool OnCanDeleteRamp()
        {
            return this.SelectedMode == RailSelectedMode.Single && this.selectedRail is RailRamp;
        }

        private void OnEditRamp()
        {
            if (OnCanEditRamp())
            {
                RampView rampView = new RampView { DataContext = new RampViewModel { RailRamp = (RailRamp)this.selectedRail } };
                if (rampView.ShowDialog().Value)
                {
                }
            }
        }

        private bool OnCanEditRamp()
        {
            return this.SelectedMode == RailSelectedMode.Single && this.selectedRail is RailRamp;
        }
        #endregion

        #region helix

        private void OnCreateHelix()
        { }
        private bool OnCanCreateHelix()
        {
            return  this.SelectedMode == RailSelectedMode.Multi &&
                    this.RailPlan.SelectedRails.All(r => r is RailItem ri && ri.Track is TrackCurved) &&
                    this.RailPlan.SelectedRails.Count() >= 16;
        }

        private void OnDeleteHelix()
        { }

        private bool OnCanDeleteHelix()
        {
            return this.SelectedMode == RailSelectedMode.Single && this.selectedRail is RailHelix;
        }

        private void OnEditHelix() 
        { }

        private bool OnCanEditHelix()
        {
            return this.SelectedMode == RailSelectedMode.Single && this.selectedRail is RailHelix;
        }

    #endregion

    #region Debug

    [Conditional("DEBUGINFO")]
        private void DebugText(DrawingContext drawingContext)
        {
            ScrollViewer scrollViewer = this.Parent as ScrollViewer;
            double x = scrollViewer?.ContentHorizontalOffset ?? 0;
            double y = scrollViewer?.ContentVerticalOffset ?? 0;
            drawingContext.DrawText(new FormattedText($"Action {this.actionType} RailItem {this.actionRailItem?.DebugIndex} Pos {this.actionRailItem?.Position.X:F2}/{this.actionRailItem?.Position.Y:F2}/{this.actionRailItem?.Angle:F2}", CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface("Verdana"), 12, Brushes.Black, 1.25), new Point(x, y));
            //drawingContext.DrawText(new FormattedText($"Rotation debugRotationAngle {debugRotationAngle:F2}", CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface("Verdana"), 12, Brushes.Black, 1.25), new Point(x, y + 12));
            drawingContext.DrawText(new FormattedText("Test zzzzzzzzzzzzzzzzz", CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface("Verdana"), 12, Brushes.Black, 1.25), new Point(x, y + 24));
        }

        [Conditional("DEBUGINFO")]
        private void DebugRailItems(IEnumerable<RailBase> railItems)
        {
            if (railItems != null && railItems.Any())
            {
                Debug.WriteLine(railItems.Select(r => r.DebugIndex.ToString()).Aggregate((a, b) => $"{a}, {b}"));
            }
        }

        [Conditional("DEBUGINFO")]
        private void DebugDockPoints(IEnumerable<RailDockPoint> dockPoints)
        {
            if (dockPoints != null && dockPoints.Any())
            {
                Debug.WriteLine(dockPoints.Select(d => $"({d.DebugDockPointIndex}, {d.RailItem.DebugIndex})").Aggregate((a, b) => $"{a}, {b}"));
            }
        }

        [Conditional("DEBUGINFO")]
        private void DebugCheckDockings()
        {
            //foreach (var dock in this.RailPlan.Rails.SelectMany(i => i.DockPoints).Where(d => d.IsDocked))
            //{

            //    if (dock.Position != dock.DockedWith.Position)
            //    {
            //        throw new Exception($"Position {dock.Position} != {dock.DockedWith.Position}");
            //    }
            //    Angle rev = dock.DockedWith.Angle.Revert();
            //    if (dock.Angle != rev)
            //    {
            //        throw new Exception($"Angle {dock.Angle} != {rev}");
            //    }
            //    if (dock != dock.DockedWith.DockedWith)
            //    {
            //        throw new Exception();
            //    }
            //}
        }

        #endregion

    }
}
