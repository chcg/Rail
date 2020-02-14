using Rail.Misc;
using Rail.Properties;
using Rail.Trigonometry;
using System.Collections.Generic;
using System.Text.Json.Serialization;
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

        [XmlIgnore, JsonIgnore]
        public double Radius { get; set; }

        [XmlIgnore, JsonIgnore]
        public string RadiusName { get; set; }

        [XmlAttribute("Drive")]
        public TrackDrive Drive { get; set; }

        [XmlIgnore, JsonIgnore]
        public override string Name
        {
            get
            {
                string drive = this.Drive == TrackDrive.Electrical ? Resources.TrackDriveElectrical :
                              (this.Drive == TrackDrive.Mechanical ? Resources.TrackDriveMechanical : string.Empty);
                return $"{Resources.TrackDoubleSlipSwitch} {drive}";
            }
        }

        [XmlIgnore, JsonIgnore]
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

        protected override Geometry CreateGeometry()
        {
            double curveAngle = 30;
            return 
                new CombinedGeometry(
                    new CombinedGeometry(
                        StraitGeometry(this.Length, StraitOrientation.Center, -this.Angle / 2),
                        StraitGeometry(this.Length, StraitOrientation.Center, +this.Angle / 2)),
                    new CombinedGeometry(
                        CurvedGeometry(curveAngle, this.Radius, CurvedOrientation.Clockwise | CurvedOrientation.Center, new Point(0, +this.TrackWidth / 2 )),
                        CurvedGeometry(curveAngle, this.Radius, CurvedOrientation.Counterclockwise | CurvedOrientation.Center, new Point(0, -this.TrackWidth / 2))));

        }

        protected override Drawing CreateRailDrawing()
        {
            double curveAngle = 30;
            DrawingGroup drawingRail = new DrawingGroup();
            if (this.HasBedding)
            {
                drawingRail.Children.Add(StraitBallast(this.Length, StraitOrientation.Center, -this.Angle / 2));
                drawingRail.Children.Add(StraitBallast(this.Length, StraitOrientation.Center, +this.Angle / 2));

                drawingRail.Children.Add(CurvedBallast(curveAngle, this.Radius, CurvedOrientation.Clockwise | CurvedOrientation.Center, new Point(0, +this.sleepersWidth / 2)));
                drawingRail.Children.Add(CurvedBallast(curveAngle, this.Radius, CurvedOrientation.Counterclockwise | CurvedOrientation.Center, new Point(0, -this.sleepersWidth / 2)));
            }
            drawingRail.Children.Add(StraitSleepers(this.Length, StraitOrientation.Center, -this.Angle / 2));
            drawingRail.Children.Add(StraitSleepers(this.Length, StraitOrientation.Center, +this.Angle / 2));
            drawingRail.Children.Add(CurvedSleepers(curveAngle, this.Radius, CurvedOrientation.Clockwise | CurvedOrientation.Center, new Point(0, +this.sleepersWidth / 2)));
            drawingRail.Children.Add(CurvedSleepers(curveAngle, this.Radius, CurvedOrientation.Counterclockwise | CurvedOrientation.Center, new Point(0, -this.sleepersWidth / 2)));
            drawingRail.Children.Add(StraitRail(this.Length, StraitOrientation.Center, -this.Angle / 2));
            drawingRail.Children.Add(StraitRail(this.Length, StraitOrientation.Center, +this.Angle / 2));
            drawingRail.Children.Add(CurvedRail(curveAngle, this.Radius, CurvedOrientation.Clockwise | CurvedOrientation.Center, new Point(0, +this.sleepersWidth / 2)));
            drawingRail.Children.Add(CurvedRail(curveAngle, this.Radius, CurvedOrientation.Counterclockwise | CurvedOrientation.Center, new Point(0, -this.sleepersWidth / 2)));
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
