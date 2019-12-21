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

        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("Decription")]
        public string Decription { get; set; }

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

        protected abstract void Create();

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

        //public virtual void Render(DrawingContext drawingContext, Point pos, double angle)
        //{
        //    //drawingContext.DrawPosition(this.Position);
        //    //drawingContext.DrawDockRect(this.circleCenter);

        //    TransformGroup grp = new TransformGroup();
        //    grp.Children.Add(new TranslateTransform(pos.X, pos.Y));
        //    grp.Children.Add(new RotateTransform(angle, pos.X, pos.Y));

        //    if (this.Geometry != null)
        //    {
        //        this.Geometry.Transform = grp;
        //        drawingContext.DrawGeometry(trackBrush, linePen, this.Geometry);
        //    }

        //    drawingContext.DrawText(text, new Point(0, 0) - new Vector(text.Width / 2, text.Height / 2));
        //    //drawingContext.DrawText(pos, (angle + 90.0) % 180.0 - 90.0, text);

            
        //    //if (this.DockPoints != null)
        //    //{
        //    //    foreach (DockPoint point in this.DockPoints)
        //    //    {
        //    //        drawingContext.DrawDockRect(point);
        //    //    }
        //    //}

        //    //drawingContext.DrawRectangle(null, linePen, this.Bounds);
        //}

        //protected Geometry CreateStraitTrackGeometry(double length, double spacing)
        //{
        //    return new PathGeometry(new PathFigureCollection
        //    {
        //        new PathFigure(new Point(-length / 2.0, -spacing / 2), new PathSegmentCollection
        //        {
        //            new LineSegment(new Point( length / 2.0, -spacing / 2), true),
        //            new LineSegment(new Point( length / 2.0,  spacing / 2), true),
        //            new LineSegment(new Point(-length / 2.0,  spacing / 2), true),
        //            new LineSegment(new Point(-length / 2.0, -spacing / 2), true)
        //        }, true)
        //    });
        //}

        //protected Drawing CreateStraitBallastDrawing(double length)
        //{
        //   return new GeometryDrawing(this.ballastBrush, null, new RectangleGeometry(new Rect(-length / 2.0, -this.RailSpacing, length, this.RailSpacing * 2)));
        //}

        //protected Drawing CreateStraitTrackDrawing(double length)
        //{
        //    int num = (int)Math.Round(length / (this.RailSpacing / 2));
        //    double sleepersDistance = length / num;

        //    var railDrawing = new DrawingGroup();
        //    railDrawing.Children.Add(new GeometryDrawing(null, railPen, new LineGeometry(new Point(-length / 2, -this.RailSpacing / 2), new Point(length / 2, -this.RailSpacing / 2))));
        //    railDrawing.Children.Add(new GeometryDrawing(null, railPen, new LineGeometry(new Point(-length / 2, this.RailSpacing / 2), new Point(length / 2, this.RailSpacing / 2))));
        //    for (int i = 0; i < num; i++)
        //    {
        //        railDrawing.Children.Add(new GeometryDrawing(null, sleepersPen, new LineGeometry(
        //            new Point(-length / 2 + sleepersDistance / 2 + sleepersDistance * i, -this.sleepersSpacing / 2),
        //            new Point(-length / 2 + sleepersDistance / 2 + sleepersDistance * i,  this.sleepersSpacing / 2))));
        //    }
        //    return railDrawing;
        //}


        //protected Geometry CreateCurvedTrackGeometry(double angle, double radius, double spacing)
        //{
        //    Size innerSize = new Size(radius - spacing / 2, radius - spacing / 2);
        //    Size outerSize = new Size(radius + spacing / 2, radius + spacing / 2);            
        //    Point circleCenter = new Point(0, radius); 

        //    return new PathGeometry(new PathFigureCollection
        //    {
        //        new PathFigure(circleCenter - PointExtentions.Circle(-angle / 2, radius - spacing / 2), new PathSegmentCollection
        //        {
        //            new LineSegment(circleCenter - PointExtentions.Circle(-angle / 2, radius + spacing / 2), true),
        //            new ArcSegment (circleCenter - PointExtentions.Circle(angle / 2, radius + spacing / 2), outerSize, angle, false, SweepDirection.Counterclockwise, true),

        //            new LineSegment(circleCenter - PointExtentions.Circle(angle / 2, radius - spacing / 2), true),
        //            new ArcSegment (circleCenter - PointExtentions.Circle(-angle / 2, radius - spacing / 2), innerSize, angle, false, SweepDirection.Clockwise, true)
        //        }, true)
        //    });
        //}

        protected Drawing CreateCurvedBallastDrawing(double angle, double radius)
        {
            Size innerBallastSize = new Size(radius - this.RailSpacing, radius - this.RailSpacing);
            Size outerBallastSize = new Size(radius + this.RailSpacing, radius + this.RailSpacing);
            Point circleCenter = new Point(0, radius);

            return new GeometryDrawing(ballastBrush, null,
                new PathGeometry(new PathFigureCollection
                {
                    new PathFigure(circleCenter - PointExtentions.Circle(-angle / 2, radius - this.RailSpacing / 2), new PathSegmentCollection
                    {
                        new LineSegment(circleCenter - PointExtentions.Circle(-angle / 2, radius + this.RailSpacing), true),
                        new ArcSegment (circleCenter - PointExtentions.Circle(+angle / 2, radius + this.RailSpacing), outerBallastSize, angle, false, SweepDirection.Counterclockwise, true),

                        new LineSegment(circleCenter - PointExtentions.Circle(+angle / 2, radius - this.RailSpacing), true),
                        new ArcSegment (circleCenter - PointExtentions.Circle(-angle / 2, radius - this.RailSpacing), innerBallastSize, angle, false, SweepDirection.Clockwise, true)
                    }, true)
                }));
        }

        //protected Drawing CreateCurvedTrackDrawing(double angle, double radius)
        //{
        //    double lenth = radius * 2 * Math.PI * angle / 360.0;
        //    int num = (int)Math.Round(lenth / (this.RailSpacing / 2));
        //    double sleepersDistance = angle / num;

        //    Size innerSize = new Size(radius - this.RailSpacing / 2, radius - this.RailSpacing / 2);
        //    Size outerSize = new Size(radius + this.RailSpacing / 2, radius + this.RailSpacing / 2);
        //    Point circleCenter = new Point(0, radius);

        //    var railDrawing = new DrawingGroup();
            
        //    railDrawing.Children.Add(new GeometryDrawing(null, railPen, new PathGeometry(new PathFigureCollection
        //    {
        //        new PathFigure(circleCenter - PointExtentions.Circle(-angle / 2, radius - this.RailSpacing / 2), new PathSegmentCollection
        //        {
        //            new ArcSegment (circleCenter - PointExtentions.Circle(angle / 2, radius - this.RailSpacing / 2), innerSize, angle, false, SweepDirection.Counterclockwise, true)
        //        }, false)
        //    })));
        //    railDrawing.Children.Add(new GeometryDrawing(null, railPen, new PathGeometry(new PathFigureCollection
        //    {
        //        new PathFigure(circleCenter - PointExtentions.Circle(-angle / 2, radius + this.RailSpacing / 2), new PathSegmentCollection
        //        {
        //            new ArcSegment (circleCenter - PointExtentions.Circle(angle / 2, radius + this.RailSpacing / 2), innerSize, angle, false, SweepDirection.Counterclockwise, true)
        //        }, false)
        //    })));
        //    for (int i = 0; i < num; i++)
        //    {
        //        double ang = (-angle / 2) + (sleepersDistance / 2) + sleepersDistance * i; 
        //        railDrawing.Children.Add(new GeometryDrawing(null, sleepersPen, new LineGeometry(
        //            circleCenter - PointExtentions.Circle(ang,  radius - this.sleepersSpacing / 2),
        //            circleCenter - PointExtentions.Circle(ang,  radius + this.sleepersSpacing / 2)
        //            )));
        //    }
        //    return railDrawing;
        //}

        protected Geometry CreateLeftTurnoutGeometry(double length, double angle, double radius)
        {
            double x = -length / 2.0;
            double y = 0.0;

            Size innerSize = new Size(radius - this.RailSpacing / 2, radius - this.RailSpacing / 2);
            Size outerSize = new Size(radius + this.RailSpacing / 2, radius + this.RailSpacing / 2);
            Point circleCenter = new Point(x, y - radius);

            return new PathGeometry(new PathFigureCollection
            {
                new PathFigure(new Point(x, y - this.RailSpacing / 2), new PathSegmentCollection
                {
                    new LineSegment(new Point(x, y + this.RailSpacing / 2), true),
                    new ArcSegment (new Point(x, y + this.RailSpacing / 2).Rotate(-angle, circleCenter), innerSize, angle, false, SweepDirection.Counterclockwise, true),

                    new LineSegment(new Point(x, y - this.RailSpacing / 2).Rotate(-angle, circleCenter), true),
                    new ArcSegment (new Point(x, y - this.RailSpacing / 2), outerSize, angle, false, SweepDirection.Clockwise, true),
                }, true)
            });
        }

        protected Drawing CreateLeftTurnoutBallastDrawing(double length, double angle, double radius)
        {
            double x = -length / 2.0;
            double y = 0.0;
            
            Size innerBallastSize = new Size(radius - this.RailSpacing, radius - this.RailSpacing);
            Size outerBallastSize = new Size(radius + this.RailSpacing, radius + this.RailSpacing);
            Point circleCenter = new Point(x, y - radius);

            return new GeometryDrawing(ballastBrush, null,
                new PathGeometry(new PathFigureCollection
                {
                        new PathFigure(new Point(x, y - this.RailSpacing), new PathSegmentCollection
                        {
                            new LineSegment(new Point(x, y + this.RailSpacing), true),
                            new ArcSegment (new Point(x, y + this.RailSpacing).Rotate(-angle, circleCenter), innerBallastSize, angle, false, SweepDirection.Counterclockwise, true),

                            new LineSegment(new Point(x, y - this.RailSpacing).Rotate(-angle, circleCenter), true),
                            new ArcSegment (new Point(x, y - this.RailSpacing), outerBallastSize, angle, false, SweepDirection.Clockwise, true),
                        }, true)
                }));
        }

        protected Drawing CreateLeftTurnoutDrawing(double length, double angle, double radius)
        {
            double x = -length / 2.0;
            double y = 0.0;

            length = radius * 2 * Math.PI * angle / 360.0;
            int num = (int)Math.Round(length / (this.RailSpacing / 2));
            double sleepersDistance = angle / num;

            Size innerSize = new Size(radius - this.RailSpacing / 2, radius - this.RailSpacing / 2);
            Size outerSize = new Size(radius + this.RailSpacing / 2, radius + this.RailSpacing / 2);
            Point circleCenter = new Point(x, y - radius);

            var railDrawing = new DrawingGroup();
            railDrawing.Children.Add(new GeometryDrawing(null, railPen, new PathGeometry(new PathFigureCollection
            {
                new PathFigure(new Point(x, y - this.RailSpacing / 2), new PathSegmentCollection
                {
                    new ArcSegment (new Point(x, y - this.RailSpacing / 2).Rotate(-angle, circleCenter), innerSize, angle, false, SweepDirection.Counterclockwise, true)
                }, false)
            })));
            railDrawing.Children.Add(new GeometryDrawing(null, railPen, new PathGeometry(new PathFigureCollection
            {
                new PathFigure(new Point(x, y + this.RailSpacing / 2), new PathSegmentCollection
                {
                    new ArcSegment (new Point(x, y + this.RailSpacing / 2).Rotate(-angle, circleCenter), innerSize, angle, false, SweepDirection.Counterclockwise, true)
                }, false)
            })));
            for (int i = 0; i < num; i++)
            {
                double ang = (sleepersDistance / 2) + sleepersDistance * i;
                railDrawing.Children.Add(new GeometryDrawing(null, sleepersPen, new LineGeometry(
                    circleCenter + PointExtentions.Circle(ang, radius - this.sleepersSpacing / 2),
                    circleCenter + PointExtentions.Circle(ang, radius + this.sleepersSpacing / 2)
                    )));
            }
            return railDrawing;
        }

        protected Geometry CreateRightTurnoutGeometry(double length, double angle, double radius)
        {
            double x = -length / 2.0;
            double y = 0.0;
            Size innerSize = new Size(radius - this.RailSpacing / 2, radius - this.RailSpacing / 2);
            Size outerSize = new Size(radius + this.RailSpacing / 2, radius + this.RailSpacing / 2);
            Point circleCenter = new Point(x, y + radius);

            return new PathGeometry(new PathFigureCollection
            {
                new PathFigure(new Point(x, y - this.RailSpacing / 2), new PathSegmentCollection
                {
                    new LineSegment(new Point(x, y + this.RailSpacing / 2), true),
                    new ArcSegment (new Point(x, y + this.RailSpacing / 2).Rotate(angle, circleCenter), innerSize, angle, false, SweepDirection.Clockwise, true),

                    new LineSegment(new Point(x, y - this.RailSpacing / 2).Rotate(angle, circleCenter), true),
                    new ArcSegment (new Point(x, y - this.RailSpacing / 2), outerSize, angle, false, SweepDirection.Counterclockwise, true),
                }, true)
            });
        }

        protected Drawing CreateRightTurnoutBallastDrawing(double length, double angle, double radius)
        {
            double x = -length / 2.0;
            double y = 0.0;

            Size innerBallastSize = new Size(radius - this.RailSpacing, radius - this.RailSpacing);
            Size outerBallastSize = new Size(radius + this.RailSpacing, radius + this.RailSpacing);
            Point circleCenter = new Point(x, y + radius);

            return new GeometryDrawing(ballastBrush, null,
                new PathGeometry(new PathFigureCollection
                {
                        new PathFigure(new Point(x, y - this.RailSpacing), new PathSegmentCollection
                        {
                            new LineSegment(new Point(x, y + this.RailSpacing), true),
                            new ArcSegment (new Point(x, y + this.RailSpacing).Rotate(angle, circleCenter), innerBallastSize, angle, false, SweepDirection.Clockwise, true),

                            new LineSegment(new Point(x, y - this.RailSpacing).Rotate(angle, circleCenter), true),
                            new ArcSegment (new Point(x, y - this.RailSpacing), outerBallastSize, angle, false, SweepDirection.Counterclockwise, true),
                        }, true)
                }));
        }

        protected Drawing CreateRightTurnoutDrawing(double length, double angle, double radius)
        {
            var railDrawing = new DrawingGroup();
                        
            double x = -length / 2.0;
            double y = 0.0;

            length = radius * 2 * Math.PI * angle / 360.0;
            int num = (int)Math.Round(length / (this.RailSpacing / 2));
            double sleepersDistance = angle / num;

            Size innerSize = new Size(radius - this.RailSpacing / 2, radius - this.RailSpacing / 2);
            Size outerSize = new Size(radius + this.RailSpacing / 2, radius + this.RailSpacing / 2);
            Point circleCenter = new Point(x, y + radius);

            railDrawing.Children.Add(new GeometryDrawing(null, railPen, new PathGeometry(new PathFigureCollection
            {
                new PathFigure(new Point(x, y - this.RailSpacing / 2), new PathSegmentCollection
                {
                    new ArcSegment (new Point(x, y - this.RailSpacing / 2).Rotate(angle, circleCenter), innerSize, angle, false, SweepDirection.Clockwise, true)
                }, false)
            })));
            railDrawing.Children.Add(new GeometryDrawing(null, railPen, new PathGeometry(new PathFigureCollection
            {
                new PathFigure(new Point(x, y + this.RailSpacing / 2), new PathSegmentCollection
                {
                    new ArcSegment (new Point(x, y + this.RailSpacing / 2).Rotate(angle, circleCenter), innerSize, angle, false, SweepDirection.Clockwise, true)
                }, false)
            })));
            for (int i = 0; i < num; i++)
            {
                double ang = 180 - (sleepersDistance / 2) - sleepersDistance * i;
                railDrawing.Children.Add(new GeometryDrawing(null, sleepersPen, new LineGeometry(
                    circleCenter + PointExtentions.Circle(ang, radius - this.sleepersSpacing / 2),
                    circleCenter + PointExtentions.Circle(ang, radius + this.sleepersSpacing / 2)
                    )));
            }
            return railDrawing;
        }

        protected Geometry CreateCrossingTrackGeometry(double length1, double length2, double angle)
        {
            return new CombinedGeometry(
                new PathGeometry(new PathFigureCollection
                {
                    new PathFigure(new Point(length1 / 2, this.RailSpacing / 2).Rotate(angle / 2), new PathSegmentCollection
                    {
                        new LineSegment(new Point(-length1 / 2,  this.RailSpacing / 2).Rotate(angle / 2), true),
                        new LineSegment(new Point(-length1 / 2, -this.RailSpacing / 2).Rotate(angle / 2), true),
                        new LineSegment(new Point( length1 / 2, -this.RailSpacing / 2).Rotate(angle / 2), true),
                        new LineSegment(new Point( length1 / 2,  this.RailSpacing / 2).Rotate(angle / 2), true),
                    }, true)
                }),
                new PathGeometry(new PathFigureCollection
                {
                    new PathFigure(new Point(length2 / 2, this.RailSpacing / 2).Rotate(-angle / 2), new PathSegmentCollection
                    {
                        new LineSegment(new Point(-length2 / 2,  this.RailSpacing / 2).Rotate(-angle / 2), true),
                        new LineSegment(new Point(-length2 / 2, -this.RailSpacing / 2).Rotate(-angle / 2), true),
                        new LineSegment(new Point( length2 / 2, -this.RailSpacing / 2).Rotate(-angle / 2), true),
                        new LineSegment(new Point( length2 / 2,  this.RailSpacing / 2).Rotate(-angle / 2), true),
                    }, true)
                }));
        }

        protected Drawing CreateCrossingBallastDrawing(double length1, double length2, double angle)
        {
            return new GeometryDrawing(ballastBrush, null,
                new CombinedGeometry(
                    new PathGeometry(new PathFigureCollection
                    {
                        new PathFigure(new Point(length1 / 2, this.RailSpacing).Rotate(angle / 2), new PathSegmentCollection
                        {
                            new LineSegment(new Point(-length1 / 2,  this.RailSpacing).Rotate(angle / 2), true),
                            new LineSegment(new Point(-length1 / 2, -this.RailSpacing).Rotate(angle / 2), true),
                            new LineSegment(new Point( length1 / 2, -this.RailSpacing).Rotate(angle / 2), true),
                            new LineSegment(new Point( length1 / 2,  this.RailSpacing).Rotate(angle / 2), true),
                        }, true)
                    }),
                    new PathGeometry(new PathFigureCollection
                    {
                        new PathFigure(new Point(length2 / 2, this.RailSpacing / 2).Rotate(-angle / 2), new PathSegmentCollection
                        {
                            new LineSegment(new Point(-length2 / 2,  this.RailSpacing).Rotate(-angle / 2), true),
                            new LineSegment(new Point(-length2 / 2, -this.RailSpacing).Rotate(-angle / 2), true),
                            new LineSegment(new Point( length2 / 2, -this.RailSpacing).Rotate(-angle / 2), true),
                            new LineSegment(new Point( length2 / 2,  this.RailSpacing).Rotate(-angle / 2), true),
                        }, true)
                    })));
        }

        protected Drawing CreateCrossingTrackDrawing(double length1, double length2, double angle)
        {
            var railDrawing = new DrawingGroup();

            railDrawing.Children.Add(new GeometryDrawing(null, railPen, new LineGeometry(new Point(-length1 / 2, -this.RailSpacing / 2).Rotate(+angle / 2), new Point(length1 / 2, -this.RailSpacing / 2).Rotate(+angle / 2))));
            railDrawing.Children.Add(new GeometryDrawing(null, railPen, new LineGeometry(new Point(-length1 / 2, +this.RailSpacing / 2).Rotate(+angle / 2), new Point(length1 / 2, +this.RailSpacing / 2).Rotate(+angle / 2))));

            railDrawing.Children.Add(new GeometryDrawing(null, railPen, new LineGeometry(new Point(-length2 / 2, -this.RailSpacing / 2).Rotate(-angle / 2), new Point(length2 / 2, -this.RailSpacing / 2).Rotate(-angle / 2))));
            railDrawing.Children.Add(new GeometryDrawing(null, railPen, new LineGeometry(new Point(-length2 / 2, +this.RailSpacing / 2).Rotate(-angle / 2), new Point(length2 / 2, +this.RailSpacing / 2).Rotate(-angle / 2))));

            int num1 = (int)Math.Round(length1 / (this.RailSpacing / 2));
            double sleepersDistance1 = length1 / num1;

            for (int i = 0; i < num1; i++)
            {
                railDrawing.Children.Add(new GeometryDrawing(null, sleepersPen, new LineGeometry(
                    new Point(-length1 / 2 + sleepersDistance1 / 2 + sleepersDistance1 * i, -this.sleepersSpacing / 2).Rotate(+angle / 2),
                    new Point(-length1 / 2 + sleepersDistance1 / 2 + sleepersDistance1 * i, +this.sleepersSpacing / 2).Rotate(+angle / 2))));
            }


            int num2 = (int)Math.Round(length2 / (this.RailSpacing / 2));
            double sleepersDistance2 = length2 / num2;
            for (int i = 0; i < num2; i++)
            {
                railDrawing.Children.Add(new GeometryDrawing(null, sleepersPen, new LineGeometry(
                    new Point(-length2 / 2 + sleepersDistance2 / 2 + sleepersDistance2 * i, -this.sleepersSpacing / 2).Rotate(-angle / 2),
                    new Point(-length2 / 2 + sleepersDistance2 / 2 + sleepersDistance2 * i, +this.sleepersSpacing / 2).Rotate(-angle / 2))));
            }
            return railDrawing;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///

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
            Direction =0x0f,
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


        //protected Drawing StraitBallast(double length)
        //{
        //    Rect rec = new Rect(-length / 2.0, -this.Spacing, +length, +this.Spacing * 2);
        //    return new GeometryDrawing(this.ballastBrush, null, new RectangleGeometry(rec));
        //}

        //protected Drawing StraitBallast(double length, Point pos)
        //{
        //    Rect rec = new Rect(-length / 2.0, -this.Spacing, +length, +this.Spacing * 2);
        //    rec.Offset((Vector)pos);
        //    return new GeometryDrawing(this.ballastBrush, null, new RectangleGeometry(rec));
        //}

        //protected Drawing StraitBallast(double length, double angle)
        //{
        //    return new GeometryDrawing(ballastBrush, null, new PathGeometry(new PathFigureCollection
        //    {
        //        new PathFigure(new Point(+length / 2, this.Spacing).Rotate(angle), new PathSegmentCollection
        //        {
        //            new LineSegment(new Point(-length / 2,  this.Spacing).Rotate(angle), true),
        //            new LineSegment(new Point(-length / 2, -this.Spacing).Rotate(angle), true),
        //            new LineSegment(new Point(+length / 2, -this.Spacing).Rotate(angle), true),
        //            new LineSegment(new Point(+length / 2,  this.Spacing).Rotate(angle), true),
        //        }, true)
        //    })); 
        //}
        //return new GeometryDrawing(ballastBrush, null, new PathGeometry(new PathFigureCollection
        //{
        //    new PathFigure(new Point(x + length, this.Spacing).Rotate(direction).Move(pos), new PathSegmentCollection
        //    {
        //        new LineSegment(new Point(x,  this.Spacing).Rotate(direction).Move(pos), true),
        //        new LineSegment(new Point(x, -this.Spacing).Rotate(direction).Move(pos), true),
        //        new LineSegment(new Point(x + length, -this.Spacing).Rotate(direction).Move(pos), true),
        //        new LineSegment(new Point(x + length,  this.Spacing).Rotate(direction).Move(pos), true),
        //    }, true)
        //}));


        //protected Drawing StraitRail(double length)
        //{
        //    int num = (int)Math.Round(length / (this.Spacing / 2));
        //    double sleepersDistance = length / num;

        //    var railDrawing = new DrawingGroup();
        //    railDrawing.Children.Add(new GeometryDrawing(null, railPen, new LineGeometry(new Point(-length / 2, -this.Spacing / 2), new Point(+length / 2, -this.Spacing / 2))));
        //    railDrawing.Children.Add(new GeometryDrawing(null, railPen, new LineGeometry(new Point(-length / 2, +this.Spacing / 2), new Point(+length / 2, +this.Spacing / 2))));
        //    for (int i = 0; i < num; i++)
        //    {
        //        double x = -length / 2 + sleepersDistance / 2 + sleepersDistance * i;
        //        double y = this.Spacing / 2 + this.sleepersOutstanding;
        //        railDrawing.Children.Add(new GeometryDrawing(null, sleepersPen, new LineGeometry(
        //            new Point(x, -y),
        //            new Point(x, +y))));
        //    }
        //    return railDrawing;

        //}

        //protected Drawing StraitRail(double length, double angle)
        //{

        //    var railDrawing = new DrawingGroup();
        //    railDrawing.Children.Add(new GeometryDrawing(null, railPen, new LineGeometry(new Point(-length / 2, -this.Spacing / 2).Rotate(angle), new Point(+length / 2, -this.Spacing / 2).Rotate(angle))));
        //    railDrawing.Children.Add(new GeometryDrawing(null, railPen, new LineGeometry(new Point(-length / 2, +this.Spacing / 2).Rotate(angle), new Point(+length / 2, +this.Spacing / 2).Rotate(angle))));

        //    int num = (int)Math.Round(length / (this.Spacing / 2));
        //    double sleepersDistance = length / num;

        //    for (int i = 0; i < num; i++)
        //    {
        //        double x = -length / 2 + sleepersDistance / 2 + sleepersDistance * i;
        //        double y = this.Spacing / 2 + this.sleepersOutstanding;
        //        railDrawing.Children.Add(new GeometryDrawing(null, sleepersPen, new LineGeometry(
        //            new Point(x, -y).Rotate(angle),
        //            new Point(x, +y).Rotate(angle))));
        //    }
        //    return railDrawing;
        //}



        //protected Drawing CurvedBallast(double angle, double radius)
        //{
        //    Size innerSize = new Size(radius - this.Spacing, radius - this.Spacing);
        //    Size outerSize = new Size(radius + this.Spacing, radius + this.Spacing);
        //    Point circleCenter = new Point(0, radius);

        //    return new GeometryDrawing(ballastBrush, null,
        //        new PathGeometry(new PathFigureCollection
        //        {
        //            new PathFigure(circleCenter - PointExtentions.Circle(-angle / 2, radius - this.Spacing), new PathSegmentCollection
        //            {
        //                new LineSegment(circleCenter - PointExtentions.Circle(-angle / 2, radius + this.Spacing), true),
        //                new ArcSegment (circleCenter - PointExtentions.Circle(+angle / 2, radius + this.Spacing), outerSize, angle, false, SweepDirection.Counterclockwise, true),

        //                new LineSegment(circleCenter - PointExtentions.Circle(+angle / 2, radius - this.Spacing), true),
        //                new ArcSegment (circleCenter - PointExtentions.Circle(-angle / 2, radius - this.Spacing), innerSize, angle, false, SweepDirection.Clockwise, true)
        //            }, true)
        //        }));
        //}



        //protected Drawing CurvedBallast(double angle, double radius, Point pos, TrackDirection dir)
        //{
        //    Size innerSize = new Size(radius - this.Spacing, radius - this.Spacing);
        //    Size outerSize = new Size(radius + this.Spacing, radius + this.Spacing);

        //    Point circleCenter;
        //    double angl;
        //    SweepDirection swdA;
        //    SweepDirection swdB;
        //    if (dir == TrackDirection.Left)
        //    { 
        //        circleCenter = pos - new Vector(0, radius);
        //        angl = -angle;
        //        swdA = SweepDirection.Counterclockwise;
        //        swdB = SweepDirection.Clockwise;
        //    }
        //    else
        //    {
        //        circleCenter = pos + new Vector(0, radius);
        //        angl = angle;
        //        swdA = SweepDirection.Clockwise;
        //        swdB = SweepDirection.Counterclockwise;
        //    }

        //    return new GeometryDrawing(ballastBrush, null,
        //        new PathGeometry(new PathFigureCollection
        //        {
        //            new PathFigure(pos - new Vector(0, this.Spacing), new PathSegmentCollection
        //                {
        //                    new LineSegment((pos + new Vector(0, this.Spacing)), true),
        //                    new ArcSegment ((pos + new Vector(0, this.Spacing)).Rotate(angl, circleCenter), innerSize, angle, false, swdA, true),

        //                    new LineSegment((pos - new Vector(0, this.Spacing)).Rotate(angl, circleCenter), true),
        //                    new ArcSegment ((pos - new Vector(0, this.Spacing)), outerSize, angle, false, swdB, true),
        //                }, true)
        //        }));
        //}

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
            Size innerSleepersSize = new Size(innerSleepersRadius, innerSleepersRadius );
                        
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
                railDrawing.Children.Add(new GeometryDrawing(null, isSelected ? railPenSelected : sleepersPen, new LineGeometry(
                    circleCenter - PointExtentions.Circle(ang, innerSleepersRadius),
                    circleCenter - PointExtentions.Circle(ang, outerSleepersRadius)
                    )));
            }
            return railDrawing;
        }
    }
}
