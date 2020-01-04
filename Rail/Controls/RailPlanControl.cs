using Rail.Misc;
using Rail.Model;
using Rail.Mvvm;
using System;
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
        private readonly Pen blackPen = new Pen(Brushes.Black, 1);
        private readonly Brush plateBrush = new SolidColorBrush(Colors.Green);

        private readonly Pen selectPen1 = new Pen(Brushes.Black, 2);
        private readonly Pen selectPen2 = new Pen(Brushes.White, 2) { DashStyle = DashStyles.Dot };

        // mouse operation variables
        private Point lastMousePosition; 
        private RailAction actionType;
        private RailItem actionRailItem;
        private bool hasMoved;
        private List<RailItem> actionRailItemDockedRailItems;
        private Angle debugRotationAngle;
        private Point selectRecStart;

        public DelegateCommand<RailItem> DeleteRailItemCommand { get; private set; }

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

        #region ShowLayer

        public static readonly DependencyProperty ShowLayerProperty =
            DependencyProperty.Register("ShowLayer", typeof(ushort), typeof(RailPlanControl),
                new FrameworkPropertyMetadata((ushort)0, new PropertyChangedCallback(OnShowLayerChanged)));

        public ushort ShowLayer
        {
            get
            {
                return (ushort)GetValue(ShowLayerProperty);
            }
            set
            {
                SetValue(ShowLayerProperty, value);
            }
        }

        private static void OnShowLayerChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RailPlanControl railPlan = (RailPlanControl)o;
            railPlan.InvalidateVisual();
        }

        #endregion

        #region InsertLayer

        public static readonly DependencyProperty InsertLayerProperty =
            DependencyProperty.Register("InsertLayer", typeof(ushort), typeof(RailPlanControl),
                new FrameworkPropertyMetadata((ushort)1));

        public ushort InsertLayer
        {
            get
            {
                return (ushort)GetValue(InsertLayerProperty);
            }
            set
            {
                SetValue(InsertLayerProperty, value);
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

            // drawn background is needed for detecting mouse moves
            drawingContext.DrawRectangle(this.Background, null, new Rect(0, 0, this.Width, this.Height));

            TransformGroup transformGroup = new TransformGroup();
            transformGroup.Children.Add(new ScaleTransform(this.ZoomFactor, this.ZoomFactor));
            transformGroup.Children.Add(new TranslateTransform(margin, margin));
            drawingContext.PushTransform(transformGroup);

            // dray plate
            RenderPlate(drawingContext);

            // draw tracks
            var rails = this.RailPlan.Rails.Where(r => this.ShowLayer == 0 || r.Layer == this.ShowLayer).ToArray();
            rails.ForEach(r => r.DrawRailItem(drawingContext, this.ViewMode));
            if (this.ShowDockingPoints)
            {
                rails.ForEach(r => r.DrawDockPoints(drawingContext));
            }

            // draw select rex
            if (this.actionType == RailAction.SelectRect)
            {
                drawingContext.DrawRectangle(null, selectPen1, new Rect(this.selectRecStart, this.lastMousePosition));
                drawingContext.DrawRectangle(null, selectPen2, new Rect(this.selectRecStart, this.lastMousePosition));
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
            drawingContext.DrawGeometry(plateBrush, blackPen, new PathGeometry(new PathFigureCollection
            {
                new PathFigure(this.RailPlan.PlatePoints.FirstOrDefault(), new PathSegmentCollection
                (
                    this.RailPlan.PlatePoints.Skip(1).Select(p => new LineSegment(p, true))
                ), true)
            }));
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
            this.RailPlan.Rails.Add(new RailItem(this.SelectedTrack, pos, this.InsertLayer));
        }

        public void InsertRailItem(RailDockPoint railDockPoint)
        {
            Debug.WriteLine($"InsertRailItem at DockPoint ({railDockPoint.DebugIndexDockPoint},{railDockPoint.DebugIndexDockPoint})");

            RailItem railItem = new RailItem(this.SelectedTrack, new Point(0, 0), this.InsertLayer);
            Point pos = railItem.Track.DockPoints.First().Position;
            //RailDockPoint newRailDockPoint = railItem.DockPoints.First();

            railItem.Move((Vector)railDockPoint.Position + (Vector)pos);

            this.RailPlan.Rails.Add(railItem);
            //FindDocking(this.actionTrack, this.dockedTracks);
        }

        public void DeleteRailItem(RailItem railItem)
        {
            // delete all docks of the item
            railItem.DockPoints.Where(dp => dp.IsDocked).ForEach(dp => { dp.DockedWith.DockedWith = null; dp.DockedWith = null; });
            // remove the item
            this.RailPlan.Rails.Remove(railItem);
            // redraw control
            //this.InvalidateVisual();
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

        public void UnselectAllRailItems()
        {
            this.RailPlan.Rails.ForEach(r => r.IsSelected = false);
            this.InvalidateVisual();
        }

        private void MoveRailItem(RailItem railItem, Vector move, IEnumerable<RailItem> subgraph)
        {
            Debug.WriteLine($"MoveRailItem {railItem.DebugIndex} ({move.X:F2},{move.Y:F2}) with subgraph");

            railItem.Move(move);
            subgraph?.Where(t => t != railItem).ForEach(t => t.Move(move));
            FindDocking(railItem, subgraph);
        }

        private void RotateRailItem(RailItem railItem, Angle angle, IEnumerable<RailItem> subgraph = null)
        {
            railItem.Angle = angle;
            subgraph?.Where(t => t != railItem).ForEach(tr => tr.Rotate(angle, railItem));
            FindDocking(railItem, subgraph);
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


        private RailItem FindDocking(RailItem railItem, IEnumerable<RailItem> docked = null)
        {
            if (this.RailPlan.Rails != null)
            {
                var dockPoints = railItem.DockPoints.Where(dp => !dp.IsDocked).ToList();
                var otherTracks =
                    docked != null ?
                    this.RailPlan.Rails.Where(t => t != railItem).Where(t => !docked.Contains(t)).ToList() :
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
                                dockPoint.Dock(dp);

                                this.actionType = RailAction.None;
                                return t;
                            }
                        }
                    }

                }
            }
            return null;
        }





        [Conditional("DEBUG")]
        private void CheckDockings()
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

        #region key handling

        protected override void OnKeyDown(KeyEventArgs e)
        {
            //if (e.Key == Key.e)
            base.OnKeyUp(e);
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);
        }

        #endregion

        #region mouse handling

        private Point GetMousePosition(MouseEventArgs e)
        {
            return e.GetPosition(this).Move(-margin, -margin).Scale(1.0 / this.ZoomFactor);
        }
               
        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            Point pos = GetMousePosition(e);

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
            base.OnMouseDoubleClick(e);
            CheckDockings();
        }

        
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            Point pos = GetMousePosition(e);
            this.hasMoved = false;

            // click inside track
            if ((this.actionRailItem = FindRailItem(pos)) != null)
            {
                // click inside docking point
                RailDockPoint dp = this.actionRailItem.DockPoints.FirstOrDefault(d => d.IsInside(pos));
                if (dp != null)
                {
                    this.actionType = RailAction.Rotate;
                    this.actionRailItemDockedRailItems = this.actionRailItem.FindSubgraph();
                    this.debugRotationAngle = Angle.Calculate(this.actionRailItem.Position, pos);
                }
                // click outside docking point
                else
                {
                    // SHIFT pressed
                    if (Keyboard.Modifiers.HasFlag(ModifierKeys.Shift))
                    {
                        this.actionRailItem.UndockAll();
                        this.actionType = RailAction.MoveSingle;
                        this.actionRailItemDockedRailItems = null;
                    }
                    // SHIFT not pressed
                    else
                    {
                        this.actionType = RailAction.MoveGraph;
                        this.actionRailItemDockedRailItems = this.actionRailItem.FindSubgraph();
                    }
                }
            }
            else
            {
                this.actionType = RailAction.SelectRect;
                this.selectRecStart = pos;
            }
            this.lastMousePosition = pos;
            this.CaptureMouse();
            this.InvalidateVisual();

            base.OnMouseLeftButtonDown(e);
            CheckDockings();
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
                this.lastMousePosition = pos;
                this.InvalidateVisual();
                break;
            case RailAction.MoveGraph:
                MoveRailItem(this.actionRailItem, pos - this.lastMousePosition, this.actionRailItemDockedRailItems);
                FindDocking(this.actionRailItem, this.actionRailItemDockedRailItems);
                this.lastMousePosition = pos;
                this.InvalidateVisual();
                break;
            case RailAction.Rotate:
                double rotationAngle = Angle.Calculate(this.actionRailItem.Position, pos);
                RotateRailItem(this.actionRailItem, rotationAngle, this.actionRailItemDockedRailItems);
                FindDocking(this.actionRailItem, this.actionRailItemDockedRailItems);
                this.debugRotationAngle = rotationAngle;
                this.lastMousePosition = pos;
                this.InvalidateVisual();
                break;
            case RailAction.SelectRect:
                this.lastMousePosition = pos;
                this.InvalidateVisual();
                break;
            }
                        
            base.OnMouseMove(e);
            CheckDockings();
        }

        

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            Point pos = GetMousePosition(e);

            
            Vector move = pos - this.lastMousePosition;
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
                double rotationAngle = Angle.Calculate(this.actionRailItem.Position, pos);
                RotateRailItem(this.actionRailItem, rotationAngle, this.actionRailItemDockedRailItems);
                FindDocking(this.actionRailItem, this.actionRailItemDockedRailItems);
                this.debugRotationAngle = rotationAngle;
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
            this.actionRailItemDockedRailItems = null;
            this.actionRailItem = null;
            this.InvalidateVisual();
            this.ReleaseMouseCapture();

            base.OnMouseLeftButtonUp(e);
            CheckDockings();
        }

        protected override void OnMouseRightButtonUp(MouseButtonEventArgs e)
        {
            Point pos = GetMousePosition(e);

            var railItem = FindRailItem(pos);
            if (railItem != null)
            {
                ContextMenu contextMenu = new ContextMenu();
                contextMenu.DataContext = railItem;
                contextMenu.Items.Add(new MenuItem() { Header = "Delete", Command = DeleteRailItemCommand, CommandParameter = railItem });
                contextMenu.Items.Add(new Separator());
                contextMenu.Items.Add(new MenuItem() { Header = "Options", Command = railItem.OptionsCommand });
                contextMenu.IsOpen = true;
            }
            base.OnMouseRightButtonUp(e);
        }

        #endregion

        #region commands

        private void OnDeleteRailItem(RailItem railItem)
        {
            DeleteRailItem(railItem);
        }

        #endregion

        #region Debug

        [Conditional("DEBUG")]
        private void DebugText(DrawingContext drawingContext)
        {
            ScrollViewer scrollViewer = this.Parent as ScrollViewer;
            double x = scrollViewer?.ContentHorizontalOffset ?? 0;
            double y = scrollViewer?.ContentVerticalOffset ?? 0;
            drawingContext.DrawText(new FormattedText($"Action {this.actionType} RailItem {this.actionRailItem?.DebugIndex} Pos {this.actionRailItem?.Position.X:F2}/{this.actionRailItem?.Position.Y:F2}/{this.actionRailItem?.Angle:F2}", CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface("Verdana"), 12, Brushes.Black, 1.25), new Point(x, y));
            drawingContext.DrawText(new FormattedText($"Rotation debugRotationAngle {debugRotationAngle:F2}", CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface("Verdana"), 12, Brushes.Black, 1.25), new Point(x, y + 12));
            drawingContext.DrawText(new FormattedText("Test zzzzzzzzzzzzzzzzz", CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface("Verdana"), 12, Brushes.Black, 1.25), new Point(x, y + 24));
        }

        [Conditional("DEBUG")]
        private void DebugRailItems(IEnumerable<RailItem> railItems)
        {
            if (railItems != null && railItems.Any())
            {
                Debug.WriteLine(railItems.Select(r => r.DebugIndex.ToString()).Aggregate((a, b) => $"{a}, {b}"));
            }
        }

        [Conditional("DEBUG")]
        private void DebugDockPoints(IEnumerable<RailDockPoint> dockPoints)
        {
            if (dockPoints != null && dockPoints.Any())
            {
                Debug.WriteLine(dockPoints.Select(d => $"({d.DebugIndexDockPoint}, {d.RailItem.DebugIndex})").Aggregate((a, b) => $"{a}, {b}"));
            }
        }

        #endregion

    }
}
