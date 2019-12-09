using Rail.Misc;
using System.Collections.Generic;
using System.Windows;
using System.Xml.Serialization;

namespace Rail.Model
{
    public class TrackCurved : TrackBase
    {
        [XmlAttribute("Radius")]
        public double Radius { get; set; }

        [XmlAttribute("Angle")]
        public double Angle { get; set; }

        public override void Update(double spacing)
        {
            base.Update(spacing);

            this.Geometry = CreateCurvedTrackGeometry(this.Angle, this.Radius);
            this.RailDrawing = CreateCurvedTrackDrawing(this.Angle, this.Radius);

            Point circleCenter = new Point(0, this.Radius);

            this.DockPoints = new List<TrackDockPoint>
            {
                new TrackDockPoint(circleCenter - PointExtentions.Circle(-this.Angle / 2, this.Radius),  this.Angle / 2 -  45),
                new TrackDockPoint(circleCenter - PointExtentions.Circle( this.Angle / 2, this.Radius), -this.Angle / 2 + 135)
            };
        }
    }
}
