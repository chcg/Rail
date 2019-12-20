using Rail.Misc;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Model
{
    public class TrackCurvedTurnout : TrackBase
    {
        [XmlAttribute("InnerRadius")]
        public double InnerRadius { get; set; }

        [XmlAttribute("InnerAngle")]
        public double InnerAngle { get; set; }

        [XmlAttribute("OuterRadius")]
        public double OuterRadius { get; set; }

        [XmlAttribute("OuterAngle")]
        public double OuterAngle { get; set; }

        [XmlAttribute("Direction")]
        public TrackDirection Direction { get; set; }

        protected override void Create()
        {
            this.GeometryTracks = CreateRightCurvedTurnoutGeometry(this.InnerAngle, this.InnerRadius, this.OuterAngle, this.OuterRadius);

            // Tracks
            DrawingGroup drawingTracks = new DrawingGroup();
            drawingTracks.Children.Add(new GeometryDrawing(trackBrush, linePen, this.GeometryTracks));
            drawingTracks.Children.Add(this.textDrawing);
            this.drawingTracks = drawingTracks;

            // Rail
            DrawingGroup drawingRail = new DrawingGroup();
            if (this.Ballast)
            {
                //drawingRail.Children.Add(StraitBallast(this.Length, StraitOrientation.Center, 0, null));
            }
            //drawingRail.Children.Add(StraitRail(this.Length));
            this.drawingRail = drawingRail;

            // Terrain
            this.drawingTerrain = drawingRail;

            this.DockPoints = new List<TrackDockPoint>
            {
                new TrackDockPoint(0, new Point(this.InnerRadius, 0), 225.0, this.dockType),
                new TrackDockPoint(1, new Point(this.InnerRadius, 0), 45.0, this.dockType)
            };
        }

        private Geometry CreateRightCurvedTurnoutGeometry(double innerAngle, double innerRadius, double outerAngle, double outerRadius)
        {
            Size innerInnerSize = new Size(innerRadius - this.RailSpacing / 2, innerRadius - this.RailSpacing / 2);
            Size innerOuterSize = new Size(innerRadius + this.RailSpacing / 2, innerRadius + this.RailSpacing / 2);
            Size outerInnerSize = new Size(outerRadius - this.RailSpacing / 2, outerRadius - this.RailSpacing / 2);
            Size outerOuterSize = new Size(outerRadius + this.RailSpacing / 2, outerRadius + this.RailSpacing / 2);

            Point innerCircleCenter = new Point(0, innerRadius);
            Point outerCircleCenter = new Point(0, outerRadius);


            return new CombinedGeometry(
                // inner curve
                new PathGeometry(new PathFigureCollection
                {
                    new PathFigure(innerCircleCenter - PointExtentions.Circle(-innerAngle / 2, innerRadius - this.RailSpacing / 2), new PathSegmentCollection
                    {
                        new LineSegment(innerCircleCenter - PointExtentions.Circle(-innerAngle / 2, innerRadius + this.RailSpacing / 2), true),
                        new ArcSegment (innerCircleCenter - PointExtentions.Circle(innerAngle / 2, innerRadius + this.RailSpacing / 2), innerOuterSize, innerAngle, false, SweepDirection.Counterclockwise, true),

                        new LineSegment(innerCircleCenter - PointExtentions.Circle(innerAngle / 2, innerRadius - this.RailSpacing / 2), true),
                        new ArcSegment (innerCircleCenter - PointExtentions.Circle(-innerAngle / 2, innerRadius - this.RailSpacing / 2), innerInnerSize, innerAngle, false, SweepDirection.Clockwise, true)
                    }, true)
                }),
                // outer curve
                new PathGeometry(new PathFigureCollection
                {
                    new PathFigure(outerCircleCenter - PointExtentions.Circle(-outerAngle / 2, outerRadius - this.RailSpacing / 2), new PathSegmentCollection
                    {
                        new LineSegment(outerCircleCenter - PointExtentions.Circle(-outerAngle / 2, outerRadius + this.RailSpacing / 2), true),
                        new ArcSegment (outerCircleCenter - PointExtentions.Circle(outerAngle / 2, outerRadius + this.RailSpacing / 2), outerOuterSize, outerAngle, false, SweepDirection.Counterclockwise, true),

                        new LineSegment(outerCircleCenter - PointExtentions.Circle(outerAngle / 2, outerRadius - this.RailSpacing / 2), true),
                        new ArcSegment (outerCircleCenter - PointExtentions.Circle(-outerAngle / 2, outerRadius - this.RailSpacing / 2), outerInnerSize, outerAngle, false, SweepDirection.Clockwise, true)
                    }, true)
                }));
        }
    }
}
