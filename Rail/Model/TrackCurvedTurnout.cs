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
            this.Geometry = CreateRightCurvedTurnoutGeometry(this.InnerAngle, this.InnerRadius, this.OuterAngle, this.OuterRadius);

            this.DockPoints = new List<TrackDockPoint>
            {
                new TrackDockPoint(this.InnerRadius, 0, 225.0),
                new TrackDockPoint(this.InnerRadius, 0, 45.0)
            };
        }

        private Geometry CreateRightCurvedTurnoutGeometry(double innerAngle, double innerRadius, double outerAngle, double outerRadius)
        {
            Size innerInnerSize = new Size(innerRadius - this.Spacing / 2, innerRadius - this.Spacing / 2);
            Size innerOuterSize = new Size(innerRadius + this.Spacing / 2, innerRadius + this.Spacing / 2);
            Size outerInnerSize = new Size(outerRadius - this.Spacing / 2, outerRadius - this.Spacing / 2);
            Size outerOuterSize = new Size(outerRadius + this.Spacing / 2, outerRadius + this.Spacing / 2);

            Point innerCircleCenter = new Point(0, innerRadius);
            Point outerCircleCenter = new Point(0, outerRadius);


            return new CombinedGeometry(
                // inner curve
                new PathGeometry(new PathFigureCollection
                {
                    new PathFigure(innerCircleCenter - PointExtentions.Circle(-innerAngle / 2, innerRadius - this.Spacing / 2), new PathSegmentCollection
                    {
                        new LineSegment(innerCircleCenter - PointExtentions.Circle(-innerAngle / 2, innerRadius + this.Spacing / 2), true),
                        new ArcSegment (innerCircleCenter - PointExtentions.Circle(innerAngle / 2, innerRadius + this.Spacing / 2), innerOuterSize, innerAngle, false, SweepDirection.Counterclockwise, true),

                        new LineSegment(innerCircleCenter - PointExtentions.Circle(innerAngle / 2, innerRadius - this.Spacing / 2), true),
                        new ArcSegment (innerCircleCenter - PointExtentions.Circle(-innerAngle / 2, innerRadius - this.Spacing / 2), innerInnerSize, innerAngle, false, SweepDirection.Clockwise, true)
                    }, true)
                }),
                // outer curve
                new PathGeometry(new PathFigureCollection
                {
                    new PathFigure(outerCircleCenter - PointExtentions.Circle(-outerAngle / 2, outerRadius - this.Spacing / 2), new PathSegmentCollection
                    {
                        new LineSegment(outerCircleCenter - PointExtentions.Circle(-outerAngle / 2, outerRadius + this.Spacing / 2), true),
                        new ArcSegment (outerCircleCenter - PointExtentions.Circle(outerAngle / 2, outerRadius + this.Spacing / 2), outerOuterSize, outerAngle, false, SweepDirection.Counterclockwise, true),

                        new LineSegment(outerCircleCenter - PointExtentions.Circle(outerAngle / 2, outerRadius - this.Spacing / 2), true),
                        new ArcSegment (outerCircleCenter - PointExtentions.Circle(-outerAngle / 2, outerRadius - this.Spacing / 2), outerInnerSize, outerAngle, false, SweepDirection.Clockwise, true)
                    }, true)
                }));
        }
    }
}
