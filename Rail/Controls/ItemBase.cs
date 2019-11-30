using Rail.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Rail.Controls
{
    public abstract class ItemBase<T> : ItemBase where T : TrackBase 
    {
        protected const double railWidth = 10;

        protected Geometry geometry;
        

        //protected RailMaterial railMaterial;
        protected T track;

        //public static ItemBase Create(RailMaterial railMaterial, double x = 0.0, double y = 0.0, double angle = 0.0)
        //{ 
        //    ItemBase track = null;
        //    switch(railMaterial.Type)
        //    {
        //    case RailType.Strait: track = new ItemStraight(railMaterial, x, y, angle); break;
        //    case RailType.Curved: track = new ItemCurved(railMaterial, x, y, angle); break;
        //    case RailType.LeftTurnout: track = new ItemLeftTurnout(railMaterial, x, y, angle); break;
        //    case RailType.RightTurnout: track = new ItemRightTurnout(railMaterial, x, y, angle); break;
        //    case RailType.LeftCurvedTurnout: track = new ItemLeftCurvedTurnout(railMaterial, x, y, angle); break;
        //    case RailType.RightCurvedTurnout: track = new ItemRightCurvedTurnout(railMaterial, x, y, angle); break;
        //    case RailType.Crossing: track = new ItemCrossing(railMaterial, x, y, angle); break;
        //    case RailType.DoubleSlipSwitch: track = new ItemCrossing(railMaterial, x, y, angle); break;
        //    case RailType.DoubleTurnout: track = new ItemDoubleTurnout(railMaterial, x, y, angle); break;
        //    case RailType.Bumper: track = new ItemBumper(railMaterial, x, y, angle); break;
        //    }
        //    return track;
        //}

        //public static ItemBase<T> Create(T track, double x = 0.0, double y = 0.0, double angle = 0.0)
        //{
        //    ItemBase<T> item = null;
        //    switch (track.GetType().Name)
        //    {
        //        case nameof(TrackStraight): item = new ItemStraight((TrackStraight)track, x, y, angle); break;
        //        //case RailType.Curved: item = new ItemCurved(railMaterial, x, y, angle); break;
        //        //case RailType.LeftTurnout: item = new ItemLeftTurnout(railMaterial, x, y, angle); break;
        //        //case RailType.RightTurnout: item = new ItemRightTurnout(railMaterial, x, y, angle); break;
        //        //case RailType.LeftCurvedTurnout: item = new ItemLeftCurvedTurnout(railMaterial, x, y, angle); break;
        //        //case RailType.RightCurvedTurnout: item = new ItemRightCurvedTurnout(railMaterial, x, y, angle); break;
        //        //case RailType.Crossing: item = new ItemCrossing(railMaterial, x, y, angle); break;
        //        //case RailType.DoubleSlipSwitch: item = new ItemCrossing(railMaterial, x, y, angle); break;
        //        //case RailType.DoubleTurnout: item = new ItemDoubleTurnout(railMaterial, x, y, angle); break;
        //        //case RailType.Bumper: item = new ItemBumper(railMaterial, x, y, angle); break;
        //    }
        //    return item;
        //}

        private ItemBase()
        { }

        //protected ItemBase(RailMaterial railMaterial, double x = 0.0, double y = 0.0, double angle = 0.0)
        //{
        //    this.railMaterial = railMaterial;
        //    this.Position = new Point(x, y);
        //    this.angle = angle;
        //    this.Text = new FormattedText(this.railMaterial.Id.ToString(), CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface("Verdana"), 16, Brushes.Black, 1.25);
        //    this.geometry = CreateGeometry();
        //}

        protected ItemBase(T track, double x = 0.0, double y = 0.0, double angle = 0.0)
        {
            this.track = track;
            this.Position = new Point(x, y);
            this.angle = angle;
            this.Text = new FormattedText(this.track.Id, CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface("Verdana"), 16, Brushes.Black, 1.25);
            this.geometry = CreateGeometry();
        }

        //public ItemBase<T> Clone()
        //{
        //    return ItemBase<T>.Create(this.track);
        //}

        public string Id
        {
            get { return this.track.Id; }
            set { this.track.Id = value; }
        }

        public FormattedText Text { get; private set; }

        public Rect Bounds
        {
            get
            {
                TransformGroup grp = new TransformGroup();
                grp.Children.Add(new TranslateTransform(this.Position.X, this.Position.Y));
                grp.Children.Add(new RotateTransform(this.angle, this.Position.X, this.Position.Y));
                this.geometry.Transform = grp;
                return this.geometry.Bounds;
            }
        }

        public override bool IsInside(Point point)
        {
            TransformGroup grp = new TransformGroup();
                grp.Children.Add(new TranslateTransform(this.Position.X, this.Position.Y));
                grp.Children.Add(new RotateTransform(this.angle, this.Position.X, this.Position.Y));
                this.geometry.Transform = grp;
            return this.geometry.FillContains(point);
        }

        

        protected Brush trackBrush = new SolidColorBrush(Colors.White);
        protected Pen linePen = new Pen(Brushes.Black, 1);
        protected Pen dockPen = new Pen(Brushes.Blue, 1);

        
        protected static double Sin(double value)
        {
            return Math.Sin(value * Math.PI / 180.0);
        }

        protected static double Cos(double value)
        {
            return Math.Cos(value * Math.PI / 180.0);
        }
                
        protected abstract Geometry CreateGeometry();

        public override void OnRender(DrawingContext drawingContext)
        {
            //drawingContext.DrawPosition(this.Position);
            //drawingContext.DrawDockRect(this.circleCenter);

            TransformGroup grp = new TransformGroup();
            grp.Children.Add(new TranslateTransform(this.Position.X, this.Position.Y));
            grp.Children.Add(new RotateTransform(this.angle, this.Position.X, this.Position.Y));
            this.geometry.Transform = grp;
            drawingContext.DrawGeometry(trackBrush, linePen, this.geometry);

            //if (this.DockPoints != null)
            //{
            //    foreach (DockPoint point in this.DockPoints)
            //    {
            //        drawingContext.DrawDockRect(point);
            //    }
            //}

            //drawingContext.DrawRectangle(null, linePen, this.Bounds);
        }
                
        //public void DockTo(RailTrack dockingTrack)
        //{
        //    foreach (DockPoint dockPoint in this.DockPoints)
        //    {
        //        foreach (DockPoint dp in dockingTrack.DockPoints)
        //        {
        //            if (Math.Abs(dp.X - dockPoint.X) < RailPlanControl.dockDistance / this.ZoomFactor 
        //             && Math.Abs(dp.Y - dockPoint.Y) < RailPlanControl.dockDistance / this.ZoomFactor)
        //            {

        //                this.Angle += dp.Angle - dockPoint.Angle + 180.0;
        //                this.Position += new Vector(dp.X - dockPoint.X, dp.Y - dockPoint.Y);
        //            }
        //        }
        //    }
        //}

        protected Geometry CreateStraitTrackGeometry(double length)
        {
            return new PathGeometry(new PathFigureCollection
            {
                new PathFigure(new Point(-length / 2.0, -railWidth), new PathSegmentCollection
                {
                    new LineSegment(new Point( length / 2.0, -railWidth), true),
                    new LineSegment(new Point( length / 2.0,  railWidth), true),
                    new LineSegment(new Point(-length / 2.0,  railWidth), true),
                    new LineSegment(new Point(-length / 2.0, -railWidth), true)
                }, true)
            });
        }

        protected Geometry CreateCurvedTrackGeometry(double angle, double radius)
        {
            Size innerSize = new Size(radius - railWidth, radius - railWidth);
            Size outerSize = new Size(radius + railWidth, radius + railWidth);

            Point circleCenter = Points.CircleCenter(angle / 2, radius);

            return new PathGeometry(new PathFigureCollection
            {
                new PathFigure(circleCenter + (Vector)Points.Circle(0.0, radius - railWidth), new PathSegmentCollection
                {
                    new LineSegment(circleCenter + (Vector)Points.Circle(0.0, radius + railWidth), true),
                    new ArcSegment (circleCenter + (Vector)Points.Circle(angle, radius + railWidth), outerSize, angle, false, SweepDirection.Clockwise, true),

                    new LineSegment(circleCenter + (Vector)Points.Circle(angle, radius - railWidth), true),
                    new ArcSegment (circleCenter + (Vector)Points.Circle(0.0, radius - railWidth), innerSize, angle, false, SweepDirection.Counterclockwise, true)
                }, true)
            });
        }

        protected Geometry CreateLeftTurnoutGeometry(double x, double y,double angle, double radius)
        {
            Size innerSize = new Size(radius - railWidth, radius - railWidth);
            Size outerSize = new Size(radius + railWidth, radius + railWidth);
            Point circleCenter = new Point(x, y - radius);

            return new PathGeometry(new PathFigureCollection
            {
                new PathFigure(new Point(x, y - railWidth), new PathSegmentCollection
                {
                    new LineSegment(new Point(x, y + railWidth), true),
                    new ArcSegment (new Point(x, y + railWidth).Rotate(-angle, circleCenter), innerSize, angle, false, SweepDirection.Counterclockwise, true),

                    new LineSegment(new Point(x, y - railWidth).Rotate(-angle, circleCenter), true),                 
                    new ArcSegment (new Point(x, y - railWidth), outerSize, angle, false, SweepDirection.Clockwise, true),
                }, true)
            });
        }

        protected Geometry CreateRightTurnoutGeometry(double x, double y, double angle, double radius)
        {
            Size innerSize = new Size(radius - railWidth, radius - railWidth);
            Size outerSize = new Size(radius + railWidth, radius + railWidth);
            Point circleCenter = new Point(x, y + radius);

            return new PathGeometry(new PathFigureCollection
            {
                new PathFigure(new Point(x, y - railWidth), new PathSegmentCollection
                {
                    new LineSegment(new Point(x, y + railWidth), true),
                    new ArcSegment (new Point(x, y + railWidth).Rotate(angle, circleCenter), innerSize, angle, false, SweepDirection.Clockwise, true),

                    new LineSegment(new Point(x, y - railWidth).Rotate(angle, circleCenter), true),  
                    new ArcSegment (new Point(x, y - railWidth), outerSize, angle, false, SweepDirection.Counterclockwise, true),
                }, true)
            });
        }

        
    }

    public abstract class ItemBase
    {
        protected Point position;
        protected double angle;

        public Point Position
        {
            get { return this.position; }
            set { this.position = value; Update(); }
        }

        public double Angle
        {
            get { return this.angle; }
            set { this.angle = value; Update(); }
        }

        public List<DockPoint> DockPoints { get; protected set; }

        protected virtual void Update()
        { }

        public abstract bool IsInside(Point point);

        public abstract void OnRender(DrawingContext drawingContext);

        public static ItemBase Create(TrackBase track, double x = 0.0, double y = 0.0, double angle = 0.0) 
        {
            ItemBase item = null;
            switch (track.GetType().Name)
            {
                case nameof(TrackStraight): item = new ItemStraight((TrackStraight)track, x, y, angle); break;
                case nameof(TrackCurved): item = new ItemCurved((TrackCurved)track, x, y, angle); break;
                case nameof(TrackLeftTurnout): item = new ItemLeftTurnout((TrackLeftTurnout)track, x, y, angle); break;
                case nameof(TrackRightTurnout): item = new ItemRightTurnout((TrackRightTurnout)track, x, y, angle); break;
                case nameof(TrackLeftCurvedTurnout): item = new ItemLeftCurvedTurnout((TrackLeftCurvedTurnout)track, x, y, angle); break;
                case nameof(TrackRightCurvedTurnout): item = new ItemRightCurvedTurnout((TrackRightCurvedTurnout)track, x, y, angle); break;
                case nameof(TrackCrossing): item = new ItemCrossing((TrackCrossing)track, x, y, angle); break;
                //case nameof(TrackDoubleSlipSwitch): item = new ItemCrossing((TrackDoubleSlipSwitch)track, x, y, angle); break;
                case nameof(TrackDoubleTurnout): item = new ItemDoubleTurnout((TrackDoubleTurnout)track, x, y, angle); break;
                case nameof(TrackBumper): item = new ItemBumper((TrackBumper)track, x, y, angle); break;
            }
            return item;
        }
    }
}
