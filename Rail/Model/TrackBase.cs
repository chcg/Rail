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

        protected Brush trackBrush = new SolidColorBrush(Colors.White);
        protected Brush trackBrushSelected = new SolidColorBrush(Colors.Yellow);
        protected Brush textBrush = new SolidColorBrush(Colors.Black);
        protected Brush ballastBrush = new SolidColorBrush(Color.FromRgb(0x51, 0x56, 0x5c));
        protected Pen dockPen = new Pen(Brushes.Blue, 2);
        protected Pen linePen = new Pen(Brushes.Black, 2);
        //protected Pen selectedLinePen = new Pen(Brushes.Blue, 2);
        protected Pen textPen = new Pen(Brushes.Black, 0.5);
        protected Pen railPen;
        protected Pen railPenSelected;
        protected Pen sleepersPen;
        protected Pen sleepersPenSelected;
        protected FormattedText text;
        protected Drawing textDrawing;
        //protected double sleepersOutstanding;
        protected string dockType;

        protected Drawing drawingTracks;
        protected Drawing drawingTracksSelected;
        protected Drawing drawingRail;
        protected Drawing drawingRailSelected;
        protected Drawing drawingTerrain;


        [XmlAttribute("Id")]
        public string Id { get; set; }

        [XmlAttribute("Article")]
        public string Article { get; set; }

        [XmlIgnore]
        public abstract string Name { get; }
        
        [XmlIgnore]
        public double RailSpacing { get; protected set; }

        protected double sleepersSpacing;

        [XmlIgnore]
        public bool Ballast { get; protected set; }

        [XmlIgnore]
        public Geometry GeometryTracks { get; protected set; }

        [XmlIgnore]
        public Geometry GeometryRail { get; protected set; }

        [XmlIgnore]
        public List<TrackDockPoint> DockPoints { get; protected set; }

        public void Update(TrackType trackType)
        {
            this.RailSpacing = trackType.Spacing;
            this.sleepersSpacing = this.RailSpacing * 5 / 3;
            this.Ballast = trackType.Ballast;
            this.dockType = trackType.DockType;

            this.railPen = new Pen(Brushes.Black, this.RailSpacing / 10);
            this.railPenSelected = new Pen(Brushes.Blue, this.RailSpacing / 10);
            this.sleepersPen = new Pen(Brushes.Black, this.RailSpacing / 4);
            this.sleepersPenSelected = new Pen(Brushes.Blue, this.RailSpacing / 4);
            this.text = new FormattedText(this.Article, CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface("Verdana"), this.RailSpacing * 0.9, Brushes.Black, 1.25);
            this.textDrawing = new GeometryDrawing(textBrush, textPen, text.BuildGeometry(new Point(0, 0) - new Vector(text.Width / 2, text.Height / 2)));
            Create();
        }
                
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
            this.GeometryRail = CreateGeometry(this.sleepersSpacing);
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
               
        protected Geometry StraitGeometry(double length, StraitOrientation orientation, double spacing, double direction = 0, Point? pos = null)
        {
            Rectangle rec = new Rectangle(orientation, length, this.RailSpacing).Rotate(direction).Move(pos);
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
            Rectangle rec = new Rectangle(orientation, length, this.sleepersSpacing).Rotate(direction).Move(pos);
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
            railDrawing.Children.Add(new GeometryDrawing(null, isSelected ? railPenSelected : railPen, new LineGeometry(new Point(x, -this.RailSpacing / 2).Rotate(direction).Move(pos), new Point(x + length, -this.RailSpacing / 2).Rotate(direction).Move(pos))));
            railDrawing.Children.Add(new GeometryDrawing(null, isSelected ? railPenSelected : railPen, new LineGeometry(new Point(x, +this.RailSpacing / 2).Rotate(direction).Move(pos), new Point(x + length, +this.RailSpacing / 2).Rotate(direction).Move(pos))));

            int num = (int)Math.Round(length / (this.RailSpacing / 2));
            double sleepersDistance = length / num;

            for (int i = 0; i < num; i++)
            {
                double sx = x + sleepersDistance / 2 + sleepersDistance * i;
                double sy = this.sleepersSpacing / 2;
                railDrawing.Children.Add(new GeometryDrawing(null, isSelected ? sleepersPenSelected : sleepersPen, new LineGeometry(
                    new Point(sx, -sy).Rotate(direction).Move(pos),
                    new Point(sx, +sy).Rotate(direction).Move(pos))));
            }
            return railDrawing;
        }

        protected Geometry CurvedGeometry(double angle, double radius, CurvedOrientation orientation, double spacing, Point pos)
        {
            double outerTrackRadius = radius + spacing / 2;
            double innerTrackRadius = radius - spacing / 2;
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
            double outerSleepersRadius = radius + this.sleepersSpacing / 2;
            double innerSleepersRadius = radius - this.sleepersSpacing / 2;

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

        protected Drawing CurvedRail(bool isSelected, double angle, double radius, CurvedOrientation orientation, Point pos)
        {
            double outerTrackRadius = radius + this.RailSpacing / 2;
            double innerTrackRadius = radius - this.RailSpacing / 2;
            double outerSleepersRadius = radius + this.sleepersSpacing / 2;
            double innerSleepersRadius = radius - this.sleepersSpacing / 2;

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

            double lenth = radius * 2 * Math.PI * angle / 360.0;
            int num = (int)Math.Round(lenth / (this.RailSpacing / 2));
            double sleepersDistance = angle / num;

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
            for (int i = 0; i < num; i++)
            {
                double ang = startAngle + (sleepersDistance / 2) + sleepersDistance * i;
                railDrawing.Children.Add(new GeometryDrawing(null, isSelected ? sleepersPenSelected : sleepersPen, new LineGeometry(
                    circleCenter - PointExtentions.Circle(ang, innerSleepersRadius),
                    circleCenter - PointExtentions.Circle(ang, outerSleepersRadius)
                    )));
            }
            return railDrawing;
        }

        public Point CurveCenter(double angle, double radius, CurvedOrientation orientation)
        {
            double startAngle = 0; // orientation.HasFlag(CurvedOrientation.Counterclockwise) ? 180 : 0;
            switch (orientation & CurvedOrientation.Direction)
            {
            case CurvedOrientation.Left: startAngle -= 0; break;
            case CurvedOrientation.Center: startAngle -= angle / 2; break;
            case CurvedOrientation.Right: startAngle -= angle; break;
            }

            Vector a = PointExtentions.Circle(startAngle, radius);
            Vector b = PointExtentions.Circle(startAngle + angle, radius);
            return (Point)((a - b) / -2);
        }
    }
}
