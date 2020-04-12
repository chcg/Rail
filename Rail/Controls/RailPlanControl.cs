using Rail.Misc;
using Rail.Model;
using Rail.Tracks;
using Rail.Tracks.Trigonometry;
using Rail.View;
using Rail.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Rail.Controls
{
    public class RailPlanControl : Control
    {
        private double margin = 0;
        //private readonly Pen plateFramePen = new Pen(TrackBrushes.PlateFrame, 1);
        private readonly Pen selectFramePen1 = new Pen(Brushes.Black, 2);
        private readonly Pen selectFramePen2 = new Pen(Brushes.White, 2) { DashStyle = DashStyles.Dot };
        private readonly Pen selectFramePen3 = new Pen(Brushes.White, 2) { DashStyle = DashStyles.Dash };

        // mouse operation variables
        private Point lastMousePosition; 
        private RailAction actionType;
        private RailBase actionRailItem;
        private RailDockPoint actionDockPoint;
        private bool hasMoved;
        private List<RailBase> actionSubgraph;
        ////private Angle debugRotationAngle;
        private Point selectRecStart;

        //private bool selectedChangeIntern = false;
        //private RailBase selectedRail;
        //private List<RailBase> selectedRails;

        

       

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
            SelectRect,
            /// <summary>
            /// show dock point binding line
            /// </summary>
            BindingLine,
            /// <summary>
            /// measure distance between two points
            /// </summary>
            Measure
        }

        static RailPlanControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RailPlanControl), new FrameworkPropertyMetadata(typeof(RailPlanControl)));
        }

        public RailPlanControl()
        { }
       
        //private void OnLoaded(object sender, RoutedEventArgs e)
        //{
        //    DependencyObject dep = new DependencyObject();
        //    if (!DesignerProperties.GetIsInDesignMode(dep))
        //    {
        //        var window = Window.GetWindow(this);
        //        window.KeyDown += OnKeyPress;
        //    }
        //}
        
        #region Rail

        public static readonly DependencyProperty RailProperty =
            DependencyProperty.Register("Rail", typeof(IRail), typeof(RailPlanControl),
                new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnRailPropertyChanged)));

        public IRail Rail
        {
            get
            {
                return (IRail)GetValue(RailProperty);
            }
            set
            {
                SetValue(RailProperty, value);
            }
        }

        private static void OnRailPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RailPlanControl railPlan = (RailPlanControl)o;
            railPlan.OnRailPropertyChanged(e);
        }

        private void OnRailPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is IRail oldRail)
            {
                oldRail.RailChanged -= OnRailChanged;
            }
            if (e.NewValue is IRail newRail)
            {
                newRail.RailChanged += OnRailChanged;
            }
            CalcGroundSize();
        }

        private void OnRailChanged(object sender, EventArgs e)
        {
            Invalidate();
        }

        #endregion

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

        

        /*
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
                this.Rail.SelectedRails.ForEach(r => this.SelectedRails.Add(r));
            }
            
            var selectedRails = this.Rail.SelectedRails.ToList();
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
                    this.Rail.MoveRailItem(subgraph, new Vector(newValue.Value - oldValue.Value, 0));
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
                    this.Rail.MoveRailItem(subgraph, new Vector(0, newValue.Value - SelectedRailsY.Value));
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
                    this.Rail.RotateRailItem(subgraph, this.selectedRail.Position, new Rotation(newValue.Value) - new Rotation(oldValue.Value));
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

        
        */

        #region GridLinesDistance

        public static readonly DependencyProperty GridLinesDistanceProperty =
            DependencyProperty.Register("GridLinesDistance", typeof(double), typeof(RailPlanControl),
                new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(OnGridLinesDistanceChanged)));

        public double GridLinesDistance
        {
            get
            {
                return (double)GetValue(GridLinesDistanceProperty);
            }
            set
            {
                SetValue(GridLinesDistanceProperty, value);
            }
        }

        private static void OnGridLinesDistanceChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RailPlanControl railPlan = (RailPlanControl)o;
            railPlan.InvalidateVisual();
        }

        #endregion

        private void CalcGroundSize()
        {
            this.Width =  margin * 2 + this.Rail.Width  * this.ZoomFactor;
            this.Height = margin * 2 + this.Rail.Height * this.ZoomFactor;
            this.InvalidateMeasure();
            this.InvalidateVisual();
        }

        private void Invalidate()
        {
            this.InvalidateVisual();
        }
        
        #region render

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            // drawn background is needed for detecting mouse moves
            drawingContext.DrawRectangle(this.Background, null, new Rect(0, 0, this.Width, this.Height));

            if (this.Rail == null)
            {
                return;
            }

            TransformGroup transformGroup = new TransformGroup();
            transformGroup.Children.Add(new ScaleTransform(this.ZoomFactor, this.ZoomFactor));
            transformGroup.Children.Add(new TranslateTransform(margin, margin));
            drawingContext.PushTransform(transformGroup);

            // draw plate
            drawingContext.DrawDrawing(this.Rail.PlateDrawing);
            
            // loop over all visible layers
            foreach (RailLayer railLayer in this.Rail.Layers.Where(l => l.Show))
            {
                // draw tracks
                var rails = this.Rail.Rails.Where(r => r.Layer == railLayer.Id).ToArray();
                rails.ForEach(r => r.DrawRailItem(drawingContext, this.ViewMode, this.Rail.Layers.FirstOrDefault(l => l.Id == r.Layer)));
                if (this.ShowDockingPoints)
                {
                    rails.ForEach(r => r.DrawDockPoints(drawingContext));
                }
            }


            switch (this.actionType)
            {
            // draw select rect
            case RailAction.SelectRect:
                drawingContext.DrawRectangle(null, selectFramePen1, new Rect(this.selectRecStart, this.lastMousePosition));
                drawingContext.DrawRectangle(null, selectFramePen2, new Rect(this.selectRecStart, this.lastMousePosition));
                break;

            // draw binding line
            case RailAction.BindingLine:
                drawingContext.DrawLine(selectFramePen1, this.selectRecStart, this.lastMousePosition);
                drawingContext.DrawLine(selectFramePen2, this.selectRecStart, this.lastMousePosition);
                break;
            case RailAction.Measure:
                drawingContext.DrawLine(selectFramePen1, this.selectRecStart, this.lastMousePosition);
                drawingContext.DrawLine(selectFramePen3, this.selectRecStart, this.lastMousePosition);
                break;
            }

            RenderGridLines(drawingContext);

            drawingContext.Pop();
            DebugText(drawingContext);
        }
        
              
        
        
        private readonly Pen gridLinePen = new Pen(Brushes.Black, 0.2);

        protected void RenderGridLines(DrawingContext drawingContext)
        {
            if (this.GridLinesDistance > 0)
            {
                double w = this.Rail.Width;
                double h = this.Rail.Height;
                for (double x = 0; x < w; x += this.GridLinesDistance)
                {
                    drawingContext.DrawLine(gridLinePen, new Point(x, 0), new Point(x, h));
                }
                for (double y = 0; y < h; y += this.GridLinesDistance)
                {
                    drawingContext.DrawLine(gridLinePen, new Point(0, y), new Point(w, y));
                }
            }
        }

        #endregion

        #region key handling

        //private void OnKeyPress(object sender, KeyEventArgs e)
        //{
        //    this.selectedChangeIntern = true;

        //    bool IsControlPressed = e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Control);
        //    bool IsShiftPressed = e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Shift);
        //    switch (e.Key)
        //    {
        //    // delete all selected
        //    case Key.Delete when !IsShiftPressed:
        //        this.Rail.OnDelete();
        //        break;
        //    // select all
        //    case Key.A when IsControlPressed:
        //        this.OnSelectAll();
        //        break;
        //    // copy all selected
        //    case Key.C when IsControlPressed:
        //    case Key.Insert when IsControlPressed:
        //        this.OnCopy();
        //        break;
        //    // cut all selected
        //    case Key.X when IsControlPressed:
        //    case Key.Delete when IsShiftPressed:
        //        this.OnCut();
        //        break;
        //    // paste all copied
        //    case Key.C when IsControlPressed:
        //    case Key.Insert when IsShiftPressed:
        //        this.OnPaste();
        //        break;
        //    // duplicate all selected
        //    case Key.D when IsControlPressed:
        //        this.OnDuplicate();
        //        break;
        //    }
        //    this.selectedChangeIntern = false;
        //}

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
            //this.selectedChangeIntern = true;

            RailDockPoint dockPoint;
            RailBase railItem;
            // double click on free dock point
            if ((dockPoint = this.Rail.FindFreeDockPoint(pos)) != null)
            {
                this.Rail.InsertRailItem(dockPoint);
                Invalidate();
                //StoreToHistory();
            }
            // double click onrail item
            else if ((railItem = this.Rail.FindRailItem(pos)) != null)
            {
                if (railItem is RailRamp)
                {
                    this.Rail.OnEditRamp();
                }
                else if (railItem is RailItem item)
                {
                    if (item.Track is TrackTable)
                    {
                        TableView view = new TableView { DataContext = new TableViewModel(item) };
                        if (view.ShowDialog().Value)
                        {
                            Invalidate();
                        }
                    }
                    else
                    {
                        this.Rail.SwitchRailItemDocking(item);
                    }
                }
            }
            // double click on plate
            else
            {
                this.Rail.InsertRailItem(pos);
                Invalidate();
            }
            //this.selectedChangeIntern = false;
            base.OnMouseDoubleClick(e);
            DebugCheckDockings();
        }

        
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            Point pos = this.lastMousePosition = GetMousePosition(e);
            this.hasMoved = false;
            //this.selectedChangeIntern = true;

            // Alt pressed
            if (Keyboard.Modifiers.HasFlag(ModifierKeys.Alt))
            {
                this.actionType = RailAction.Measure;
                this.selectRecStart = pos;
            }
            else
            {
                // click inside track
                this.actionRailItem = this.Rail.FindRailItem(pos);
                this.actionSubgraph = this.actionRailItem?.FindSubgraph();
                if (this.actionRailItem != null && !this.Rail.IsAnchored(this.actionSubgraph))
                {
                    // click inside docking point
                    RailDockPoint dp = this.actionRailItem.DockPoints?.FirstOrDefault(d => d.IsInside(pos));
                    if (dp != null)
                    {
                        this.actionType = RailAction.Rotate;
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
                        }
                        else
                        {
                            this.actionType = RailAction.MoveGraph;
                        }
                    }
                }
                else if ((this.actionDockPoint = this.Rail.FindFreeDockPoint(pos)) != null)
                {
                    this.actionType = RailAction.BindingLine;
                    this.selectRecStart = pos;
                }
                else if (e.ClickCount == 1)
                {
                    this.actionType = RailAction.SelectRect;
                    this.selectRecStart = pos;
                }
            }
            this.CaptureMouse();
            this.Invalidate();

            base.OnMouseLeftButtonDown(e);
            DebugCheckDockings();
            //Keyboard.Focus(this);
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
                this.Rail.MoveRailItem(this.actionSubgraph, pos - this.lastMousePosition);
                // don't find docks
                Invalidate();
                break;
            case RailAction.MoveGraph:
                this.Rail.MoveRailItem(this.actionSubgraph, pos - this.lastMousePosition);
                this.Rail.FindDocking(this.actionRailItem);
                Invalidate();
                break;
            case RailAction.Rotate:
                Rotation rotation = Rotation.Calculate(this.actionRailItem.Position, this.lastMousePosition, pos);
                this.Rail.RotateRailItem(this.actionSubgraph, this.actionRailItem.Position, rotation);
                this.Rail.FindDocking(this.actionRailItem);
                Invalidate();
                break;
            case RailAction.SelectRect:
            case RailAction.BindingLine:
            case RailAction.Measure:
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
                //StoreToHistory();
                break;
            case RailAction.SelectRect:
                this.Rail.SelectRectange(new Rect(this.selectRecStart, pos), addSelect);
                break;
            case RailAction.BindingLine:
                this.Rail.BindDockingPoints(this.actionDockPoint, this.Rail.FindFreeDockPoint(pos));
                break;
            case RailAction.Measure:
                this.Rail.ShowMeasure(this.selectRecStart, pos);
                break;
            }
           
            // handle select
            if (!this.hasMoved)
            {
                if (this.actionRailItem != null)
                {
                    this.Rail.SelectRailItem(this.actionRailItem, addSelect);
                }
                else
                {
                    this.Rail.UnselectAllRailItems();
                }
            }

            this.actionType = RailAction.None;
            this.actionSubgraph = null;
            this.actionRailItem = null;
            Invalidate();
            this.ReleaseMouseCapture();
            //this.selectedChangeIntern = false;

            base.OnMouseLeftButtonUp(e);
            DebugCheckDockings();
        }

        // no right mouse button because Apple has no one

        //protected override void OnMouseRightButtonUp(MouseButtonEventArgs e)
        //{ }
        
        #endregion

        #region drag & drop

        protected override void OnDragEnter(DragEventArgs e)
        {
            string[] dataFormats = e.Data.GetFormats();
            if (dataFormats.Length > 0 && e.Data.GetData(dataFormats[0]) is TrackBase)
            {
                e.Handled = true;
            }

            base.OnDragEnter(e);
        }

        protected override void OnDrop(DragEventArgs e)
        {
            string[] dataFormats = e.Data.GetFormats();
            if (dataFormats.Length > 0 && e.Data.GetData(dataFormats[0]) is TrackBase trackBase)
            {
                var pos = e.GetPosition(this).Move(-this.margin, -this.margin).Scale(1.0 / this.ZoomFactor); 
                this.Rail.InsertRailItem(pos, trackBase);
                Invalidate();
                e.Handled = true;
            }
            base.OnDrop(e);
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
