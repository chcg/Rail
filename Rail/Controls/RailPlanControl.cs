using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Rail.Controls
{
    public class RailPlanControl : Control
    {
        public const double dockDistance = 10;
        public const double rotateDistance = 12;

        static RailPlanControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RailPlanControl), new FrameworkPropertyMetadata(typeof(RailPlanControl)));
        }

        public RailPlanControl()
        {

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

        #region GroundWidth

        public static readonly DependencyProperty GroundWidthProperty =
            DependencyProperty.Register("GroundWidth", typeof(double), typeof(RailPlanControl),
                new FrameworkPropertyMetadata(2000.0, new PropertyChangedCallback(OnGroundWidthChanged)));

        public double GroundWidth
        {
            get
            {
                return (double)GetValue(GroundWidthProperty);
            }
            set
            {
                SetValue(GroundWidthProperty, value);
            }
        }

        private static void OnGroundWidthChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RailPlanControl railPlan = (RailPlanControl)o;
            railPlan.CalcGroundSize();
        }

        #endregion

        #region GroundHeight

        public static readonly DependencyProperty GroundHeightProperty =
            DependencyProperty.Register("GroundHeight", typeof(double), typeof(RailPlanControl),
                new FrameworkPropertyMetadata(1000.0, new PropertyChangedCallback(OnGroundHeightChanged)));

        public double GroundHeight
        {
            get
            {
                return (double)GetValue(GroundHeightProperty);
            }
            set
            {
                SetValue(GroundHeightProperty, value);
            }
        }

        private static void OnGroundHeightChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RailPlanControl railPlan = (RailPlanControl)o;
            railPlan.CalcGroundSize();
        }

        #endregion

        #region Tracks

        public static readonly DependencyProperty TracksProperty =
            DependencyProperty.Register("Tracks", typeof(ObservableCollection<ItemBase>), typeof(RailPlanControl),
                new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnTracksChanged)));

        public ObservableCollection<ItemBase> Tracks
        {
            get
            {
                return (ObservableCollection<ItemBase>)GetValue(TracksProperty);
            }
            set
            {
                SetValue(TracksProperty, value);
            }
        }

        private static void OnTracksChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RailPlanControl railPlan = (RailPlanControl)o;
            if (e.OldValue != null)
            { 
                ((ObservableCollection<ItemBase>)e.OldValue).CollectionChanged -= railPlan.OnTracksCollectionChanged;
            }
            if (e.NewValue != null)
            {
                ((ObservableCollection<ItemBase>)e.NewValue).CollectionChanged += railPlan.OnTracksCollectionChanged;
            }
            railPlan.InvalidateVisual();
        }

        private void OnTracksCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            InvalidateVisual();
        }

        #endregion

        private void CalcGroundSize()
        {
            this.Width = this.GroundWidth * this.ZoomFactor;
            this.Height = this.GroundHeight * this.ZoomFactor;
            this.InvalidateMeasure();
            this.InvalidateVisual();
        }

        //private double Sin(double value)
        //{
        //    return Math.Sin(value * Math.PI / 180.0);
        //}

        //private double Cos(double value)
        //{
        //    return Math.Cos(value * Math.PI / 180.0);
        //}

        private readonly Pen blackPen = new Pen(Brushes.Black, 1); 
        

        //private double radius = 360.0;
        ////private double angle = 30.0;
        //private double railWidth = 5.0;

        protected override void OnRender(DrawingContext drawingContext)
        {
            drawingContext.DrawRectangle(this.Background, null, new Rect(0, 0, this.ActualWidth, this.ActualHeight));
            drawingContext.PushTransform(new ScaleTransform(this.ZoomFactor, this.ZoomFactor));

            // draw background
            //drawingContext.DrawRectangle(this.Background, null, new Rect(0, 0, this.GroundWidth, this.GroundHeight));

            // draw tracks
            if (this.Tracks != null)
            {
                foreach (ItemBase track in this.Tracks)
                {
                    track.OnRender(drawingContext);
                }

                foreach (ItemBase track in this.Tracks)
                {
                    drawingContext.DrawDockPoints(track.DockPoints);
                }
                
            }

            //drawingContext.DrawPosition(new Point(400, 400));
            //drawingContext.DrawPosition(Points.RotateX(300, 400, 0, 400, 400));

            //for (int a = 0; a < 360; a++)
            //{
            //    drawingContext.DrawLine(blackPen,
            //        Points.RotateX(300, 400, a, 400, 400),
            //        Points.RotateX(300, 400, a + 1, 400, 400));
            //}
            drawingContext.Pop();
            base.OnRender(drawingContext);
        }

        private ItemBase FindTrack(Point point)
        {
            ItemBase track = null;
            if (this.Tracks != null)
            {
                track = this.Tracks.Where(t => t.IsInside(point)).FirstOrDefault();
            }
            return track;
        }

        //private RailTrack FindDocking(RailTrack track)
        //{
        //    if (this.Tracks != null)
        //    {
        //        List<DockPoint> dockPoints = track.DockPoints;
        //        var otherTracks = this.Tracks.Where(t => t != track).ToList();

        //        foreach (DockPoint dockPoint in dockPoints)
        //        {
        //            foreach (RailTrack t in otherTracks)
        //            {
        //                foreach (DockPoint dp in t.DockPoints)
        //                {
        //                    if (Math.Abs(dp.X - dockPoint.X) < dockDistance && Math.Abs(dp.Y - dockPoint.Y) < dockDistance)
        //                    {
        //                        track.Position += new Vector(dp.X - dockPoint.X, dp.Y - dockPoint.Y);
        //                        return t;
        //                    }
        //                }
        //            }

        //        }
        //    }
        //    return null;
        //}

        private ItemBase FindDocking(ItemBase track, IEnumerable<ItemBase> docked = null)
        {
            if (this.Tracks != null)
            {
                List<DockPoint> dockPoints = track.DockPoints;
                var otherTracks =
                    docked != null ?
                    this.Tracks.Where(t => t != track).Where(t => !docked.Contains(t)).ToList() :
                    this.Tracks.Where(t => t != track);

                foreach (DockPoint dockPoint in dockPoints)
                {
                    foreach (ItemBase t in otherTracks)
                    {
                        foreach (DockPoint dp in t.DockPoints)
                        {
                            //if (Math.Abs(dp.X - dockPoint.X) < dockDistance && Math.Abs(dp.Y - dockPoint.Y) < dockDistance)
                            if (dp.Distance(dockPoint) < dockDistance)
                            {
                                double rotate = 180.0 - dockPoint.Angle + dp.Angle;

                                var sub = FindSubgraph(track).Where(f => f != track).ToList();
                                foreach (ItemBase rt in sub)
                                {
                                    //rt.Angle += rotate;
                                    //rt.Position = track.Position.Rotate(rotate, dp);
                                }
                                track.Angle += rotate;
                                track.Position += new Vector(dp.X - dockPoint.X, dp.Y - dockPoint.Y);
                                return t;
                            }
                        }
                    }

                }
            }
            return null;
        }

        //private double Distance(Point a, Point b)
        //{
        //    return Math.Sqrt(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2));
        //}

        private void FindSubgraph(List<ItemBase> list, ItemBase startTrack)
        {
            if (this.Tracks != null)
            {
                List<DockPoint> dockPoints = startTrack.DockPoints;
                var otherTracks = this.Tracks.Where(t => t != startTrack).ToList();

                foreach (DockPoint dockPoint in dockPoints)
                {
                    foreach (ItemBase t in otherTracks)
                    {
                        if (!list.Contains(t))
                        {
                            foreach (DockPoint dp in t.DockPoints)
                            {
                                if (dp.Distance(dockPoint) < dockDistance)
                                {
                                    list.Add(t);
                                    FindSubgraph(list, t);
                                }
                            }
                        }
                    }
                }
            }
        }

        private List<ItemBase> FindSubgraph(ItemBase track)
        {
            List<ItemBase> tracks = new List<ItemBase>();
            FindSubgraph(tracks, track);
            //tracks.Add(track);
            return tracks;
        }

        private void MoveTrack(ItemBase track, Vector move, IEnumerable<ItemBase> subgraph = null)
        {
            track.Position += move;
            if (subgraph != null)
            {
                foreach (ItemBase tr in subgraph.Where(t => t != track))
                {
                    tr.Position += move;
                }
            }
            //FindDocking(track, subgraph);
        }

        private void RotateTrack(ItemBase track, double angle, IEnumerable<ItemBase> subgraph = null)
        {
            if (angle == 0.0)
            {
                return;
            }
            track.Angle += angle;
            if (subgraph != null)
            {
                foreach (ItemBase tr in subgraph.Where(t => t != track))
                {
                    tr.Angle += angle;
                    tr.Position = tr.Position.Rotate(angle, track.Position);
                }
            }
            //FindDocking(track, subgraph);
        }

        private RailAction actionType;
        private ItemBase actionTrack;
        private List<ItemBase> dockedTracks;
        private Point zoomedLastMousePosition;

        private double startRotationValue;
        private double lastRotationAngle;
        
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            Point zoomedMousePosition = e.GetPosition(this).Scale(1.0 / this.ZoomFactor);

            if ((this.actionTrack = FindTrack(zoomedMousePosition)) != null)
            {
                if (this.actionTrack.DockPoints.Any(d => d.Distance(zoomedMousePosition) < rotateDistance))
                {
                    this.actionType = RailAction.Rotate;
                    this.dockedTracks = FindSubgraph(this.actionTrack);
                    this.startRotationValue = e.GetPosition(this).Y;
                    this.lastRotationAngle = 0;                    
                }
                else
                {
                    if (Keyboard.Modifiers.HasFlag(ModifierKeys.Shift))
                    {
                        this.actionType = RailAction.MoveSingle;
                        this.dockedTracks = null;
                    }
                    else
                    {
                        this.actionType = RailAction.MoveGraph;
                        this.dockedTracks = FindSubgraph(this.actionTrack);
                    }
                }
                this.zoomedLastMousePosition = zoomedMousePosition;
                this.CaptureMouse();
            }
            base.OnMouseLeftButtonDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            
            if (this.actionTrack != null)
            {
                Trace.TraceInformation("OnMouseMove ({0}, {1})", e.GetPosition(this).X, e.GetPosition(this).Y);

                Point zoomedMousePosition = e.GetPosition(this).Scale(1.0 / this.ZoomFactor);
                double rotate = Math.Truncate((e.GetPosition(this).Y - this.startRotationValue) / 5.0) * 7.5; 

                switch (this.actionType)
                {
                case RailAction.MoveSingle:
                    MoveTrack(this.actionTrack, zoomedMousePosition - this.zoomedLastMousePosition);                    
                    break;
                case RailAction.MoveGraph:
                    MoveTrack(this.actionTrack, zoomedMousePosition - this.zoomedLastMousePosition, this.dockedTracks);
                    FindDocking(this.actionTrack, this.dockedTracks);
                    break;
                case RailAction.Rotate:
                    RotateTrack(this.actionTrack, rotate - this.lastRotationAngle, this.dockedTracks);
                    FindDocking(this.actionTrack, this.dockedTracks);
                    break;
                }
                this.zoomedLastMousePosition = zoomedMousePosition;
                this.lastRotationAngle = rotate;
                this.InvalidateVisual();
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        { 
            if (this.actionTrack != null)
            {
                Point mousePosition = e.GetPosition(this);
                // delete track/tracks if outside
                if (mousePosition.X < 0 || mousePosition.X >= this.ActualWidth || mousePosition.Y < 0 || mousePosition.Y >= this.ActualHeight)
                {
                    Trace.TraceInformation("OnMouseLeftButtonUp ({0}, {1}) ({2}, {3}) ", e.GetPosition(this).X, e.GetPosition(this).Y, this.ActualWidth, this.ActualHeight);
                    this.Tracks.Remove(this.actionTrack);
                    if (this.dockedTracks != null)
                    {
                        foreach (ItemBase track in this.dockedTracks)
                        {
                            this.Tracks.Remove(track);
                        }
                    }
                }
                else
                {
                    //Point zoomedMousePosition = e.GetPosition(this).Scale(1.0 / this.ZoomFactor);
                    //Vector move = zoomedMousePosition - this.zoomedLastMousePosition;

                    ////Vector m = mousePosition - this.actionMouseLastPosition;
                    //RailTrack dockingTrack;

                    //switch (this.actionType)
                    //{
                    //case RailAction.MoveSingle:
                    //    this.actionTrack.Position += move;
                    //    dockingTrack = FindDocking(this.actionTrack);
                    //    if (dockingTrack != null)
                    //    {
                    //        this.actionTrack.DockTo(dockingTrack);
                    //    }
                    //    break;
                    //case RailAction.MoveGraph:
                    //    this.actionTrack.Position += move;
                    //    dockingTrack = FindDocking(this.actionTrack);
                    //    if (dockingTrack != null)
                    //    {
                    //        this.actionTrack.DockTo(dockingTrack);
                    //    }
                    //    foreach (RailTrack track in this.dockedTracks)
                    //    {
                    //        track.Position += move;
                    //    }
                    //    break;
                    //case RailAction.Rotate:
                    //    //this.actionTrack.Angle = this.actionTrackStartAngle + ((int)(move.Y / 5)) * 7.5;
                    //    break;
                    //}
                }
                this.dockedTracks = null;
                this.actionTrack = null;
                this.InvalidateVisual();
                this.ReleaseMouseCapture();

                //this.actionMouseLastPosition = e.GetPosition(this);
            }
            base.OnMouseLeftButtonUp(e);
        }

        protected enum RailAction
        {
            MoveSingle,
            MoveGraph,
            Rotate
        }

        
    }
}
