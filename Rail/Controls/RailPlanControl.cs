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

namespace Rail.Controls
{
    public class RailPlanControl : Control
    {
        private double margin = 0;
        private readonly Pen plateFramePen = new Pen(TrackBrushes.PlateFrame, 1);
        private readonly Pen selectFramePen1 = new Pen(Brushes.Black, 2);
        private readonly Pen selectFramePen2 = new Pen(Brushes.White, 2) { DashStyle = DashStyles.Dot };

        // mouse operation variables
        private Point lastMousePosition; 
        private RailAction actionType;
        private RailItem actionRailItem;
        private bool hasMoved;
        private List<RailItem> actionSubgraph;
        //private Angle debugRotationAngle;
        private Point selectRecStart;

        private bool selectedChangeIntern = false;
        private RailItem selectedRail;
        private List<RailItem> selectedRails;

        public DelegateCommand<RailItem> DeleteRailItemCommand { get; private set; }
        public DelegateCommand<RailItem> RotateRailItemCommand { get; private set; }
        public DelegateCommand<RailItem> PropertiesRailItemCommand { get; private set; }
        
        protected enum RailAction
        {
            None,
            MoveSingle,
            MoveGraph,
            Rotate,
            SelectRect
        }

        static RailPlanControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RailPlanControl), new FrameworkPropertyMetadata(typeof(RailPlanControl)));
        }

        public RailPlanControl()
        {
            this.DeleteRailItemCommand = new DelegateCommand<RailItem>(OnDeleteRailItem);
            this.RotateRailItemCommand = new DelegateCommand<RailItem>(OnRotateRailItem);
            this.PropertiesRailItemCommand = new DelegateCommand<RailItem>(OnPropertiesRailItem);
            this.Loaded += OnLoaded;
        }

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
                new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnRailPlanPropertyChanged)));

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
                this.RailPlan.Rails.Where(r => r.IsSelected).ForEach(r => this.SelectedRails.Add(r));
            }
            
            var selectedRails = this.RailPlan.Rails.Where(r => r.IsSelected).ToList();
            switch (selectedRails.Count())
            {
            case 0:
                this.SelectedMode = RailSelectedMode.None;
                this.SelectedRailsX = null;
                this.SelectedRailsY = null;
                this.SelectedRailsAngle = null;
                this.SelectedRailsLayer = Guid.Empty;
                this.SelectedRailsGradient = null;
                this.SelectedRailsHeight = null;
                break;
            case 1:
                this.SelectedMode = RailSelectedMode.Single;
                this.selectedRail = selectedRails.Single();

                this.SelectedRailsX = this.selectedRail.X;
                this.SelectedRailsY = this.selectedRail.Y;
                this.SelectedRailsAngle = this.selectedRail.Angle;
                this.SelectedRailsLayer = this.selectedRail.Layer;
                this.SelectedRailsGradient = this.selectedRail.Gradient;
                this.SelectedRailsHeight = this.selectedRail.Height;
                break;
            default:
                this.SelectedMode = RailSelectedMode.Multi;
                this.selectedRails = selectedRails;

                this.SelectedRailsX = null;
                this.SelectedRailsY = null;
                this.SelectedRailsAngle = null;
                this.SelectedRailsLayer = selectedRails.Select(r => r.Layer).IdenticalOrDefault();
                this.SelectedRailsGradient = selectedRails.Select(r => r.Gradient).IdenticalOrDefault();
                this.SelectedRailsHeight = selectedRails.Select(r => r.Height).IdenticalOrDefault();
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
            railPlanControl.OnSelectedRailsYChanged((double?)e.NewValue, (double?)e.OldValue);
        }

        private void OnSelectedRailsYChanged(double? newValue, double? oldValue)
        {
            if (newValue.HasValue && SelectedRailsY.HasValue && newValue != SelectedRailsY && !this.selectedChangeIntern)
            {
                switch (this.SelectedMode)
                {
                case RailSelectedMode.Single:
                    this.selectedRail.Move(new Vector(0, newValue.Value - SelectedRailsY.Value));
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
            if (newValue.HasValue && oldValue.HasValue && newValue != oldValue)
            {
                switch (this.SelectedMode)
                {
                case RailSelectedMode.Single:
                    this.selectedRail.Rotate(new Rotation(oldValue.Value) - new Rotation(newValue.Value), this.selectedRail.Position);
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

        #region SelectedRailsGradient

        public static readonly DependencyProperty SelectedRailsGradientProperty =
            DependencyProperty.Register("SelectedRailsGradient", typeof(double?), typeof(RailPlanControl),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnSelectedRailsGradientChanged)));

        public double? SelectedRailsGradient
        {
            get
            {
                return (double?)GetValue(SelectedRailsGradientProperty);
            }
            set
            {
                SetValue(SelectedRailsGradientProperty, value);
            }
        }

        private static void OnSelectedRailsGradientChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RailPlanControl railPlanControl = (RailPlanControl)o;
            railPlanControl.OnSelectedRailsGradientChanged((double?)e.NewValue, (double?)e.OldValue);
        }

        private void OnSelectedRailsGradientChanged(double? newValue, double? oldValue)
        {
            if (newValue.HasValue && newValue != oldValue)
            {
                switch (this.SelectedMode)
                {
                case RailSelectedMode.Single:
                    this.selectedRail.Gradient = newValue.Value;
                    this.InvalidateVisual();
                    break;
                case RailSelectedMode.Multi:
                    this.selectedRails.ForEach(r => r.Gradient = newValue.Value);
                    this.InvalidateVisual();
                    break;
                }
            }
        }


        #endregion

        #region SelectedRailsHeight

        public static readonly DependencyProperty SelectedRailsHeightProperty =
            DependencyProperty.Register("SelectedRailsHeight", typeof(double?), typeof(RailPlanControl),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnSelectedRailsHeightChanged)));

        public double? SelectedRailsHeight
        {
            get
            {
                return (double?)GetValue(SelectedRailsHeightProperty);
            }
            set
            {
                SetValue(SelectedRailsHeightProperty, value);
            }
        }

        private static void OnSelectedRailsHeightChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RailPlanControl railPlanControl = (RailPlanControl)o;
            railPlanControl.OnSelectedRailsHeightChanged((double?)e.NewValue, (double?)e.OldValue);
        }

        private void OnSelectedRailsHeightChanged(double? newValue, double? oldValue)
        {
            if (newValue.HasValue && newValue != oldValue)
            {
                switch (this.SelectedMode)
                {
                case RailSelectedMode.Single:
                    this.selectedRail.Height = newValue.Value;
                    this.InvalidateVisual();
                    break;
                case RailSelectedMode.Multi:
                    this.selectedRails.ForEach(r => r.Height = newValue.Value);
                    this.InvalidateVisual();
                    break;
                }
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

        private void Invalidate()
        {
            this.UpdateSelectedRails();
            this.InvalidateVisual();
        }

        #endregion

        #region actions

        private RailItem FindRailItem(Point point)
        {
            RailItem track = this.RailPlan.Rails.Where(t => t.IsInside(point, this.ViewMode)).FirstOrDefault();
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

            RailItem railItem = new RailItem(this.SelectedTrack, new Point(0, 0), this.InsertLayer.Id);
            Point pos = railItem.Track.DockPoints.First().Position;
            //RailDockPoint newRailDockPoint = railItem.DockPoints.First();

            railItem.Move((Vector)railDockPoint.Position + (Vector)pos);

            this.RailPlan.Rails.Add(railItem);
            //FindDocking(this.actionTrack, this.dockedTracks);
        }

        public void DeleteRailItem(RailItem railItem)
        {
            // delete all docks of the item
            railItem.DockPoints.Where(dp => dp.IsDocked).ForEach(dp => dp.Undock());
            // remove the item
            this.RailPlan.Rails.Remove(railItem);
        }

        public void DeleteSelectedRailItems()
        {
            // delete all docks of the item
            var list = this.RailPlan.Rails.Where(r => r.IsSelected).ToList();
            // remove the item
            list.ForEach(r => DeleteRailItem(r));
        }

        public void SelectRailItem(RailItem railItem, bool addSelect)
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

        public void SelectAllRailItems()
        {
            this.RailPlan.Rails.ForEach(r => r.IsSelected = true);
        }

        public void UnselectAllRailItems()
        {
            this.RailPlan.Rails.ForEach(r => r.IsSelected = false);
        }

        //private void MoveRailItem(RailItem railItem, Vector move, IEnumerable<RailItem> subgraph)
        //{
        //    Debug.WriteLine($"MoveRailItem {railItem.DebugIndex} ({move.X:F2},{move.Y:F2}) with subgraph");

        //    railItem.Move(move);
        //    subgraph?.Where(t => t != railItem).ForEach(t => t.Move(move));
        //}

        private void MoveRailItem(IEnumerable<RailItem> subgraph, Vector move)
        {
            //Debug.WriteLine($"MoveRailItem {railItem.DebugIndex} ({move.X:F2},{move.Y:F2}) with subgraph");

            subgraph.ForEach(t => t.Move(move));
        }

        //private void RotateRailItem(RailItem railItem, Angle angle, IEnumerable<RailItem> subgraph = null)
        //{
        //    railItem.Angle = angle;
        //    subgraph?.Where(t => t != railItem).ForEach(tr => tr.Rotate(angle, railItem));
        //}

        //private void RotateRailItem(RailItem railItem, Rotation rotation, IEnumerable<RailItem> subgraph = null)
        //{
        //    railItem.Rotate(rotation);
        //    subgraph?.Where(t => t != railItem).ForEach(tr => tr.Rotate(rotation, railItem));
        //}

        private void RotateRailItem(IEnumerable<RailItem> subgraph, Point center, Rotation rotation)
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


        private void FindDocking(RailItem railItem)
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
                    foreach (RailItem t in otherTracks)
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
            switch (e.Key)
            {
            case Key.Delete:
                DeleteSelectedRailItems();
                this.Invalidate();
                break;
            case Key.A when e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Control):
                this.SelectAllRailItems();
                this.Invalidate();
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

            RailItem railItem = FindRailItem(pos);
            if (railItem != null)
            {
                RailDockPoint railDockPoint = railItem?.DockPoints.FirstOrDefault(d => d.IsInside(pos));
                if (railDockPoint != null)
                {
                    InsertRailItem(railDockPoint);
                }
                else
                {
                    // do nothing
                }
            }
            else
            {
                InsertRailItem(pos);
            }
            this.selectedChangeIntern = false;
            base.OnMouseDoubleClick(e);
            DebugCheckDockings();
            //Keyboard.Focus(this);
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
                RailDockPoint dp = this.actionRailItem.DockPoints.FirstOrDefault(d => d.IsInside(pos));
                if (dp != null)
                {
                    this.actionType = RailAction.Rotate;
                    this.actionSubgraph = this.actionRailItem.FindSubgraph();
                }
                // click outside docking point
                else
                {
                    // SHIFT pressed
                    if (Keyboard.Modifiers.HasFlag(ModifierKeys.Shift))
                    {
                        this.actionRailItem.UndockAll();
                        this.actionType = RailAction.MoveSingle;
                        //this.actionSubgraph = null;
                    }
                    // SHIFT not pressed
                    else
                    {
                        this.actionType = RailAction.MoveGraph;
                        this.actionSubgraph = this.actionRailItem.FindSubgraph();
                    }
                }
            }
            else
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
            case RailAction.MoveGraph:
                MoveRailItem(this.actionSubgraph, pos - this.lastMousePosition);
                //MoveRailItem(this.actionRailItem, pos - this.lastMousePosition, this.actionSubgraph);
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
                //this.actionRailItem.Position += pos - this.lastMousePosition;
                //dockingTrack = FindDocking(this.actionRailItem);
                //if (dockingTrack != null)
                //{
                //    DockTo(this.actionRailItem, dockingTrack);
                //}
                break;
            case RailAction.MoveGraph:
                //this.actionRailItem.Position += pos - this.lastMousePosition;
                //dockingTrack = FindDocking(this.actionRailItem);
                //if (dockingTrack != null)
                //{
                //    DockTo(this.actionRailItem, dockingTrack);
                //}
                //foreach (RailItem track in this.actionRailItemDockedRailItems)
                //{
                //    track.Position += pos - this.lastMousePosition;
                //}
                break;
            case RailAction.Rotate:
                //double rotationAngle = Angle.Calculate(this.actionRailItem.Position, pos);
                //RotateRailItem(this.actionRailItem, rotationAngle, this.actionRailItemDockedRailItems);
                //FindDocking(this.actionRailItem, this.actionRailItemDockedRailItems);
                //this.debugRotationAngle = rotationAngle;
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

        protected override void OnMouseRightButtonUp(MouseButtonEventArgs e)
        {
            Point pos = GetMousePosition(e);

            var railItem = FindRailItem(pos);
            if (railItem != null)
            {
                ContextMenu contextMenu = new ContextMenu();
                //contextMenu.DataContext = railItem;
                contextMenu.Items.Add(new MenuItem() { Header = Rail.Properties.Resources.MenuDelete, Command = DeleteRailItemCommand, CommandParameter = railItem });
                contextMenu.Items.Add(new MenuItem() { Header = Rail.Properties.Resources.MenuRotate, Command = RotateRailItemCommand, CommandParameter = railItem, IsEnabled = railItem.HasOnlyOneDock });
                contextMenu.Items.Add(new Separator());
                contextMenu.Items.Add(new MenuItem() { Header = Rail.Properties.Resources.MenuProperties, Command = PropertiesRailItemCommand, CommandParameter = railItem });
                contextMenu.IsOpen = true;
            }
            base.OnMouseRightButtonUp(e);
        }

        #endregion

        #region commands

        private void OnDeleteRailItem(RailItem railItem)
        {
            this.selectedChangeIntern = true;
            DeleteRailItem(railItem);
            this.selectedChangeIntern = false;
            this.InvalidateVisual();
        }
                
        private void OnRotateRailItem(RailItem railItem)
        {
        }
        
        private void OnPropertiesRailItem(RailItem railItem)
        {

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
        private void DebugRailItems(IEnumerable<RailItem> railItems)
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
