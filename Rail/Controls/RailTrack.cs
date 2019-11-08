using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Controls
{
    public abstract class RailTrack
    {
        protected const double railWidth = 10;

        protected Geometry geometry;
        protected Point position;
        protected double angle;

        protected RailMaterial railMaterial;

        public static RailTrack Create(RailMaterial railMaterial, double x = 0.0, double y = 0.0, double angle = 0.0)
        { 
            RailTrack track = null;
            switch(railMaterial.Type)
            {
            case RailType.Strait: track = new RailStraitTrack(railMaterial, x, y, angle); break;
            case RailType.Curved: track = new RailCurvedTrack(railMaterial, x, y, angle); break;
            case RailType.LeftTurnout: track = new RailLeftTurnoutTrack(railMaterial, x, y, angle); break;
            case RailType.RightTurnout: track = new RailRightTurnoutTrack(railMaterial, x, y, angle); break;
            case RailType.LeftCurvedTurnout: track = new RailLeftCurvedTurnoutTrack(railMaterial, x, y, angle); break;
            case RailType.RightCurvedTurnout: track = new RailRightCurvedTurnoutTrack(railMaterial, x, y, angle); break;
            case RailType.Crossing: track = new RailCrossingTrack(railMaterial, x, y, angle); break;
            case RailType.DoubleSlipSwitch: track = new RailCrossingTrack(railMaterial, x, y, angle); break;
            case RailType.DoubleTurnout: track = new RailDoubleTurnoutTrack(railMaterial, x, y, angle); break;
            case RailType.Bumper: track = new RailBumperTrack(railMaterial, x, y, angle); break;
            }
            return track;
        }
        
        private RailTrack()
        { }

        protected RailTrack(RailMaterial railMaterial, double x = 0.0, double y = 0.0, double angle = 0.0)
        {
            this.railMaterial = railMaterial;
            this.Position = new Point(x, y);
            this.angle = angle;
            this.Text = new FormattedText(this.railMaterial.Id.ToString(), CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface("Verdana"), 16, Brushes.Black);
            this.geometry = CreateGeometry();
        }

        public RailTrack Clone()
        {
            return RailTrack.Create(this.railMaterial);
        }

        public int Id
        {
            get { return this.railMaterial.Id; }
            set { this.railMaterial.Id = value; }
        }

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

        [XmlIgnore]
        public FormattedText Text { get; private set; }

        [XmlIgnore]
        public List<DockPoint> DockPoints { get; protected set; }

        [XmlIgnore]
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

        public bool IsInside(Point point)
        {
            TransformGroup grp = new TransformGroup();
                grp.Children.Add(new TranslateTransform(this.Position.X, this.Position.Y));
                grp.Children.Add(new RotateTransform(this.angle, this.Position.X, this.Position.Y));
                this.geometry.Transform = grp;
            return this.geometry.FillContains(point);
        }

        protected virtual void Update()
        {
            //switch (Type = RailType.Curved, Radius = 360.0, Angle = 30.0)
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

        public virtual void OnRender(DrawingContext drawingContext)
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
        //            if (Math.Abs(dp.X - dockPoint.X) < RailPlan.dockDistance / this.ZoomFactor 
        //             && Math.Abs(dp.Y - dockPoint.Y) < RailPlan.dockDistance / this.ZoomFactor)
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
}
