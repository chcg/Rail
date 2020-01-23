using Rail.Misc;
using Rail.Properties;
using Rail.Trigonometry;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Model
{
    public class TrackTurnout : TrackBaseSingle
    {
        [XmlAttribute("Length")]
        public string LengthNameOrValue { get; set; }

        [XmlIgnore]
        public double Length { get; set; }

        [XmlIgnore]
        public string LengthName { get; set; }

        [XmlAttribute("Radius")]
        public string RadiusNameOrValue { get; set; }

        [XmlIgnore]
        public double Radius { get; set; }

        [XmlIgnore]
        public string RadiusName { get; set; }

        [XmlAttribute("Angle")]
        public double Angle { get; set; }

        [XmlAttribute("Direction")]
        public TrackDirection Direction { get; set; }

        [XmlAttribute("Drive")]
        public TrackDrive Drive { get; set; }
        
        [XmlIgnore]
        public override string Name
        {
            get
            {
                string drive = this.Drive == TrackDrive.Electrical ? Resources.TrackDriveElectrical :
                              (this.Drive == TrackDrive.Mechanical ? Resources.TrackDriveMechanical : string.Empty); 
                return Direction == TrackDirection.Left ? 
                    $"{Resources.TrackTurnoutLeft} {drive}" :
                    $"{Resources.TrackTurnoutRight} {drive}";
            }
        }

        [XmlIgnore]
        public override string Description
        {
            get
            {
                string drive = this.Drive == TrackDrive.Electrical ? Resources.TrackDriveElectrical :
                              (this.Drive == TrackDrive.Mechanical ? Resources.TrackDriveMechanical : string.Empty);
                return Direction == TrackDirection.Left ?
                    $"{this.Article} {Resources.TrackTurnoutLeft} {drive}" :
                    $"{this.Article} {Resources.TrackTurnoutRight} {drive}";
            }
        }

        public override void Update(TrackType trackType)
        {
            this.Length = GetValue(trackType.Lengths, this.LengthNameOrValue);
            this.LengthName = GetName(this.LengthNameOrValue);
            this.Radius = GetValue(trackType.Radii, this.RadiusNameOrValue);
            this.RadiusName = GetName(this.RadiusNameOrValue);
            base.Update(trackType);
        }

        protected override Geometry CreateGeometry(double spacing)
        {
            return new CombinedGeometry(
                StraitGeometry(this.Length, StraitOrientation.Center, spacing),
                this.Direction == TrackDirection.Left ?
                    CurvedGeometry(this.Angle, this.Radius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, spacing, new Point(-this.Length / 2, 0)) :
                    CurvedGeometry(this.Angle, this.Radius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right, spacing, new Point(this.Length / 2, 0)));
        }

        protected override Drawing CreateRailDrawing()
        {
            DrawingGroup drawingRail = new DrawingGroup();
            if (this.ViewType.HasFlag(TrackViewType.Ballast))
            {
                drawingRail.Children.Add(StraitBallast(this.Length));
                drawingRail.Children.Add(this.Direction == TrackDirection.Left ?
                    CurvedBallast(this.Angle, this.Radius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, new Point(-this.Length / 2, 0)) :
                    CurvedBallast(this.Angle, this.Radius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right, new Point(this.Length / 2, 0)));
            }
            drawingRail.Children.Add(StraitSleepers(this.Length));
            drawingRail.Children.Add(this.Direction == TrackDirection.Left ?
                    CurvedSleepers(this.Angle, this.Radius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, new Point(-this.Length / 2, 0)) :
                    CurvedSleepers(this.Angle, this.Radius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right, new Point(this.Length / 2, 0)));
            drawingRail.Children.Add(StraitRail(this.Length));
            drawingRail.Children.Add(this.Direction == TrackDirection.Left ?
                    CurvedRail(this.Angle, this.Radius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, new Point(-this.Length / 2, 0)) :
                    CurvedRail(this.Angle, this.Radius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right, new Point(this.Length / 2, 0)));
            return drawingRail;
        }

        protected override List<TrackDockPoint> CreateDockPoints()
        {
            Point circleCenterLeft = new Point(-this.Length / 2, -this.Radius);
            Point circleCenterRight = new Point(this.Length / 2, -this.Radius);

            return this.Direction == TrackDirection.Left ?
                new List<TrackDockPoint>
                {
                    new TrackDockPoint(0, new Point(-this.Length / 2.0, 0.0), 90 + 45, this.dockType),
                    new TrackDockPoint(1, new Point( this.Length / 2.0, 0.0), 180 + 90 + 45, this.dockType),
                    new TrackDockPoint(2, new Point(-this.Length / 2.0, 0.0).Rotate(-this.Angle, circleCenterLeft), -this.Angle - 45, this.dockType),
                } :
                new List<TrackDockPoint>
                {
                    new TrackDockPoint(0, new Point(-this.Length / 2.0, 0.0), 90 + 45, this.dockType),
                    new TrackDockPoint(1, new Point( this.Length / 2.0, 0.0), 180 + 90 + 45, this.dockType),
                    new TrackDockPoint(3, new Point( this.Length / 2.0, 0.0).Rotate(+this.Angle, circleCenterRight), this.Angle + 135, this.dockType)
                };
        }
    }
}
