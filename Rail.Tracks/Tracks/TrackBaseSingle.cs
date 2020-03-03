using Rail.Tracks.Trigonometry;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Tracks
{
    public abstract class TrackBaseSingle : TrackBase
    {
        protected readonly double combineLengthOffset = 0.5;
        protected readonly double combineAngleOffset = 0.05;
        protected readonly Pen dockPen = new Pen(TrackBrushes.Dock, 2);
        protected readonly Pen linePen = new Pen(TrackBrushes.TrackFrame, 2);
        protected readonly Pen dotPen = new Pen(Brushes.White, 2) { DashStyle = DashStyles.Dot };
        protected readonly Pen textPen = new Pen(TrackBrushes.Text, 0.5);
        
        protected FormattedText text;
        protected Drawing textDrawing;
        protected string dockType;
        protected Drawing drawingRail;

        // rail
        protected TrackRailType railType;
        protected double railWidth;
        protected Pen railPen;

        // sleeper
        protected TrackSleeperType sleeperType;
        protected double sleeperWidth;
        protected Pen sleeperPen;

        // ballast
        protected TrackBallastType ballastType;
        protected double ballastWidth;
        protected Brush ballastBrush;


        [XmlAttribute("Article")]
        public string Article { get; set; }

        /// <summary>
        /// additional articles in package
        /// </summary>
        /// <remarks>komma separated article list</remarks>
        [XmlAttribute("AddArticles")]
        public string AddArticles { get; set; }        
                
        [XmlIgnore, JsonIgnore]
        public string Manufacturer { get; protected set; }

        [XmlIgnore, JsonIgnore]
        protected bool HasBallast { get { return this.ballastType >= TrackBallastType.Light; } }

        /// <summary>
        /// Get a list with the materials and order numbers of the track.
        /// </summary>
        /// <remarks>
        /// A track can have more than one material / order number for example the track spoke connections for a turntable.  
        /// </remarks>
        [XmlIgnore, JsonIgnore]
        public override List<TrackMaterial> Materials 
        {
            get
            {
                return new List<TrackMaterial> { new TrackMaterial { Id = this.Id, Number = 1, Manufacturer = this.Manufacturer, Article = this.Article, Name = this.Name } };
            }
        }

        //private readonly double ballastWidthFactor = 5.0 / 3.0;
        //private readonly double sleepersWidthFactor = 4.0 / 3.0;
        private readonly double railThicknessFactor = 1.0 / 10.0;
        private readonly double sleepersThicknessFactor = 1.0 / 4.0;
        private readonly double textFactor = 0.9;
        
        public override void Update(TrackType trackType)
        {
            TrackParameter parameter = trackType.Parameter;

            this.Manufacturer = parameter.Manufacturer;
            this.dockType = parameter.DockType;

            // rail
            this.railType = parameter.RailType;
            this.railWidth = this.RailWidth = parameter.RailWidth;
            this.railPen = new Pen(this.railType switch
            {
                TrackRailType.Silver => TrackBrushes.SilverRail,
                TrackRailType.Brass => TrackBrushes.BrassRail,
                TrackRailType.Black => TrackBrushes.BlackRail,
                _ => null
            }, this.RailWidth * railThicknessFactor);

            // sleeper
            this.sleeperType = parameter.SleeperType;
            this.sleeperWidth = parameter.SleeperWidth;
            this.sleeperPen = new Pen(this.sleeperType switch
            {
                TrackSleeperType.WoodenSleepers => TrackBrushes.WoodenSleepers,
                TrackSleeperType.ConcreteSleepers => TrackBrushes.ConcreteSleepers,
                _ => null
            }, this.RailWidth * sleepersThicknessFactor);

            // ballast
            this.ballastType = parameter.BallastType;
            this.ballastWidth = parameter.BallastWidth;
            this.ballastBrush = this.ballastType switch
            {
                TrackBallastType.Light => TrackBrushes.LightBallast,
                TrackBallastType.Medium => TrackBrushes.MediumBallast,
                TrackBallastType.Dark => TrackBrushes.DarkBallast,
                _ => null
            };
            
            this.TrackWidth = Math.Max(parameter.BallastWidth, parameter.SleeperWidth);

            this.text = new FormattedText(this.Article, CultureInfo.CurrentUICulture, FlowDirection.LeftToRight, new Typeface("Verdana"), this.RailWidth * textFactor, TrackBrushes.Text, 1.25);
            this.textDrawing = new GeometryDrawing(TrackBrushes.Text, textPen, text.BuildGeometry(new Point(0, 0) - new Vector(text.Width / 2, text.Height / 2)));
            Create();
        }

        protected virtual void Create()
        {
            this.TrackGeometry = CreateGeometry();
            this.drawingRail = CreateRailDrawing();
            this.DockPoints = CreateDockPoints();
        }

        /// <summary>
        /// Create geometrie with different spacings
        /// </summary>
        /// <param name="spacing">Spacing of the geometrie.</param>
        /// <returns>The geometrie</returns>
        protected abstract Geometry CreateGeometry();
        protected abstract Drawing CreateRailDrawing();
        protected abstract List<TrackDockPoint> CreateDockPoints();

        // for TrackControl
        public override void RenderTrack(DrawingContext drawingContext, Brush trackBrush)
        {
            drawingContext.DrawDrawing(new GeometryDrawing(trackBrush, this.linePen, this.TrackGeometry));
            drawingContext.DrawDrawing(this.textDrawing);
        }

        public override void RenderRail(DrawingContext drawingContext)
        {
            drawingContext.DrawDrawing(this.drawingRail);
        }

        public override void RenderSelection(DrawingContext drawingContext)
        {
            drawingContext.DrawDrawing(new GeometryDrawing(null, this.linePen, this.TrackGeometry));
            drawingContext.DrawDrawing(new GeometryDrawing(null, this.dotPen, this.TrackGeometry));
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

        protected Geometry StraitGeometry(double length, StraitOrientation orientation, double direction = 0, Point? pos = null)
        {
            Rectangle rec = new Rectangle(orientation, length + combineLengthOffset, this.TrackWidth).Rotate(direction).Move(pos);
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
            return new GeometryDrawing(this.ballastBrush, null, new PathGeometry(new PathFigureCollection
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

        protected Drawing StraitSleepers(double length, StraitOrientation orientation = StraitOrientation.Center, double direction = 0, Point? pos = null)
        {
            double x = 0;
            switch (orientation)
            {
            case StraitOrientation.Left: x = 0; break;
            case StraitOrientation.Center: x = -length / 2; break;
            case StraitOrientation.Right: x = -length; break;
            }


            int num = (int)Math.Round(length / (this.RailWidth / 2));
            double sleepersDistance = length / num;

            var railDrawing = new DrawingGroup();

            for (int i = 0; i < num; i++)
            {
                double sx = x + sleepersDistance / 2 + sleepersDistance * i;
                double sy = this.sleeperWidth / 2;
                railDrawing.Children.Add(new GeometryDrawing(null, this.sleeperPen, new LineGeometry(
                    new Point(sx, -sy).Rotate(direction).Move(pos),
                    new Point(sx, +sy).Rotate(direction).Move(pos))));
            }

            return railDrawing;
        }
        
        protected Drawing StraitRail(double length, StraitOrientation orientation = StraitOrientation.Center, double direction = 0, Point? pos = null)
        {
            double x = 0;
            switch (orientation)
            {
            case StraitOrientation.Left: x = 0; break;
            case StraitOrientation.Center: x = -length / 2; break;
            case StraitOrientation.Right: x = -length; break;
            }

            var railDrawing = new DrawingGroup();            

            railDrawing.Children.Add(new GeometryDrawing(null, this.railPen, new LineGeometry(new Point(x, -this.railWidth / 2).Rotate(direction).Move(pos), new Point(x + length, -this.railWidth / 2).Rotate(direction).Move(pos))));
            railDrawing.Children.Add(new GeometryDrawing(null, this.railPen, new LineGeometry(new Point(x, +this.railWidth / 2).Rotate(direction).Move(pos), new Point(x + length, +this.railWidth / 2).Rotate(direction).Move(pos))));
            
            return railDrawing;
        }

        protected Geometry CurvedGeometry(double angle, double radius, CurvedOrientation orientation, Point pos)
        {
            angle += combineAngleOffset;
            double outerTrackRadius = radius + this.TrackWidth / 2;
            double innerTrackRadius = radius - this.TrackWidth / 2;
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

            //return new PathGeometry(new PathFigureCollection
            //{
            //    new PathFigure(circleCenter - PointExtentions.Circle(startAngle, innerTrackRadius), new PathSegmentCollection
            //    {
            //        new LineSegment(circleCenter - PointExtentions.Circle(startAngle, outerTrackRadius), true),
            //        new ArcSegment (circleCenter - PointExtentions.Circle(startAngle + angle, outerTrackRadius), outerTrackSize, angle, false, SweepDirection.Counterclockwise, true),

            //        new LineSegment(circleCenter - PointExtentions.Circle(startAngle + angle, innerTrackRadius), true),
            //        new ArcSegment (circleCenter - PointExtentions.Circle(startAngle,  innerTrackRadius), innerTrackSize, angle, false, SweepDirection.Clockwise, true)
            //    }, true)
            //});

            return new PathGeometry(new PathFigureCollection
            {
                new PathFigure(circleCenter.CircleCenter(startAngle, innerTrackRadius), new PathSegmentCollection
                {
                    new LineSegment(circleCenter.CircleCenter(startAngle, outerTrackRadius), true),
                    new ArcSegment (circleCenter.CircleCenter(startAngle + angle, outerTrackRadius), outerTrackSize, angle, false, SweepDirection.Counterclockwise, true),

                    new LineSegment(circleCenter.CircleCenter(startAngle + angle, innerTrackRadius), true),
                    new ArcSegment (circleCenter.CircleCenter(startAngle,  innerTrackRadius), innerTrackSize, angle, false, SweepDirection.Clockwise, true)
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

            //return new GeometryDrawing(TrackBrushes.Ballast, null, new PathGeometry(new PathFigureCollection
            //{
            //    new PathFigure(circleCenter - PointExtentions.Circle(startAngle, innerSleepersRadius), new PathSegmentCollection
            //    {
            //        new LineSegment(circleCenter - PointExtentions.Circle(startAngle, outerSleepersRadius), true),
            //        new ArcSegment (circleCenter - PointExtentions.Circle(startAngle + angle, outerSleepersRadius), outerSleepersSize, angle, false, SweepDirection.Counterclockwise, true),

            //        new LineSegment(circleCenter - PointExtentions.Circle(startAngle + angle, innerSleepersRadius), true),
            //        new ArcSegment (circleCenter - PointExtentions.Circle(startAngle,  innerSleepersRadius), innerSleepersSize, angle, false, SweepDirection.Clockwise, true)
            //    }, true)
            //}));
 
            return new GeometryDrawing(this.ballastBrush, null, new PathGeometry(new PathFigureCollection
            {
                new PathFigure(circleCenter.CircleCenter(startAngle, innerSleepersRadius), new PathSegmentCollection
                {
                    new LineSegment(circleCenter.CircleCenter(startAngle, outerSleepersRadius), true),
                    new ArcSegment (circleCenter.CircleCenter(startAngle + angle, outerSleepersRadius), outerSleepersSize, angle, false, SweepDirection.Counterclockwise, true),

                    new LineSegment(circleCenter.CircleCenter(startAngle + angle, innerSleepersRadius), true),
                    new ArcSegment (circleCenter.CircleCenter(startAngle,  innerSleepersRadius), innerSleepersSize, angle, false, SweepDirection.Clockwise, true)
                }, true)
            }));
        }

        protected Drawing CurvedSleepers(double angle, double radius, CurvedOrientation orientation, Point pos)
        {
            double outerSleepersRadius = radius + this.sleeperWidth / 2;
            double innerSleepersRadius = radius - this.sleeperWidth / 2;

            Point circleCenter = pos + (orientation.HasFlag(CurvedOrientation.Counterclockwise) ? new Vector(0, -radius) : new Vector(0, +radius));
            double startAngle = orientation.HasFlag(CurvedOrientation.Counterclockwise) ? 180 : 0;

            switch (orientation & CurvedOrientation.Direction)
            {
            case CurvedOrientation.Left: startAngle -= 0; break;
            case CurvedOrientation.Center: startAngle -= angle / 2; break;
            case CurvedOrientation.Right: startAngle -= angle; break;
            }

            double lenth = radius * 2 * Math.PI * angle / 360.0;
            int num = (int)Math.Round(lenth / (this.RailWidth / 2));
            double sleepersDistance = angle / num;

            var railDrawing = new DrawingGroup();

            for (int i = 0; i < num; i++)
            {
                double ang = startAngle + (sleepersDistance / 2) + sleepersDistance * i;
                //railDrawing.Children.Add(new GeometryDrawing(null, sleepersPen, new LineGeometry(
                //    circleCenter - PointExtentions.Circle(ang, innerSleepersRadius),
                //    circleCenter - PointExtentions.Circle(ang, outerSleepersRadius))));
                railDrawing.Children.Add(new GeometryDrawing(null, this.sleeperPen, new LineGeometry(
                    circleCenter.CircleCenter(ang, innerSleepersRadius),
                    circleCenter.CircleCenter(ang, outerSleepersRadius))));
            }

            return railDrawing;
        }

        protected Drawing CurvedRail(double angle, double radius, CurvedOrientation orientation, Point pos)
        {
            double outerTrackRadius = radius + this.railWidth / 2;
            double innerTrackRadius = radius - this.railWidth / 2;
            
            Size innerTrackSize = new Size(innerTrackRadius, innerTrackRadius);

            Point circleCenter = pos + (orientation.HasFlag(CurvedOrientation.Counterclockwise) ? new Vector(0, -radius) : new Vector(0, +radius));
            double startAngle = orientation.HasFlag(CurvedOrientation.Counterclockwise) ? 180 : 0;

            switch (orientation & CurvedOrientation.Direction)
            {
            case CurvedOrientation.Left: startAngle -= 0; break;
            case CurvedOrientation.Center: startAngle -= angle / 2; break;
            case CurvedOrientation.Right: startAngle -= angle; break;
            }

            var railDrawing = new DrawingGroup();

            //railDrawing.Children.Add(new GeometryDrawing(null, railPen, new PathGeometry(new PathFigureCollection
            //{
            //    new PathFigure(circleCenter - PointExtentions.Circle(startAngle, innerTrackRadius), new PathSegmentCollection
            //    {
            //        new ArcSegment (circleCenter - PointExtentions.Circle(startAngle + angle, innerTrackRadius), innerTrackSize, angle, false, SweepDirection.Counterclockwise, true)
            //    }, false)
            //})));
            //railDrawing.Children.Add(new GeometryDrawing(null, railPen, new PathGeometry(new PathFigureCollection
            //{
            //    new PathFigure(circleCenter - PointExtentions.Circle(startAngle, outerTrackRadius), new PathSegmentCollection
            //    {
            //        new ArcSegment (circleCenter - PointExtentions.Circle(startAngle + angle, outerTrackRadius), innerTrackSize, angle, false, SweepDirection.Counterclockwise, true)
            //    }, false)
            //})));

            railDrawing.Children.Add(new GeometryDrawing(null, this.railPen, new PathGeometry(new PathFigureCollection
            {
                new PathFigure(circleCenter.CircleCenter(startAngle, innerTrackRadius), new PathSegmentCollection
                {
                    new ArcSegment (circleCenter.CircleCenter(startAngle + angle, innerTrackRadius), innerTrackSize, angle, false, SweepDirection.Counterclockwise, true)
                }, false)
            })));
            railDrawing.Children.Add(new GeometryDrawing(null, this.railPen, new PathGeometry(new PathFigureCollection
            {
                new PathFigure(circleCenter.CircleCenter(startAngle, outerTrackRadius), new PathSegmentCollection
                {
                    new ArcSegment (circleCenter.CircleCenter(startAngle + angle, outerTrackRadius), innerTrackSize, angle, false, SweepDirection.Counterclockwise, true)
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

        protected double GetValue(List<TrackNamedValue> list, string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return 0;
                //throw new ArgumentNullException("value", $"Error in article {this.Article}");
            }
            if (list == null || Char.IsDigit(value[0]))
            {
                return double.Parse(value, new CultureInfo("en-US"));
            }
            else
            {
                return list.First(i => i.Name == value).Value;
            }
        }

        protected string GetName(string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("value", $"Error in article {this.Article}");
            }
            if (Char.IsDigit(value[0]))
            {
                return string.Empty;
            }
            else
            {
                return value;
            }
        }
    }
}
