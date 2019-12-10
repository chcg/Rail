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
        protected Brush ballastBrush = new SolidColorBrush(Color.FromRgb(0x51, 0x56, 0x5c));
        protected Pen dockPen = new Pen(Brushes.Blue, 2);
        protected Pen linePen = new Pen(Brushes.Black, 2);
        protected Pen railPen;
        protected Pen sleepersPen;
        protected FormattedText text;
        protected double sleepersOutstanding;

        [XmlAttribute("Id")]
        public string Id { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("Decription")]
        public string Decription { get; set; }

        [XmlIgnore]
        public double Spacing { get; protected set; }

        [XmlIgnore]
        public bool Ballast { get; protected set; }

        [XmlIgnore]
        public Geometry Geometry { get; protected set; }

        [XmlIgnore]
        public Drawing BallastDrawing { get; protected set; }

        [XmlIgnore]
        public Drawing RailDrawing { get; protected set; }

        [XmlIgnore]
        public List<TrackDockPoint> DockPoints { get; protected set; }

        public virtual void Update(double spacing, bool ballast)
        {
            if (spacing == 0.0)
            {
                throw new ArgumentOutOfRangeException(nameof(spacing));
            }
            this.Spacing = spacing;
            this.Ballast = ballast;
            this.sleepersOutstanding = this.Spacing / 3;
            this.railPen = new Pen(Brushes.Black, this.Spacing / 10);
            this.sleepersPen = new Pen(Brushes.Black, this.Spacing / 4);
            this.text = new FormattedText(this.Id, CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface("Verdana"), this.Spacing * 0.9, Brushes.Black, 1.25);
        }

        // for TrackControl
        public virtual void Render(DrawingContext drawingContext, bool showRails)
        {
            if (showRails && this.RailDrawing !=null)
            {
                if (this.Ballast)
                {
                    drawingContext.DrawDrawing(this.BallastDrawing);
                }
                drawingContext.DrawDrawing(this.RailDrawing);
            }
            else
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

        protected Drawing CreateStraitBallastDrawing(double length)
        {
           return new GeometryDrawing(this.ballastBrush, null, new RectangleGeometry(new Rect(-length / 2.0, -this.Spacing, length, this.Spacing * 2)));
        }

        protected Drawing CreateStraitTrackDrawing(double length)
        {
            int num = (int)Math.Round(length / (this.Spacing / 2));
            double sleepersDistance = length / num;

            var railDrawing = new DrawingGroup();
            railDrawing.Children.Add(new GeometryDrawing(null, railPen, new LineGeometry(new Point(-length / 2, -this.Spacing / 2), new Point(length / 2, -this.Spacing / 2))));
            railDrawing.Children.Add(new GeometryDrawing(null, railPen, new LineGeometry(new Point(-length / 2, this.Spacing / 2), new Point(length / 2, this.Spacing / 2))));
            for (int i = 0; i < num; i++)
            {
                railDrawing.Children.Add(new GeometryDrawing(null, sleepersPen, new LineGeometry(
                    new Point(-length / 2 + sleepersDistance / 2 + sleepersDistance * i, -this.Spacing / 2 - this.sleepersOutstanding),
                    new Point(-length / 2 + sleepersDistance / 2 + sleepersDistance * i,  this.Spacing / 2 + this.sleepersOutstanding))));
            }
            return railDrawing;
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

        protected Drawing CreateCurvedBallastDrawing(double angle, double radius)
        {
            Size innerBallastSize = new Size(radius - this.Spacing, radius - this.Spacing);
            Size outerBallastSize = new Size(radius + this.Spacing, radius + this.Spacing);
            Point circleCenter = new Point(0, radius);

            return new GeometryDrawing(ballastBrush, null,
                new PathGeometry(new PathFigureCollection
                {
                    new PathFigure(circleCenter - PointExtentions.Circle(-angle / 2, radius - this.Spacing / 2), new PathSegmentCollection
                    {
                        new LineSegment(circleCenter - PointExtentions.Circle(-angle / 2, radius + this.Spacing), true),
                        new ArcSegment (circleCenter - PointExtentions.Circle(+angle / 2, radius + this.Spacing), outerBallastSize, angle, false, SweepDirection.Counterclockwise, true),

                        new LineSegment(circleCenter - PointExtentions.Circle(+angle / 2, radius - this.Spacing), true),
                        new ArcSegment (circleCenter - PointExtentions.Circle(-angle / 2, radius - this.Spacing), innerBallastSize, angle, false, SweepDirection.Clockwise, true)
                    }, true)
                }));
        }

        protected Drawing CreateCurvedTrackDrawing(double angle, double radius)
        {
            double lenth = radius * 2 * Math.PI * angle / 360.0;
            int num = (int)Math.Round(lenth / (this.Spacing / 2));
            double sleepersDistance = angle / num;

            Size innerSize = new Size(radius - this.Spacing / 2, radius - this.Spacing / 2);
            Size outerSize = new Size(radius + this.Spacing / 2, radius + this.Spacing / 2);
            Point circleCenter = new Point(0, radius);

            var railDrawing = new DrawingGroup();
            
            railDrawing.Children.Add(new GeometryDrawing(null, railPen, new PathGeometry(new PathFigureCollection
            {
                new PathFigure(circleCenter - PointExtentions.Circle(-angle / 2, radius - this.Spacing / 2), new PathSegmentCollection
                {
                    new ArcSegment (circleCenter - PointExtentions.Circle(angle / 2, radius - this.Spacing / 2), innerSize, angle, false, SweepDirection.Counterclockwise, true)
                }, false)
            })));
            railDrawing.Children.Add(new GeometryDrawing(null, railPen, new PathGeometry(new PathFigureCollection
            {
                new PathFigure(circleCenter - PointExtentions.Circle(-angle / 2, radius + this.Spacing / 2), new PathSegmentCollection
                {
                    new ArcSegment (circleCenter - PointExtentions.Circle(angle / 2, radius + this.Spacing / 2), innerSize, angle, false, SweepDirection.Counterclockwise, true)
                }, false)
            })));
            for (int i = 0; i < num; i++)
            {
                double ang = (-angle / 2) + (sleepersDistance / 2) + sleepersDistance * i; 
                railDrawing.Children.Add(new GeometryDrawing(null, sleepersPen, new LineGeometry(
                    circleCenter - PointExtentions.Circle(ang,  radius - this.Spacing / 2 - this.sleepersOutstanding),
                    circleCenter - PointExtentions.Circle(ang,  radius + this.Spacing / 2 + this.sleepersOutstanding)
                    )));
            }
            return railDrawing;
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

        protected Drawing CreateLeftTurnoutBallastDrawing(double length, double angle, double radius)
        {
            double x = -length / 2.0;
            double y = 0.0;
            
            Size innerBallastSize = new Size(radius - this.Spacing, radius - this.Spacing);
            Size outerBallastSize = new Size(radius + this.Spacing, radius + this.Spacing);
            Point circleCenter = new Point(x, y - radius);

            return new GeometryDrawing(ballastBrush, null,
                new PathGeometry(new PathFigureCollection
                {
                        new PathFigure(new Point(x, y - this.Spacing), new PathSegmentCollection
                        {
                            new LineSegment(new Point(x, y + this.Spacing), true),
                            new ArcSegment (new Point(x, y + this.Spacing).Rotate(-angle, circleCenter), innerBallastSize, angle, false, SweepDirection.Counterclockwise, true),

                            new LineSegment(new Point(x, y - this.Spacing).Rotate(-angle, circleCenter), true),
                            new ArcSegment (new Point(x, y - this.Spacing), outerBallastSize, angle, false, SweepDirection.Clockwise, true),
                        }, true)
                }));
        }

        protected Drawing CreateLeftTurnoutDrawing(double length, double angle, double radius)
        {
            double x = -length / 2.0;
            double y = 0.0;

            length = radius * 2 * Math.PI * angle / 360.0;
            int num = (int)Math.Round(length / (this.Spacing / 2));
            double sleepersDistance = angle / num;

            Size innerSize = new Size(radius - this.Spacing / 2, radius - this.Spacing / 2);
            Size outerSize = new Size(radius + this.Spacing / 2, radius + this.Spacing / 2);
            Point circleCenter = new Point(x, y - radius);

            var railDrawing = new DrawingGroup();
            railDrawing.Children.Add(new GeometryDrawing(null, railPen, new PathGeometry(new PathFigureCollection
            {
                new PathFigure(new Point(x, y - this.Spacing / 2), new PathSegmentCollection
                {
                    new ArcSegment (new Point(x, y - this.Spacing / 2).Rotate(-angle, circleCenter), innerSize, angle, false, SweepDirection.Counterclockwise, true)
                }, false)
            })));
            railDrawing.Children.Add(new GeometryDrawing(null, railPen, new PathGeometry(new PathFigureCollection
            {
                new PathFigure(new Point(x, y + this.Spacing / 2), new PathSegmentCollection
                {
                    new ArcSegment (new Point(x, y + this.Spacing / 2).Rotate(-angle, circleCenter), innerSize, angle, false, SweepDirection.Counterclockwise, true)
                }, false)
            })));
            for (int i = 0; i < num; i++)
            {
                double ang = (sleepersDistance / 2) + sleepersDistance * i;
                railDrawing.Children.Add(new GeometryDrawing(null, sleepersPen, new LineGeometry(
                    circleCenter + PointExtentions.Circle(ang, radius - this.Spacing / 2 - this.sleepersOutstanding),
                    circleCenter + PointExtentions.Circle(ang, radius + this.Spacing / 2 + this.sleepersOutstanding)
                    )));
            }
            return railDrawing;
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

        protected Drawing CreateRightTurnoutBallastDrawing(double length, double angle, double radius)
        {
            double x = -length / 2.0;
            double y = 0.0;

            Size innerBallastSize = new Size(radius - this.Spacing, radius - this.Spacing);
            Size outerBallastSize = new Size(radius + this.Spacing, radius + this.Spacing);
            Point circleCenter = new Point(x, y + radius);

            return new GeometryDrawing(ballastBrush, null,
                new PathGeometry(new PathFigureCollection
                {
                        new PathFigure(new Point(x, y - this.Spacing), new PathSegmentCollection
                        {
                            new LineSegment(new Point(x, y + this.Spacing), true),
                            new ArcSegment (new Point(x, y + this.Spacing).Rotate(angle, circleCenter), innerBallastSize, angle, false, SweepDirection.Clockwise, true),

                            new LineSegment(new Point(x, y - this.Spacing).Rotate(angle, circleCenter), true),
                            new ArcSegment (new Point(x, y - this.Spacing), outerBallastSize, angle, false, SweepDirection.Counterclockwise, true),
                        }, true)
                }));
        }

        protected Drawing CreateRightTurnoutDrawing(double length, double angle, double radius)
        {
            var railDrawing = new DrawingGroup();
                        
            double x = -length / 2.0;
            double y = 0.0;

            length = radius * 2 * Math.PI * angle / 360.0;
            int num = (int)Math.Round(length / (this.Spacing / 2));
            double sleepersDistance = angle / num;

            Size innerSize = new Size(radius - this.Spacing / 2, radius - this.Spacing / 2);
            Size outerSize = new Size(radius + this.Spacing / 2, radius + this.Spacing / 2);
            Point circleCenter = new Point(x, y + radius);

            railDrawing.Children.Add(new GeometryDrawing(null, railPen, new PathGeometry(new PathFigureCollection
            {
                new PathFigure(new Point(x, y - this.Spacing / 2), new PathSegmentCollection
                {
                    new ArcSegment (new Point(x, y - this.Spacing / 2).Rotate(angle, circleCenter), innerSize, angle, false, SweepDirection.Clockwise, true)
                }, false)
            })));
            railDrawing.Children.Add(new GeometryDrawing(null, railPen, new PathGeometry(new PathFigureCollection
            {
                new PathFigure(new Point(x, y + this.Spacing / 2), new PathSegmentCollection
                {
                    new ArcSegment (new Point(x, y + this.Spacing / 2).Rotate(angle, circleCenter), innerSize, angle, false, SweepDirection.Clockwise, true)
                }, false)
            })));
            for (int i = 0; i < num; i++)
            {
                double ang = 180 - (sleepersDistance / 2) - sleepersDistance * i;
                railDrawing.Children.Add(new GeometryDrawing(null, sleepersPen, new LineGeometry(
                    circleCenter + PointExtentions.Circle(ang, radius - this.Spacing / 2 - this.sleepersOutstanding),
                    circleCenter + PointExtentions.Circle(ang, radius + this.Spacing / 2 + this.sleepersOutstanding)
                    )));
            }
            return railDrawing;
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

        protected Drawing CreateCrossingBallastDrawing(double length1, double length2, double angle)
        {
            return new GeometryDrawing(ballastBrush, null,
                new CombinedGeometry(
                    new PathGeometry(new PathFigureCollection
                    {
                        new PathFigure(new Point(length1 / 2, this.Spacing).Rotate(angle / 2), new PathSegmentCollection
                        {
                            new LineSegment(new Point(-length1 / 2,  this.Spacing).Rotate(angle / 2), true),
                            new LineSegment(new Point(-length1 / 2, -this.Spacing).Rotate(angle / 2), true),
                            new LineSegment(new Point( length1 / 2, -this.Spacing).Rotate(angle / 2), true),
                            new LineSegment(new Point( length1 / 2,  this.Spacing).Rotate(angle / 2), true),
                        }, true)
                    }),
                    new PathGeometry(new PathFigureCollection
                    {
                        new PathFigure(new Point(length2 / 2, this.Spacing / 2).Rotate(-angle / 2), new PathSegmentCollection
                        {
                            new LineSegment(new Point(-length2 / 2,  this.Spacing).Rotate(-angle / 2), true),
                            new LineSegment(new Point(-length2 / 2, -this.Spacing).Rotate(-angle / 2), true),
                            new LineSegment(new Point( length2 / 2, -this.Spacing).Rotate(-angle / 2), true),
                            new LineSegment(new Point( length2 / 2,  this.Spacing).Rotate(-angle / 2), true),
                        }, true)
                    })));
        }

        protected Drawing CreateCrossingTrackDrawing(double length1, double length2, double angle)
        {
            var railDrawing = new DrawingGroup();

            railDrawing.Children.Add(new GeometryDrawing(null, railPen, new LineGeometry(new Point(-length1 / 2, -this.Spacing / 2).Rotate(+angle / 2), new Point(length1 / 2, -this.Spacing / 2).Rotate(+angle / 2))));
            railDrawing.Children.Add(new GeometryDrawing(null, railPen, new LineGeometry(new Point(-length1 / 2, +this.Spacing / 2).Rotate(+angle / 2), new Point(length1 / 2, +this.Spacing / 2).Rotate(+angle / 2))));

            railDrawing.Children.Add(new GeometryDrawing(null, railPen, new LineGeometry(new Point(-length2 / 2, -this.Spacing / 2).Rotate(-angle / 2), new Point(length2 / 2, -this.Spacing / 2).Rotate(-angle / 2))));
            railDrawing.Children.Add(new GeometryDrawing(null, railPen, new LineGeometry(new Point(-length2 / 2, +this.Spacing / 2).Rotate(-angle / 2), new Point(length2 / 2, +this.Spacing / 2).Rotate(-angle / 2))));

            int num1 = (int)Math.Round(length1 / (this.Spacing / 2));
            double sleepersDistance1 = length1 / num1;

            for (int i = 0; i < num1; i++)
            {
                railDrawing.Children.Add(new GeometryDrawing(null, sleepersPen, new LineGeometry(
                    new Point(-length1 / 2 + sleepersDistance1 / 2 + sleepersDistance1 * i, -this.Spacing / 2 - this.sleepersOutstanding).Rotate(+angle / 2),
                    new Point(-length1 / 2 + sleepersDistance1 / 2 + sleepersDistance1 * i, +this.Spacing / 2 + this.sleepersOutstanding).Rotate(+angle / 2))));
            }


            int num2 = (int)Math.Round(length2 / (this.Spacing / 2));
            double sleepersDistance2 = length2 / num2;
            for (int i = 0; i < num2; i++)
            {
                railDrawing.Children.Add(new GeometryDrawing(null, sleepersPen, new LineGeometry(
                    new Point(-length2 / 2 + sleepersDistance2 / 2 + sleepersDistance2 * i, -this.Spacing / 2 - this.sleepersOutstanding).Rotate(-angle / 2),
                    new Point(-length2 / 2 + sleepersDistance2 / 2 + sleepersDistance2 * i, +this.Spacing / 2 + this.sleepersOutstanding).Rotate(-angle / 2))));
            }
            return railDrawing;
        }
    }
}
