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
        protected Pen linePen = new Pen(Brushes.Black, 2);
        protected Pen dockPen = new Pen(Brushes.Blue, 2);
        protected FormattedText text;

        [XmlAttribute("Id")]
        public string Id { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("Decription")]
        public string Decription { get; set; }

        [XmlIgnore]
        public double Spacing { get; protected set; }

        [XmlIgnore]
        public Geometry Geometry { get; protected set; }

        [XmlIgnore]
        public List<TrackDockPoint> DockPoints { get; protected set; }

        public virtual void Update(double spacing)
        {
            if (spacing == 0.0)
            {
                throw new ArgumentOutOfRangeException(nameof(spacing));
            }
            this.Spacing = spacing;
            this.text = new FormattedText(this.Id, CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface("Verdana"), this.Spacing * 0.9, Brushes.Black, 1.25);
        }

        // for TrackControl
        public virtual void Render(DrawingContext drawingContext)
        {

            if (this.Geometry != null)
            {
                drawingContext.DrawGeometry(trackBrush, linePen, this.Geometry);
            }

            drawingContext.DrawText(text, new Point(0, 0) - new Vector(text.Width / 2, text.Height / 2));

            // for test only
            //drawingContext.DrawLine(linePen, new Point(-110, 0), new Point(110, 0));
            //drawingContext.DrawLine(linePen, new Point(0, -30), new Point(0, 30));

            //if (this is TrackCurved)
            //{
            //    drawingContext.DrawEllipse(Brushes.Red, linePen, new Point(0, (this as TrackCurved).Radius), 8, 8); 
            //}
        }

        public virtual void Render(DrawingContext drawingContext, Point pos, double angle)
        {
            //drawingContext.DrawPosition(this.Position);
            //drawingContext.DrawDockRect(this.circleCenter);

            TransformGroup grp = new TransformGroup();
            grp.Children.Add(new TranslateTransform(pos.X, pos.Y));
            grp.Children.Add(new RotateTransform(angle, pos.X, pos.Y));

            if (this.Geometry != null)
            {
                this.Geometry.Transform = grp;
                drawingContext.DrawGeometry(trackBrush, linePen, this.Geometry);
            }

            drawingContext.DrawText(text, new Point(0, 0) - new Vector(text.Width / 2, text.Height / 2));
            //drawingContext.DrawText(pos, (angle + 90.0) % 180.0 - 90.0, text);

            
            //if (this.DockPoints != null)
            //{
            //    foreach (DockPoint point in this.DockPoints)
            //    {
            //        drawingContext.DrawDockRect(point);
            //    }
            //}

            //drawingContext.DrawRectangle(null, linePen, this.Bounds);
        }

        protected Geometry CreateStraitTrackGeometry(double length)
        {
            return new PathGeometry(new PathFigureCollection
            {
                new PathFigure(new Point(-length / 2.0, -this.Spacing / 2), new PathSegmentCollection
                {
                    new LineSegment(new Point( length / 2.0, -this.Spacing / 2), true),
                    new LineSegment(new Point( length / 2.0,  this.Spacing / 2), true),
                    new LineSegment(new Point(-length / 2.0,  this.Spacing / 2), true),
                    new LineSegment(new Point(-length / 2.0, -this.Spacing / 2), true)
                }, true)
            });
        }
              

        protected Geometry CreateCurvedTrackGeometry(double angle, double radius)
        {
            Size innerSize = new Size(radius - this.Spacing / 2, radius - this.Spacing / 2);
            Size outerSize = new Size(radius + this.Spacing / 2, radius + this.Spacing / 2);
            
            Point circleCenter = new Point(0, radius); 

            return new PathGeometry(new PathFigureCollection
            {
                new PathFigure(circleCenter - PointExtentions.Circle(-angle / 2, radius - this.Spacing / 2), new PathSegmentCollection
                {
                    new LineSegment(circleCenter - PointExtentions.Circle(-angle / 2, radius + this.Spacing / 2), true),
                    new ArcSegment (circleCenter - PointExtentions.Circle(angle / 2, radius + this.Spacing / 2), outerSize, angle, false, SweepDirection.Counterclockwise, true),

                    new LineSegment(circleCenter - PointExtentions.Circle(angle / 2, radius - this.Spacing / 2), true),
                    new ArcSegment (circleCenter - PointExtentions.Circle(-angle / 2, radius - this.Spacing / 2), innerSize, angle, false, SweepDirection.Clockwise, true)
                }, true)
            });
        }

        protected Geometry CreateLeftTurnoutGeometry(double length, double angle, double radius)
        {
            double x = -length / 2.0;
            double y = 0.0;

            Size innerSize = new Size(radius - this.Spacing / 2, radius - this.Spacing / 2);
            Size outerSize = new Size(radius + this.Spacing / 2, radius + this.Spacing / 2);
            Point circleCenter = new Point(x, y - radius);

            return new PathGeometry(new PathFigureCollection
            {
                new PathFigure(new Point(x, y - this.Spacing / 2), new PathSegmentCollection
                {
                    new LineSegment(new Point(x, y + this.Spacing / 2), true),
                    new ArcSegment (new Point(x, y + this.Spacing / 2).Rotate(-angle, circleCenter), innerSize, angle, false, SweepDirection.Counterclockwise, true),

                    new LineSegment(new Point(x, y - this.Spacing / 2).Rotate(-angle, circleCenter), true),
                    new ArcSegment (new Point(x, y - this.Spacing / 2), outerSize, angle, false, SweepDirection.Clockwise, true),
                }, true)
            });
        }

        protected Geometry CreateRightTurnoutGeometry(double length, double angle, double radius)
        {
            double x = -length / 2.0;
            double y = 0.0;
            Size innerSize = new Size(radius - this.Spacing / 2, radius - this.Spacing / 2);
            Size outerSize = new Size(radius + this.Spacing / 2, radius + this.Spacing / 2);
            Point circleCenter = new Point(x, y + radius);

            return new PathGeometry(new PathFigureCollection
            {
                new PathFigure(new Point(x, y - this.Spacing / 2), new PathSegmentCollection
                {
                    new LineSegment(new Point(x, y + this.Spacing / 2), true),
                    new ArcSegment (new Point(x, y + this.Spacing / 2).Rotate(angle, circleCenter), innerSize, angle, false, SweepDirection.Clockwise, true),

                    new LineSegment(new Point(x, y - this.Spacing / 2).Rotate(angle, circleCenter), true),
                    new ArcSegment (new Point(x, y - this.Spacing / 2), outerSize, angle, false, SweepDirection.Counterclockwise, true),
                }, true)
            });
        }

        protected Geometry CreateCrossingTrackGeometry(double length1, double length2, double angle)
        {
            return new CombinedGeometry(
                new PathGeometry(new PathFigureCollection
                {
                    new PathFigure(new Point(length1 / 2, this.Spacing / 2).Rotate(angle / 2), new PathSegmentCollection
                    {
                        new LineSegment(new Point(-length1 / 2,  this.Spacing / 2).Rotate(angle / 2), true),
                        new LineSegment(new Point(-length1 / 2, -this.Spacing / 2).Rotate(angle / 2), true),
                        new LineSegment(new Point( length1 / 2, -this.Spacing / 2).Rotate(angle / 2), true),
                        new LineSegment(new Point( length1 / 2,  this.Spacing / 2).Rotate(angle / 2), true),
                    }, true)
                }),
                new PathGeometry(new PathFigureCollection
                {
                    new PathFigure(new Point(length2 / 2, this.Spacing / 2).Rotate(-angle / 2), new PathSegmentCollection
                    {
                        new LineSegment(new Point(-length2 / 2,  this.Spacing / 2).Rotate(-angle / 2), true),
                        new LineSegment(new Point(-length2 / 2, -this.Spacing / 2).Rotate(-angle / 2), true),
                        new LineSegment(new Point( length2 / 2, -this.Spacing / 2).Rotate(-angle / 2), true),
                        new LineSegment(new Point( length2 / 2,  this.Spacing / 2).Rotate(-angle / 2), true),
                    }, true)
                }));
        }
    }
}
