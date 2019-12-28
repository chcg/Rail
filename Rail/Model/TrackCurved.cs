using Rail.Misc;
using Rail.Properties;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Model
{
    public class TrackCurved : TrackBase
    {
        [XmlAttribute("Radius")]
        public double Radius { get; set; }

        [XmlAttribute("Angle")]
        public double Angle { get; set; }

        [XmlIgnore]
        public override string Name
        {
            get
            {
                return $"{Resources.TrackCurved} {Radius} mm {Angle}°";
            }
        }

        protected override Geometry CreateGeometry(double spacing)
        {
            return CurvedGeometry(this.Angle, this.Radius, CurvedOrientation.Center, spacing, new Point(0, 0));
        }

        protected override Drawing CreateRailDrawing(bool isSelected)
        {
            DrawingGroup drawingRail = new DrawingGroup();
            if (this.Ballast)
            {
                drawingRail.Children.Add(CurvedBallast(this.Angle, this.Radius, CurvedOrientation.Center, new Point(0, 0)));
            }
            drawingRail.Children.Add(CurvedRail(isSelected, this.Angle, this.Radius, CurvedOrientation.Center, new Point(0, 0)));
            return drawingRail;
        }

        protected override List<TrackDockPoint> CreateDockPoints()
        {
            Point circleCenter = new Point(0, this.Radius);
            return new List<TrackDockPoint>
            {
                new TrackDockPoint(0, circleCenter - PointExtentions.Circle(-this.Angle / 2, this.Radius),  this.Angle / 2 -  45, this.dockType),
                new TrackDockPoint(1, circleCenter - PointExtentions.Circle( this.Angle / 2, this.Radius), -this.Angle / 2 + 135, this.dockType)
            };
        }
    }
}
