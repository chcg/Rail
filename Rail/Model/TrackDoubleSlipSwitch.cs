using Rail.Misc;
using Rail.Properties;
using Rail.Trigonometry;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Model
{
    public class TrackDoubleSlipSwitch : TrackStraight
    {
        [XmlAttribute("Angle")]
        public double Angle { get; set; }

        [XmlAttribute("Radius")]
        public string RadiusNameOrValue { get; set; }

        [XmlIgnore]
        public double Radius { get; set; }

        [XmlIgnore]
        public string RadiusName { get; set; }

        [XmlAttribute("Drive")]
        public TrackDrive Drive { get; set; }

        [XmlIgnore]
        public override string Name
        {
            get
            {
                string drive = this.Drive == TrackDrive.Electrical ? Resources.TrackDriveElectrical :
                              (this.Drive == TrackDrive.Mechanical ? Resources.TrackDriveMechanical : string.Empty);
                return $"{Resources.TrackDoubleSlipSwitch} {drive}";
            }
        }

        [XmlIgnore]
        public override string Description
        {
            get
            {
                string drive = this.Drive == TrackDrive.Electrical ? Resources.TrackDriveElectrical :
                              (this.Drive == TrackDrive.Mechanical ? Resources.TrackDriveMechanical : string.Empty);
                return $"{this.Article} {Resources.TrackDoubleSlipSwitch} {drive}";
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
            double curveAngle = 30;
            return 
                new CombinedGeometry(
                    new CombinedGeometry(
                        StraitGeometry(this.Length, StraitOrientation.Center, spacing, -this.Angle / 2),
                        StraitGeometry(this.Length, StraitOrientation.Center, spacing, +this.Angle / 2)),
                    new CombinedGeometry(
                        CurvedGeometry(curveAngle, this.Radius, CurvedOrientation.Clockwise | CurvedOrientation.Center, spacing, new Point(0, +spacing / 2 )),
                        CurvedGeometry(curveAngle, this.Radius, CurvedOrientation.Counterclockwise | CurvedOrientation.Center, spacing, new Point(0, -spacing / 2))));

        }

        protected override Drawing CreateRailDrawing(bool isSelected)
        {
            double curveAngle = 30;
            DrawingGroup drawingRail = new DrawingGroup();
            if (this.ViewType.HasFlag(TrackViewType.Ballast))
            {
                drawingRail.Children.Add(StraitBallast(this.Length, StraitOrientation.Center, -this.Angle / 2));
                drawingRail.Children.Add(StraitBallast(this.Length, StraitOrientation.Center, +this.Angle / 2));

                drawingRail.Children.Add(CurvedBallast(curveAngle, this.Radius, CurvedOrientation.Clockwise | CurvedOrientation.Center, new Point(0, +this.sleepersWidth / 2)));
                drawingRail.Children.Add(CurvedBallast(curveAngle, this.Radius, CurvedOrientation.Counterclockwise | CurvedOrientation.Center, new Point(0, -this.sleepersWidth / 2)));
            }
            drawingRail.Children.Add(StraitSleepers(isSelected, this.Length, StraitOrientation.Center, -this.Angle / 2));
            drawingRail.Children.Add(StraitSleepers(isSelected, this.Length, StraitOrientation.Center, +this.Angle / 2));
            drawingRail.Children.Add(CurvedSleepers(isSelected, curveAngle, this.Radius, CurvedOrientation.Clockwise | CurvedOrientation.Center, new Point(0, +this.sleepersWidth / 2)));
            drawingRail.Children.Add(CurvedSleepers(isSelected, curveAngle, this.Radius, CurvedOrientation.Counterclockwise | CurvedOrientation.Center, new Point(0, -this.sleepersWidth / 2)));
            drawingRail.Children.Add(StraitRail(isSelected, this.Length, StraitOrientation.Center, -this.Angle / 2));
            drawingRail.Children.Add(StraitRail(isSelected, this.Length, StraitOrientation.Center, +this.Angle / 2));
            drawingRail.Children.Add(CurvedRail(isSelected, curveAngle, this.Radius, CurvedOrientation.Clockwise | CurvedOrientation.Center, new Point(0, +this.sleepersWidth / 2)));
            drawingRail.Children.Add(CurvedRail(isSelected, curveAngle, this.Radius, CurvedOrientation.Counterclockwise | CurvedOrientation.Center, new Point(0, -this.sleepersWidth / 2)));
            return drawingRail;
        }

        protected override List<TrackDockPoint> CreateDockPoints()
        {
            return new List<TrackDockPoint>
            {
                new TrackDockPoint(0, new Point(-this.Length / 2.0, 0.0).Rotate( this.Angle /2),  this.Angle /2 + 135, this.dockType),
                new TrackDockPoint(1, new Point(-this.Length / 2.0, 0.0).Rotate(-this.Angle /2), -this.Angle /2 + 135, this.dockType),
                new TrackDockPoint(2, new Point( this.Length / 2.0, 0.0).Rotate( this.Angle /2),  this.Angle /2 + 45-90, this.dockType),
                new TrackDockPoint(3, new Point( this.Length / 2.0, 0.0).Rotate(-this.Angle /2), -this.Angle /2 + 45-90, this.dockType),
            };
        }

    }
}
