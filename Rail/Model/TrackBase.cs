//using System.Drawing;
using System.Windows.Media;
using System.Windows;
using System.Xml.Serialization;
using System.Globalization;
using Rail.Controls;
using System;
using System.Collections.Generic;

namespace Rail.Model
{
    public abstract class TrackBase
    {
        protected double spacing;
        
        protected Brush trackBrush = new SolidColorBrush(Colors.White);
        protected Pen linePen = new Pen(Brushes.Black, 1);
        protected Pen dockPen = new Pen(Brushes.Blue, 1);
        protected FormattedText text;

        [XmlAttribute("Id")]
        public string Id { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("Decription")]
        public string Decription { get; set; }

        [XmlIgnore]
        public Geometry Geometry { get; protected set; }

        [XmlIgnore]
        public List<TrackDockPoint> DockPoints { get; protected set; }

        public virtual void Update(double spacing)
        {
            this.spacing = spacing != 0 ? spacing : 16;
            this.text = new FormattedText(this.Id, CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface("Verdana"), 14, Brushes.Black, 1.25);
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
                new PathFigure(new Point(-length / 2.0, -this.spacing / 2), new PathSegmentCollection
                {
                    new LineSegment(new Point( length / 2.0, -this.spacing / 2), true),
                    new LineSegment(new Point( length / 2.0,  this.spacing / 2), true),
                    new LineSegment(new Point(-length / 2.0,  this.spacing / 2), true),
                    new LineSegment(new Point(-length / 2.0, -this.spacing / 2), true)
                }, true)
            });
        }
              

        protected Geometry CreateCurvedTrackGeometry(double angle, double radius)
        {
            Size innerSize = new Size(radius - this.spacing / 2, radius - this.spacing / 2);
            Size outerSize = new Size(radius + this.spacing / 2, radius + this.spacing / 2);
            
            Point circleCenter = new Point(0, radius); 

            return new PathGeometry(new PathFigureCollection
            {
                new PathFigure(circleCenter - Circle(-angle / 2, radius - this.spacing / 2), new PathSegmentCollection
                {
                    new LineSegment(circleCenter - Circle(-angle / 2, radius + this.spacing / 2), true),
                    new ArcSegment (circleCenter - Circle(angle / 2, radius + this.spacing / 2), outerSize, angle, false, SweepDirection.Counterclockwise, true),

                    new LineSegment(circleCenter - Circle(angle / 2, radius - this.spacing / 2), true),
                    new ArcSegment (circleCenter - Circle(-angle / 2, radius - this.spacing / 2), innerSize, angle, false, SweepDirection.Clockwise, true)
                }, true)
            });
        }

        protected Geometry CreateLeftTurnoutGeometry(double length, double angle, double radius)
        {
            double x = -length / 2.0;
            double y = 0.0;

            Size innerSize = new Size(radius - this.spacing / 2, radius - this.spacing / 2);
            Size outerSize = new Size(radius + this.spacing / 2, radius + this.spacing / 2);
            Point circleCenter = new Point(x, y - radius);

            return new PathGeometry(new PathFigureCollection
            {
                new PathFigure(new Point(x, y - this.spacing / 2), new PathSegmentCollection
                {
                    new LineSegment(new Point(x, y + this.spacing / 2), true),
                    new ArcSegment (new Point(x, y + this.spacing / 2).Rotate(-angle, circleCenter), innerSize, angle, false, SweepDirection.Counterclockwise, true),

                    new LineSegment(new Point(x, y - this.spacing / 2).Rotate(-angle, circleCenter), true),
                    new ArcSegment (new Point(x, y - this.spacing / 2), outerSize, angle, false, SweepDirection.Clockwise, true),
                }, true)
            });
        }

        protected Geometry CreateRightTurnoutGeometry(double length, double angle, double radius)
        {
            double x = -length / 2.0;
            double y = 0.0;
            Size innerSize = new Size(radius - this.spacing / 2, radius - this.spacing / 2);
            Size outerSize = new Size(radius + this.spacing / 2, radius + this.spacing / 2);
            Point circleCenter = new Point(x, y + radius);

            return new PathGeometry(new PathFigureCollection
            {
                new PathFigure(new Point(x, y - this.spacing / 2), new PathSegmentCollection
                {
                    new LineSegment(new Point(x, y + this.spacing / 2), true),
                    new ArcSegment (new Point(x, y + this.spacing / 2).Rotate(angle, circleCenter), innerSize, angle, false, SweepDirection.Clockwise, true),

                    new LineSegment(new Point(x, y - this.spacing / 2).Rotate(angle, circleCenter), true),
                    new ArcSegment (new Point(x, y - this.spacing / 2), outerSize, angle, false, SweepDirection.Counterclockwise, true),
                }, true)
            });
        }

        protected Geometry CreateCrossingTrackGeometry(double length1, double length2, double angle)
        {
            return new CombinedGeometry(
                new PathGeometry(new PathFigureCollection
                {
                    new PathFigure(Rotate(new Point(length1 / 2, this.spacing / 2), angle / 2), new PathSegmentCollection
                    {
                        new LineSegment(Rotate(new Point(-length1 / 2,  this.spacing / 2), angle / 2), true),
                        new LineSegment(Rotate(new Point(-length1 / 2, -this.spacing / 2), angle / 2), true),
                        new LineSegment(Rotate(new Point( length1 / 2, -this.spacing / 2), angle / 2), true),
                        new LineSegment(Rotate(new Point( length1 / 2,  this.spacing / 2), angle / 2), true),
                    }, true)
                }),
                new PathGeometry(new PathFigureCollection
                {
                    new PathFigure(Rotate(new Point(length1 / 2, this.spacing / 2), -angle / 2), new PathSegmentCollection
                    {
                        new LineSegment(Rotate(new Point(-length1 / 2,  this.spacing / 2), -angle / 2), true),
                        new LineSegment(Rotate(new Point(-length1 / 2, -this.spacing / 2), -angle / 2), true),
                        new LineSegment(Rotate(new Point( length1 / 2, -this.spacing / 2), -angle / 2), true),
                        new LineSegment(Rotate(new Point( length1 / 2,  this.spacing / 2), -angle / 2), true),
                    }, true)
                }));
        }

        #region Helper

        protected static double Sin(double angle)
        {
            return Math.Sin(Math.PI * angle / 180.0);
        }

        protected static double Cos(double angle)
        {
            return Math.Sin(Math.PI * angle / 180.0);
        }

        public static Vector Circle(double angle, double radius)
        {
            angle *= (Math.PI / 180.0);
            double sin = Math.Sin(angle);
            double cos = Math.Cos(angle);

            return (Vector)new Point(sin * radius, cos * radius);
        }

        public static Point Rotate(double x, double y, double angle)
        {
            return new Point(x * Cos(angle) - y * Sin(angle), x * Sin(angle) + y * Cos(angle));
        }

        public Point Rotate(Point point, double angle)
        {
            angle *= (Math.PI / 180.0);
            double sin = Math.Sin(angle);
            double cos = Math.Cos(angle);

            return new Point(
                (cos * point.X) - (sin * point.Y),
                (sin * point.X) + (cos * point.Y));
        }

        //public static Point Rotate(Point p, double angle)
        //{
        //    return Rotate(p.X, p.Y, angle);
        //}

        #endregion
    }
}
