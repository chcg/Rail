using Rail.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Model
{
    public class TrackTurnout : TrackBase
    {
        [XmlAttribute("Length")]
        public double Length { get; set; }

        [XmlAttribute("Radius")]
        public double Radius { get; set; }

        [XmlAttribute("Angle")]
        public double Angle { get; set; }

        [XmlAttribute("Direction")]
        public TrackDirection Direction { get; set; }

        protected override void Create()
        {
            this.Geometry = new CombinedGeometry(
                CreateStraitTrackGeometry(this.Length),
                CreateLeftTurnoutGeometry(this.Length, this.Angle, this.Radius));

            CurvedOrientation orientation = this.Direction == TrackDirection.Left ? CurvedOrientation.Left : CurvedOrientation.Right;

            DrawingGroup drawing = new DrawingGroup();
            if (this.Ballast)
            {
                drawing.Children.Add(StraitBallast(this.Length));
                drawing.Children.Add(CurvedBallast(this.Angle, this.Radius, orientation, 0, new Point(-this.Length / 2, 0)));
            }
            drawing.Children.Add(StraitRail(this.Length));
            drawing.Children.Add(CurvedRail(this.Angle, this.Radius, new Point(-this.Length / 2, 0), this.Direction));
            this.RailDrawing = drawing;

            if (this.Direction == TrackDirection.Left)
            {
                Point circleCenter = new Point(-this.Length / 2, -this.Radius);
                this.DockPoints = new List<TrackDockPoint>
                {
                    new TrackDockPoint(-this.Length / 2.0, 0.0, 90 + 45),
                    new TrackDockPoint( this.Length / 2.0, 0.0, 180 + 90 + 45),
                    new TrackDockPoint(new Point(-this.Length / 2.0, 0).Rotate(-this.Angle, circleCenter), -this.Angle - 45)
                };
            }
            else
            {
                Point circleCenter = new Point(-this.Length / 2, this.Radius);
                this.DockPoints = new List<TrackDockPoint>
                {
                    new TrackDockPoint(-this.Length / 2.0, 0.0, 90 + 45),
                    new TrackDockPoint( this.Length / 2.0, 0.0, 180 + 90 + 45),
                    new TrackDockPoint(new Point(-this.Length / 2.0, 0).Rotate(this.Angle, circleCenter), this.Angle - 45)
                };
            }
        }
    }
}
