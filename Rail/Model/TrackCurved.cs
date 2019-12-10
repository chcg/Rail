using Rail.Misc;
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

        protected override void Create()
        {
            this.Geometry = CreateCurvedTrackGeometry(this.Angle, this.Radius);
            
            DrawingGroup drawing = new DrawingGroup();
            if (this.Ballast)
            {
                drawing.Children.Add(CurvedBallast(this.Angle, this.Radius));
            }
            drawing.Children.Add(CurvedRail(this.Angle, this.Radius));
            this.RailDrawing = drawing;

            Point circleCenter = new Point(0, this.Radius);
            this.DockPoints = new List<TrackDockPoint>
            {
                new TrackDockPoint(circleCenter - PointExtentions.Circle(-this.Angle / 2, this.Radius),  this.Angle / 2 -  45),
                new TrackDockPoint(circleCenter - PointExtentions.Circle( this.Angle / 2, this.Radius), -this.Angle / 2 + 135)
            };
        }
    }
}
