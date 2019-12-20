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
            this.GeometryTracks = CreateTurntableTrackGeometry(this.OuterRadius, this.InnerRadius, this.Angle, this.RailNum);

            // Tracks
            DrawingGroup drawingTracks = new DrawingGroup();
            drawingTracks.Children.Add(new GeometryDrawing(trackBrush, linePen, this.GeometryTracks));
            drawingTracks.Children.Add(this.textDrawing);
            this.drawingTracks = drawingTracks;

            // Rail
            DrawingGroup drawingRail = new DrawingGroup();
            drawingRail.Children.Add(TurntableBackground(this.OuterRadius, this.InnerRadius, this.Angle, this.RailNum));
            if (this.Ballast)
            {
               drawingRail.Children.Add(TurntableBallast(this.OuterRadius, this.InnerRadius, this.Angle, this.RailNum));
            }
            drawingRail.Children.Add(TurntableRail(this.OuterRadius, this.InnerRadius, this.Angle, this.RailNum));
            this.drawingRail = drawingRail;

            // Terrain
            this.drawingTerrain = drawingRail;

            var dockPoints = new List<TrackDockPoint>();
            for (int i = 0; i < this.RailNum; i++)
            {
                Point point = new Point(0, this.OuterRadius).Rotate(this.Angle * i);
                dockPoints.Add(new TrackDockPoint(i, point, this.Angle * i + 45, this.dockType));
            }
            this.DockPoints = dockPoints;
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
                        new PathFigure(new Point(-outerRadius, -this.RailSpacing).Rotate(angle * i), new PathSegmentCollection
                        {
                            new LineSegment(new Point(-innerRadius, -this.RailSpacing).Rotate(angle * i), true),
                            new LineSegment(new Point(-innerRadius,  this.RailSpacing).Rotate(angle * i), true),
                            new LineSegment(new Point(-outerRadius,  this.RailSpacing).Rotate(angle * i), true),
                            new LineSegment(new Point(-outerRadius, -this.RailSpacing).Rotate(angle * i), true)
                        }, true)
                    })));
            }
            drawing.Children.Add(new GeometryDrawing(this.ballastBrush, null, new RectangleGeometry(new Rect(-innerRadius, -this.RailSpacing, innerRadius * 2, this.RailSpacing * 2))));
            return drawing;
        }

        private Drawing TurntableRail(double outerRadius, double innerRadius, double angle, int railNum)
        {
            var drawing = new DrawingGroup();
            drawing.Children.Add(CreateStraitTrackDrawing(innerRadius * 2));

            for (int i = 0; i < RailNum; i++)
            {

                drawing.Children.Add(new GeometryDrawing(null, railPen, new LineGeometry(new Point(-outerRadius, -this.RailSpacing / 2).Rotate(angle * i), new Point(-innerRadius, -this.RailSpacing / 2).Rotate(angle * i))));
                drawing.Children.Add(new GeometryDrawing(null, railPen, new LineGeometry(new Point(-outerRadius, +this.RailSpacing / 2).Rotate(angle * i), new Point(-innerRadius, +this.RailSpacing / 2).Rotate(angle * i))));

                double length1 = outerRadius - innerRadius;
                int num1 = (int)Math.Round(length1 / (this.RailSpacing / 2));
                double sleepersDistance1 = length1 / num1;

                for (int j = 0; j < num1; j++)
                {
                    drawing.Children.Add(new GeometryDrawing(null, sleepersPen, new LineGeometry(
                        new Point(-outerRadius + sleepersDistance1 / 2 + sleepersDistance1 * j, -this.sleepersSpacing / 2).Rotate(angle * i),
                        new Point(-outerRadius + sleepersDistance1 / 2 + sleepersDistance1 * j, +this.sleepersSpacing / 2).Rotate(angle * i))));
                }
            }
            return drawing;
        }
    }
}
