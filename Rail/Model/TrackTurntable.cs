using Rail.Misc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Model
{
    public class TrackTurntable : TrackBase
    {
        [XmlAttribute("OuterRadius")]
        public double OuterRadius { get; set; }

        [XmlAttribute("InnerRadius")]
        public double InnerRadius { get; set; }

        [XmlAttribute("Angle")]
        public double Angle { get; set; }

        [XmlAttribute("RailNum")]
        public int RailNum { get; set; }

        protected override void Create()
        {
            this.Geometry = CreateTurntableTrackGeometry(this.OuterRadius, this.InnerRadius, this.Angle, this.RailNum);
            
            DrawingGroup drawing = new DrawingGroup();
            drawing.Children.Add(TurntableBackground(this.OuterRadius, this.InnerRadius, this.Angle, this.RailNum));
            if (this.Ballast)
            {
               drawing.Children.Add(TurntableBallast(this.OuterRadius, this.InnerRadius, this.Angle, this.RailNum));
            }
            drawing.Children.Add(TurntableRail(this.OuterRadius, this.InnerRadius, this.Angle, this.RailNum));
            this.RailDrawing = drawing;


            this.DockPoints = new List<TrackDockPoint>
            {
                new TrackDockPoint(0.0, 0.0, 135),
                new TrackDockPoint(0.0, 0.0, 315)
            };
        }

        private Geometry CreateTurntableTrackGeometry(double outerRadius, double innerRadius, double angle, int railNum)
        {
            return new EllipseGeometry(new Point(0, 0), outerRadius, outerRadius);
        }

        private Drawing TurntableBackground(double outerRadius, double innerRadius, double angle, int railNum)
        {
            var drawing = new DrawingGroup();
            drawing.Children.Add(new GeometryDrawing(new SolidColorBrush(Colors.DarkGray), linePen, new EllipseGeometry(new Point(0, 0), outerRadius, outerRadius)));
            drawing.Children.Add(new GeometryDrawing(new SolidColorBrush(Colors.Gray), linePen, new EllipseGeometry(new Point(0, 0), innerRadius, innerRadius)));
            return drawing;
        }

        private Drawing TurntableBallast(double outerRadius, double innerRadius, double angle, int railNum)
        {
            var drawing = new DrawingGroup();
            for (int i = 0; i < this.RailNum; i++)
            {
                drawing.Children.Add(new GeometryDrawing(this.ballastBrush, null,
                    new PathGeometry(new PathFigureCollection
                    {
                        new PathFigure(new Point(-outerRadius, -this.Spacing).Rotate(angle * i), new PathSegmentCollection
                        {
                            new LineSegment(new Point(-innerRadius, -this.Spacing).Rotate(angle * i), true),
                            new LineSegment(new Point(-innerRadius,  this.Spacing).Rotate(angle * i), true),
                            new LineSegment(new Point(-outerRadius,  this.Spacing).Rotate(angle * i), true),
                            new LineSegment(new Point(-outerRadius, -this.Spacing).Rotate(angle * i), true)
                        }, true)
                    })));
            }
            drawing.Children.Add(new GeometryDrawing(this.ballastBrush, null, new RectangleGeometry(new Rect(-innerRadius, -this.Spacing, innerRadius * 2, this.Spacing * 2))));
            return drawing;
        }

        private Drawing TurntableRail(double outerRadius, double innerRadius, double angle, int railNum)
        {
            var drawing = new DrawingGroup();
            drawing.Children.Add(CreateStraitTrackDrawing(innerRadius * 2));

            for (int i = 0; i < RailNum; i++)
            {

                drawing.Children.Add(new GeometryDrawing(null, railPen, new LineGeometry(new Point(-outerRadius, -this.Spacing / 2).Rotate(angle * i), new Point(-innerRadius, -this.Spacing / 2).Rotate(angle * i))));
                drawing.Children.Add(new GeometryDrawing(null, railPen, new LineGeometry(new Point(-outerRadius, +this.Spacing / 2).Rotate(angle * i), new Point(-innerRadius, +this.Spacing / 2).Rotate(angle * i))));

                double length1 = outerRadius - innerRadius;
                int num1 = (int)Math.Round(length1 / (this.Spacing / 2));
                double sleepersDistance1 = length1 / num1;

                for (int j = 0; j < num1; j++)
                {
                    drawing.Children.Add(new GeometryDrawing(null, sleepersPen, new LineGeometry(
                        new Point(-outerRadius + sleepersDistance1 / 2 + sleepersDistance1 * j, -this.Spacing / 2 - this.sleepersOutstanding).Rotate(angle * i),
                        new Point(-outerRadius + sleepersDistance1 / 2 + sleepersDistance1 * j, +this.Spacing / 2 + this.sleepersOutstanding).Rotate(angle * i))));
                }
            }
            return drawing;
        }
    }
}
