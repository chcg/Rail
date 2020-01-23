using Rail.Misc;
using Rail.Properties;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Model
{
    public class TrackYTurnout : TrackBaseSingle
    {
        [XmlAttribute("Radius")]
        public string RadiusNameOrValue { get; set; }

        [XmlIgnore]
        public double Radius { get; set; }

        [XmlIgnore]
        public string RadiusName { get; set; }

        [XmlAttribute("Angle")]
        public double Angle { get; set; }

        [XmlAttribute("Drive")]
        public TrackDrive Drive { get; set; }

        [XmlIgnore]
        public override string Name
        {
            get
            {
                string drive = this.Drive == TrackDrive.Electrical ? Resources.TrackDriveElectrical :
                              (this.Drive == TrackDrive.Mechanical ? Resources.TrackDriveMechanical : string.Empty);
                return $"{Resources.TrackYTurnout} {drive}";
            }
        }

        [XmlIgnore]
        public override string Description
        {
            get
            {
                string drive = this.Drive == TrackDrive.Electrical ? Resources.TrackDriveElectrical :
                              (this.Drive == TrackDrive.Mechanical ? Resources.TrackDriveMechanical : string.Empty);
                return $"{this.Article} {Resources.TrackYTurnout} {drive}";
            }
        }

        public override void Update(TrackType trackType)
        {
            this.Radius = GetValue(trackType.Radii, this.RadiusNameOrValue);
            this.RadiusName = GetName(this.RadiusNameOrValue);
            base.Update(trackType);
        }

        protected override Geometry CreateGeometry(double spacing)
        {
            double length = this.Radius * 2 * Math.PI * this.Angle / 360.0;

            return new CombinedGeometry(
                CurvedGeometry(this.Angle, this.Radius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, spacing, new Point(-length / 2, 0)),
                CurvedGeometry(this.Angle, this.Radius, CurvedOrientation.Clockwise | CurvedOrientation.Right, spacing, new Point(-length / 2, 0)));
        }

        protected override Drawing CreateRailDrawing()
        {
            double length = this.Radius * 2 * Math.PI * this.Angle / 360.0;

            DrawingGroup drawingRail = new DrawingGroup();
            if (this.ViewType.HasFlag(TrackViewType.Ballast))
            {
                drawingRail.Children.Add(CurvedBallast(this.Angle, this.Radius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, new Point(-length / 2, 0)));
                drawingRail.Children.Add(CurvedBallast(this.Angle, this.Radius, CurvedOrientation.Clockwise | CurvedOrientation.Right, new Point(-length / 2, 0)));
            }
            drawingRail.Children.Add(CurvedSleepers(this.Angle, this.Radius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, new Point(-length / 2, 0)));
            drawingRail.Children.Add(CurvedSleepers(this.Angle, this.Radius, CurvedOrientation.Clockwise | CurvedOrientation.Right, new Point(-length / 2, 0)));
            drawingRail.Children.Add(CurvedRail(this.Angle, this.Radius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, new Point(-length / 2, 0)));
            drawingRail.Children.Add(CurvedRail(this.Angle, this.Radius, CurvedOrientation.Clockwise | CurvedOrientation.Right, new Point(-length / 2, 0)));
            return drawingRail;
        }

        protected override List<TrackDockPoint> CreateDockPoints()
        {
            return new List<TrackDockPoint>
            {
                //new TrackDockPoint(-this.Length / 2.0, 0.0, 135),
                //new TrackDockPoint( this.Length / 2.0, 0.0, 315),
                //new TrackDockPoint(new Point(-this.Length / 2.0, 0).Rotate(-this.Angle, circleCenterLeft), -this.Angle - 45),
                //new TrackDockPoint(new Point(-this.Length / 2.0, 0).Rotate( this.Angle, circleCenterRight), this.Angle - 45)
            };
        }
    }
 
}
