//using System.Drawing;
using System.Windows.Media;
using System.Windows;
using System.Xml.Serialization;
using System.Globalization;
using Rail.Controls;
using System;
using System.Collections.Generic;
using Rail.Misc;

namespace Rail.Model
{
    public abstract class TrackBase
    {

        protected readonly Brush trackBrush = TrackBrushes.TrackBackground;
        protected readonly Brush trackBrushSelected = TrackBrushes.TrackSelectedBackground;
        protected readonly Brush textBrush = TrackBrushes.Text;
        protected readonly Brush ballastBrush = TrackBrushes.Ballast;
        protected readonly Pen dockPen = new Pen(TrackBrushes.Dock, 2);
        protected readonly Pen linePen = new Pen(TrackBrushes.TrackFrame, 2);
        protected readonly Pen textPen = new Pen(TrackBrushes.Text, 0.5);
        protected Pen railPen;
        protected Pen woodenSleepersPen;
        protected Pen concreteSleepersPen;
        protected Pen railPenSelected;
        protected Pen selectedSleepersPen;
        protected FormattedText text;
        protected Drawing textDrawing;
        protected string dockType;

        protected Drawing drawingTracks;
        protected Drawing drawingTracksSelected;
        protected Drawing drawingRail;
        protected Drawing drawingRailSelected;
        protected Drawing drawingTerrain;

        protected double railWidth;
        protected double sleepersWidth;
        protected double ballastWidth;

        [XmlAttribute("Id")]
        public string Id { get; set; }

        [XmlAttribute("Article")]
        public string Article { get; set; }

        [XmlAttribute("ViewType")]
        public TrackViewType ViewType { get; set; }

        [XmlIgnore]
        public string Manufacturer { get; protected set; }

        [XmlIgnore]
        public abstract string Name { get; }

        [XmlIgnore]
        public abstract string Description { get; }

        [XmlIgnore]
        public double RailSpacing { get; protected set; }
        
        [XmlIgnore]
        public Geometry GeometryTracks { get; protected set; }

        [XmlIgnore]
        public Geometry GeometryRail { get; protected set; }

        [XmlIgnore]
        public List<TrackDockPoint> DockPoints { get; protected set; }

        [XmlIgnore]
        public virtual List<TrackMaterial> Materials 
        {
            get
            {
                return new List<TrackMaterial> { new TrackMaterial { Id = this.Id, Number = 1, Manufacturer = this.Manufacturer, Article = this.Article, Name = this.Name } };
            }
        }

        private readonly double ballastWidthFactor = 5.0 / 3.0;
        private readonly double sleepersWidthFactor = 4.0 / 3.0;
        private readonly double railThicknessFactor = 1.0 / 10.0;
        private readonly double sleepersThicknessFactor = 1.0 / 4.0;
        private readonly double textFactor = 0.9;
        
        public void Update(TrackType trackType)
        {
            this.RailSpacing = trackType.Spacing;
            // override if not set
            this.ViewType = this.ViewType == TrackViewType.None ? trackType.ViewType : this.ViewType;
            this.dockType = trackType.DockType;
            this.Manufacturer = trackType.Manufacturer;

            this.railWidth = this.RailSpacing;
            this.ballastWidth = this.RailSpacing * ballastWidthFactor;
            this.sleepersWidth = this.ViewType.HasFlag(TrackViewType.Ballast) ? this.RailSpacing * sleepersWidthFactor : this.RailSpacing * ballastWidthFactor;

            this.railPen = new Pen(TrackBrushes.Rail, this.RailSpacing * railThicknessFactor);
            this.railPenSelected = new Pen(TrackBrushes.SelectedRail, this.RailSpacing * railThicknessFactor);
            this.woodenSleepersPen = new Pen(TrackBrushes.WoodenSleepers, this.RailSpacing * sleepersThicknessFactor);
            this.concreteSleepersPen = new Pen(TrackBrushes.ConcreteSleepers, this.RailSpacing * sleepersThicknessFactor);
            this.selectedSleepersPen = new Pen(TrackBrushes.SelectedSleepers, this.RailSpacing * sleepersThicknessFactor);
            this.text = new FormattedText(this.Article, CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface("Verdana"), this.RailSpacing * textFactor, TrackBrushes.Text, 1.25);
            this.textDrawing = new GeometryDrawing(textBrush, textPen, text.BuildGeometry(new Point(0, 0) - new Vector(text.Width / 2, text.Height / 2)));
            Create();
        }

        //public void Update(TrackType trackType)
        //{
        //    this.RailSpacing = trackType.Spacing;
        //    this.ViewType = trackType.ViewType;
        //    this.dockType = trackType.DockType;
        //    this.Manufacturer = trackType.Manufacturer;

        //    //this.railWidth = this.RailSpacing;
        //    //this.ballastWidth = this.RailSpacing * 5 / 3;
        //    //this.sleepersWidth = this.ViewType.HasFlag(TrackViewType.Ballast) ? this.RailSpacing * 4 / 3 : this.RailSpacing * 5 / 3;

        //    this.railPen = new Pen(Brushes.Black, this.RailSpacing / 10);
        //    this.railPenSelected = new Pen(Brushes.Blue, this.RailSpacing / 10);
        //    this.woodenSleepersPen = new Pen(Brushes.Black, this.RailSpacing / 4);
        //    this.concreteSleepersPen = new Pen(Brushes.LightGray, this.RailSpacing / 4);
        //    this.sleepersPenSelected = new Pen(Brushes.Blue, this.RailSpacing / 4);
        //    this.text = new FormattedText(this.Article, CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface("Verdana"), this.RailSpacing * 0.9, Brushes.Black, 1.25);
        //    this.textDrawing = new GeometryDrawing(textBrush, textPen, text.BuildGeometry(new Point(0, 0) - new Vector(text.Width / 2, text.Height / 2)));
        //    Create();
        //}

        protected virtual void Create()
        {
            // Tracks
            this.GeometryTracks = CreateGeometry(this.RailSpacing);

            DrawingGroup drawingTracks = new DrawingGroup();
            drawingTracks.Children.Add(new GeometryDrawing(trackBrush, this.linePen, this.GeometryTracks));
            drawingTracks.Children.Add(this.textDrawing);
            this.drawingTracks = drawingTracks;

            DrawingGroup drawingTracksSelected = new DrawingGroup();
            drawingTracksSelected.Children.Add(new GeometryDrawing(trackBrushSelected, this.linePen, this.GeometryTracks));
            drawingTracksSelected.Children.Add(this.textDrawing);
            this.drawingTracksSelected = drawingTracksSelected;

            // Rail
            this.GeometryRail = CreateGeometry(this.sleepersWidth);
            this.drawingRail = CreateRailDrawing(false);
            this.drawingRailSelected = CreateRailDrawing(true);

            // Terrain
            this.drawingTerrain = drawingRail;

            // dock points
            this.DockPoints = CreateDockPoints();
        }

        protected abstract Geometry CreateGeometry(double spacing);
        protected abstract Drawing CreateRailDrawing(bool isSelected);
        protected abstract List<TrackDockPoint> CreateDockPoints();

        // for TrackControl
        public virtual void Render(DrawingContext drawingContext, RailViewMode viewMode, bool isSelected)
        {
            switch (viewMode)
            {
            case RailViewMode.Tracks:
                drawingContext.DrawDrawing(isSelected ? this.drawingTracksSelected : this.drawingTracks);
                break;
            case RailViewMode.Rail:
                drawingContext.DrawDrawing(isSelected ? this.drawingRailSelected : this.drawingRail);
                break;
            case RailViewMode.Terrain:
                drawingContext.DrawDrawing(this.drawingTerrain);
                break;
            }
        }

        

        public enum StraitOrientation
        {
            Left,
            Center,
            Right
        }

        [Flags]
        public enum CurvedOrientation
        {
            Left = 0x01,
            Center = 0x02,
            Right = 0x03,
            Direction = 0x0f,
            Clockwise = 0x00,
            Counterclockwise = 0x10
        }

        protected Pen GetSleepersPen(bool isSelected)
        {
            if (isSelected)
            {
                return selectedSleepersPen;
            }
            else
            {
                switch (this.ViewType & TrackViewType.Sleepers)
                {
                case TrackViewType.WoodenSleepers:
                    return this.woodenSleepersPen;
                case TrackViewType.ConcreteSleepers:
                    return this.concreteSleepersPen;
                default:
                    return null;
                }
            }
        }

        protected Geometry StraitGeometry(double length, StraitOrientation orientation, double width, double direction = 0, Point? pos = null)
        {
            Rectangle rec = new Rectangle(orientation, length, width).Rotate(direction).Move(pos);
            return new PathGeometry(new PathFigureCollection
            {
                new PathFigure(rec.LeftTop, new PathSegmentCollection
                {
                    new LineSegment(rec.LeftBottom, true),
                    new LineSegment(rec.RightBottom, true),
                    new LineSegment(rec.RightTop, true),
                    new LineSegment(rec.LeftTop, true),
                }, true)
            });
        }

        protected Drawing StraitBallast(double length, StraitOrientation orientation = StraitOrientation.Center, double direction = 0, Point? pos = null)
        {
            Rectangle rec = new Rectangle(orientation, length, this.ballastWidth).Rotate(direction).Move(pos);
            return new GeometryDrawing(ballastBrush, null, new PathGeometry(new PathFigureCollection
            {
                new PathFigure(rec.LeftTop, new PathSegmentCollection
                {
                    new LineSegment(rec.LeftBottom, true),
                    new LineSegment(rec.RightBottom, true),
                    new LineSegment(rec.RightTop, true),
                    new LineSegment(rec.LeftTop, true),
                }, true)
            }));
        }

        protected Drawing StraitSleepers(bool isSelected, double length, StraitOrientation orientation = StraitOrientation.Center, double direction = 0, Point? pos = null)
        {
            Pen sleepersPen = GetSleepersPen(isSelected);
            
            double x = 0;
            switch (orientation)
            {
            case StraitOrientation.Left: x = 0; break;
            case StraitOrientation.Center: x = -length / 2; break;
            case StraitOrientation.Right: x = -length; break;
            }


            int num = (int)Math.Round(length / (this.RailSpacing / 2));
            double sleepersDistance = length / num;

            var railDrawing = new DrawingGroup();

            for (int i = 0; i < num; i++)
            {
                double sx = x + sleepersDistance / 2 + sleepersDistance * i;
                double sy = this.sleepersWidth / 2;
                railDrawing.Children.Add(new GeometryDrawing(null, sleepersPen, new LineGeometry(
                    new Point(sx, -sy).Rotate(direction).Move(pos),
                    new Point(sx, +sy).Rotate(direction).Move(pos))));
            }

            return railDrawing;
        }
        
        protected Drawing StraitRail(bool isSelected, double length, StraitOrientation orientation = StraitOrientation.Center, double direction = 0, Point? pos = null)
        {
            double x = 0;
            switch (orientation)
            {
            case StraitOrientation.Left: x = 0; break;
            case StraitOrientation.Center: x = -length / 2; break;
            case StraitOrientation.Right: x = -length; break;
            }

            var railDrawing = new DrawingGroup();            

            railDrawing.Children.Add(new GeometryDrawing(null, isSelected ? railPenSelected : railPen, new LineGeometry(new Point(x, -this.railWidth / 2).Rotate(direction).Move(pos), new Point(x + length, -this.railWidth / 2).Rotate(direction).Move(pos))));
            railDrawing.Children.Add(new GeometryDrawing(null, isSelected ? railPenSelected : railPen, new LineGeometry(new Point(x, +this.railWidth / 2).Rotate(direction).Move(pos), new Point(x + length, +this.railWidth / 2).Rotate(direction).Move(pos))));
            
            return railDrawing;
        }

        protected Geometry CurvedGeometry(double angle, double radius, CurvedOrientation orientation, double width, Point pos)
        {
            double outerTrackRadius = radius + width / 2;
            double innerTrackRadius = radius - width / 2;
            Size outerTrackSize = new Size(outerTrackRadius, outerTrackRadius);
            Size innerTrackSize = new Size(innerTrackRadius, innerTrackRadius);

            Point circleCenter = pos + (orientation.HasFlag(CurvedOrientation.Counterclockwise) ? new Vector(0, -radius) : new Vector(0, +radius));
            double startAngle = orientation.HasFlag(CurvedOrientation.Counterclockwise) ? 180 : 0;

            switch (orientation & CurvedOrientation.Direction)
            {
            case CurvedOrientation.Left: startAngle -= 0; break;
            case CurvedOrientation.Center: startAngle -= angle / 2; break;
            case CurvedOrientation.Right: startAngle -= angle; break;
            }

            return new PathGeometry(new PathFigureCollection
            {
                new PathFigure(circleCenter - PointExtentions.Circle(startAngle, innerTrackRadius), new PathSegmentCollection
                {
                    new LineSegment(circleCenter - PointExtentions.Circle(startAngle, outerTrackRadius), true),
                    new ArcSegment (circleCenter - PointExtentions.Circle(startAngle + angle, outerTrackRadius), outerTrackSize, angle, false, SweepDirection.Counterclockwise, true),

                    new LineSegment(circleCenter - PointExtentions.Circle(startAngle + angle, innerTrackRadius), true),
                    new ArcSegment (circleCenter - PointExtentions.Circle(startAngle,  innerTrackRadius), innerTrackSize, angle, false, SweepDirection.Clockwise, true)
                }, true)
            });
        }

        protected Drawing CurvedBallast(double angle, double radius, CurvedOrientation orientation, Point pos)
        {
            double outerSleepersRadius = radius + this.ballastWidth / 2;
            double innerSleepersRadius = radius - this.ballastWidth / 2;

            Size outerSleepersSize = new Size(outerSleepersRadius, outerSleepersRadius);
            Size innerSleepersSize = new Size(innerSleepersRadius, innerSleepersRadius);

            Point circleCenter = pos + (orientation.HasFlag(CurvedOrientation.Counterclockwise) ? new Vector(0, -radius) : new Vector(0, +radius));
            double startAngle = orientation.HasFlag(CurvedOrientation.Counterclockwise) ? 180 : 0;

            switch (orientation & CurvedOrientation.Direction)
            {
            case CurvedOrientation.Left: startAngle -= 0; break;
            case CurvedOrientation.Center: startAngle -= angle / 2; break;
            case CurvedOrientation.Right: startAngle -= angle; break;
            }

            return new GeometryDrawing(ballastBrush, null, new PathGeometry(new PathFigureCollection
            {
                new PathFigure(circleCenter - PointExtentions.Circle(startAngle, innerSleepersRadius), new PathSegmentCollection
                {
                    new LineSegment(circleCenter - PointExtentions.Circle(startAngle, outerSleepersRadius), true),
                    new ArcSegment (circleCenter - PointExtentions.Circle(startAngle + angle, outerSleepersRadius), outerSleepersSize, angle, false, SweepDirection.Counterclockwise, true),

                    new LineSegment(circleCenter - PointExtentions.Circle(startAngle + angle, innerSleepersRadius), true),
                    new ArcSegment (circleCenter - PointExtentions.Circle(startAngle,  innerSleepersRadius), innerSleepersSize, angle, false, SweepDirection.Clockwise, true)
                }, true)
            }));
        }

        protected Drawing CurvedSleepers(bool isSelected, double angle, double radius, CurvedOrientation orientation, Point pos)
        {
            Pen sleepersPen = GetSleepersPen(isSelected);

            //double outerTrackRadius = radius + this.railWidth / 2;
            //double innerTrackRadius = radius - this.railWidth / 2;
            double outerSleepersRadius = radius + this.sleepersWidth / 2;
            double innerSleepersRadius = radius - this.sleepersWidth / 2;

            //Size outerTrackSize = new Size(outerTrackRadius, outerTrackRadius);
            //Size innerTrackSize = new Size(innerTrackRadius, innerTrackRadius);

            Point circleCenter = pos + (orientation.HasFlag(CurvedOrientation.Counterclockwise) ? new Vector(0, -radius) : new Vector(0, +radius));
            double startAngle = orientation.HasFlag(CurvedOrientation.Counterclockwise) ? 180 : 0;

            switch (orientation & CurvedOrientation.Direction)
            {
            case CurvedOrientation.Left: startAngle -= 0; break;
            case CurvedOrientation.Center: startAngle -= angle / 2; break;
            case CurvedOrientation.Right: startAngle -= angle; break;
            }

            double lenth = radius * 2 * Math.PI * angle / 360.0;
            int num = (int)Math.Round(lenth / (this.RailSpacing / 2));
            double sleepersDistance = angle / num;

            var railDrawing = new DrawingGroup();

            for (int i = 0; i < num; i++)
            {
                double ang = startAngle + (sleepersDistance / 2) + sleepersDistance * i;
                railDrawing.Children.Add(new GeometryDrawing(null, sleepersPen, new LineGeometry(
                    circleCenter - PointExtentions.Circle(ang, innerSleepersRadius),
                    circleCenter - PointExtentions.Circle(ang, outerSleepersRadius))));
            }

            return railDrawing;
        }

        protected Drawing CurvedRail(bool isSelected, double angle, double radius, CurvedOrientation orientation, Point pos)
        {
            double outerTrackRadius = radius + this.railWidth / 2;
            double innerTrackRadius = radius - this.railWidth / 2;
            //double outerSleepersRadius = radius + this.sleepersSpacing / 2;
            //double innerSleepersRadius = radius - this.sleepersSpacing / 2;

            //Size outerTrackSize = new Size(outerTrackRadius, outerTrackRadius);
            Size innerTrackSize = new Size(innerTrackRadius, innerTrackRadius);

            Point circleCenter = pos + (orientation.HasFlag(CurvedOrientation.Counterclockwise) ? new Vector(0, -radius) : new Vector(0, +radius));
            double startAngle = orientation.HasFlag(CurvedOrientation.Counterclockwise) ? 180 : 0;

            switch (orientation & CurvedOrientation.Direction)
            {
            case CurvedOrientation.Left: startAngle -= 0; break;
            case CurvedOrientation.Center: startAngle -= angle / 2; break;
            case CurvedOrientation.Right: startAngle -= angle; break;
            }

            //double lenth = radius * 2 * Math.PI * angle / 360.0;
            //int num = (int)Math.Round(lenth / (this.RailSpacing / 2));
            //double sleepersDistance = angle / num;

            var railDrawing = new DrawingGroup();
            
            railDrawing.Children.Add(new GeometryDrawing(null, isSelected ? railPenSelected : railPen, new PathGeometry(new PathFigureCollection
            {
                new PathFigure(circleCenter - PointExtentions.Circle(startAngle, innerTrackRadius), new PathSegmentCollection
                {
                    new ArcSegment (circleCenter - PointExtentions.Circle(startAngle + angle, innerTrackRadius), innerTrackSize, angle, false, SweepDirection.Counterclockwise, true)
                }, false)
            })));
            railDrawing.Children.Add(new GeometryDrawing(null, isSelected ? railPenSelected : railPen, new PathGeometry(new PathFigureCollection
            {
                new PathFigure(circleCenter - PointExtentions.Circle(startAngle, outerTrackRadius), new PathSegmentCollection
                {
                    new ArcSegment (circleCenter - PointExtentions.Circle(startAngle + angle, outerTrackRadius), innerTrackSize, angle, false, SweepDirection.Counterclockwise, true)
                }, false)
            })));
            return railDrawing;
        }

        //public Point CurveCenter(double angle, double radius, CurvedOrientation orientation)
        //{
        //    double startAngle = 0; // orientation.HasFlag(CurvedOrientation.Counterclockwise) ? 180 : 0;
        //    switch (orientation & CurvedOrientation.Direction)
        //    {
        //    case CurvedOrientation.Left: startAngle -= 0; break;
        //    case CurvedOrientation.Center: startAngle -= angle / 2; break;
        //    case CurvedOrientation.Right: startAngle -= angle; break;
        //    }

        //    Vector a = PointExtentions.Circle(startAngle, radius);
        //    Vector b = PointExtentions.Circle(startAngle + angle, radius);
        //    return (Point)((a - b) / -2);
        //}
    }
}
