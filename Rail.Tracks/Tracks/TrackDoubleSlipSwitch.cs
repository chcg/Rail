using Rail.Tracks.Properties;
using Rail.Tracks.Trigonometry;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Tracks
{
    public class TrackDoubleSlipSwitch : TrackBaseSingle
    {
        #region store

        [XmlAttribute("Length")]
        public string LengthName { get; set; }

        [XmlAttribute("CrossingAngle")]
        public string CrossingAngleName { get; set; }

        [XmlAttribute("SlipRadius")]
        public string SlipRadiusName { get; set; }

        [XmlAttribute("TurnoutDrive")]
        public TrackDrive TurnoutDrive { get; set; }

        #endregion

        #region internal

        [XmlIgnore, JsonIgnore]
        public double Length { get; set; }

        [XmlIgnore, JsonIgnore]
        public double CrossingAngle { get; set; }

        
        [XmlIgnore, JsonIgnore]
        public double SlipRadius { get; set; }

        #endregion

        #region override

        [XmlIgnore, JsonIgnore]
        public override double RampLength { get { return this.Length; } }


        [XmlIgnore, JsonIgnore]
        public override string Name
        {
            get
            {
                string drive = this.TurnoutDrive == TrackDrive.Electrical ? Resources.TrackDriveElectrical :
                              (this.TurnoutDrive == TrackDrive.Mechanical ? Resources.TrackDriveMechanical : string.Empty);
                return $"{Resources.TrackDoubleSlipSwitch} {drive}";
            }
        }

        [XmlIgnore, JsonIgnore]
        public override string Description
        {
            get
            {
                string drive = this.TurnoutDrive == TrackDrive.Electrical ? Resources.TrackDriveElectrical :
                              (this.TurnoutDrive == TrackDrive.Mechanical ? Resources.TrackDriveMechanical : string.Empty);
                return $"{this.Article} {Resources.TrackDoubleSlipSwitch} {drive}";
            }
        }

        public override void Update(TrackType trackType)
        {
            this.Length = GetValue(trackType.Lengths, this.LengthName);
            this.CrossingAngle = GetValue(trackType.Angles, this.CrossingAngleName);
            this.SlipRadius = GetValue(trackType.Radii, this.SlipRadiusName);
            base.Update(trackType);
        }

        protected override Geometry CreateGeometry()
        {
            double curveAngle = 30;
            return 
                new CombinedGeometry(
                    new CombinedGeometry(
                        StraitGeometry(this.Length, StraitOrientation.Center, -this.CrossingAngle / 2),
                        StraitGeometry(this.Length, StraitOrientation.Center, +this.CrossingAngle / 2)),
                    new CombinedGeometry(
                        CurvedGeometry(curveAngle, this.SlipRadius, CurvedOrientation.Clockwise | CurvedOrientation.Center, new Point(0, +this.TrackWidth / 2 )),
                        CurvedGeometry(curveAngle, this.SlipRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Center, new Point(0, -this.TrackWidth / 2))));

        }

        protected override Drawing CreateRailDrawing()
        {
            double curveAngle = 30;
            DrawingGroup drawingRail = new DrawingGroup();
            if (this.HasBallast)
            {
                drawingRail.Children.Add(StraitBallast(this.Length, StraitOrientation.Center, -this.CrossingAngle / 2));
                drawingRail.Children.Add(StraitBallast(this.Length, StraitOrientation.Center, +this.CrossingAngle / 2));

                drawingRail.Children.Add(CurvedBallast(curveAngle, this.SlipRadius, CurvedOrientation.Clockwise | CurvedOrientation.Center, new Point(0, +this.sleeperWidth / 2)));
                drawingRail.Children.Add(CurvedBallast(curveAngle, this.SlipRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Center, new Point(0, -this.sleeperWidth / 2)));
            }
            drawingRail.Children.Add(StraitSleepers(this.Length, StraitOrientation.Center, -this.CrossingAngle / 2));
            drawingRail.Children.Add(StraitSleepers(this.Length, StraitOrientation.Center, +this.CrossingAngle / 2));
            drawingRail.Children.Add(CurvedSleepers(curveAngle, this.SlipRadius, CurvedOrientation.Clockwise | CurvedOrientation.Center, new Point(0, +this.sleeperWidth / 2)));
            drawingRail.Children.Add(CurvedSleepers(curveAngle, this.SlipRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Center, new Point(0, -this.sleeperWidth / 2)));
            drawingRail.Children.Add(StraitRail(this.Length, StraitOrientation.Center, -this.CrossingAngle / 2));
            drawingRail.Children.Add(StraitRail(this.Length, StraitOrientation.Center, +this.CrossingAngle / 2));
            drawingRail.Children.Add(CurvedRail(curveAngle, this.SlipRadius, CurvedOrientation.Clockwise | CurvedOrientation.Center, new Point(0, +this.sleeperWidth / 2)));
            drawingRail.Children.Add(CurvedRail(curveAngle, this.SlipRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Center, new Point(0, -this.sleeperWidth / 2)));
            return drawingRail;
        }

        protected override List<TrackDockPoint> CreateDockPoints()
        {
            return new List<TrackDockPoint>
            {
                new TrackDockPoint(0, new Point(-this.Length / 2.0, 0.0).Rotate( this.CrossingAngle /2),  this.CrossingAngle /2 + 135, this.dockType),
                new TrackDockPoint(1, new Point(-this.Length / 2.0, 0.0).Rotate(-this.CrossingAngle /2), -this.CrossingAngle /2 + 135, this.dockType),
                new TrackDockPoint(2, new Point( this.Length / 2.0, 0.0).Rotate( this.CrossingAngle /2),  this.CrossingAngle /2 + 45-90, this.dockType),
                new TrackDockPoint(3, new Point( this.Length / 2.0, 0.0).Rotate(-this.CrossingAngle /2), -this.CrossingAngle /2 + 45-90, this.dockType),
            };
        }

        #endregion
    }
}
