using Rail.Tracks.Properties;
using Rail.Tracks.Trigonometry;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Tracks
{
    public class TrackDoubleTurnout : TrackBaseSingle
    {
        #region store 

        [XmlAttribute("StraightLength")]
        public string StraightLengthName { get; set; }

        [XmlAttribute("LeftTurnoutLength")]
        public string LeftTurnoutLengthName { get; set; }

        [XmlAttribute("LeftTurnoutRadius")]
        public string LeftTurnoutRadiusName { get; set; }

        [XmlAttribute("LeftTurnoutAngle")]
        public string LeftTurnoutAngleName { get; set; }

        [XmlAttribute("RightTurnoutLength")]
        public string RightTurnoutLengthName { get; set; }

        [XmlAttribute("RightTurnoutRadius")]
        public string RightTurnoutRadiusName { get; set; }

        [XmlAttribute("RightTurnoutAngle")]
        public string RightTurnoutAngleName { get; set; }

        [XmlAttribute("TurnoutDrive")]
        public TrackDrive TurnoutDrive { get; set; }

        #endregion

        #region internal

        [XmlIgnore, JsonIgnore]
        public double StraightLength { get; set; }

        [XmlIgnore, JsonIgnore]
        public double LeftTurnoutLength { get; set; }

        [XmlIgnore, JsonIgnore]
        public double LeftTurnoutRadius { get; set; }

        [XmlIgnore, JsonIgnore]
        public double LeftTurnoutAngle { get; set; }

        [XmlIgnore, JsonIgnore]
        public double RightTurnoutLength { get; set; }

        [XmlIgnore, JsonIgnore]
        public double RightTurnoutRadius { get; set; }

        [XmlIgnore, JsonIgnore]
        public double RightTurnoutAngle { get; set; }

        #endregion

        #region override

        [XmlIgnore, JsonIgnore]
        public override double RampLength { get { return this.StraightLength; } }

        [XmlIgnore, JsonIgnore]
        public override string Name
        {
            get
            {
                string drive = this.TurnoutDrive == TrackDrive.Electrical ? Resources.TrackDriveElectrical :
                              (this.TurnoutDrive == TrackDrive.Mechanical ? Resources.TrackDriveMechanical : string.Empty);
                return $"{Resources.TrackDoubleTurnout} {drive}";
            }
        }

        [XmlIgnore, JsonIgnore]
        public override string Description
        {
            get
            {
                string drive = this.TurnoutDrive == TrackDrive.Electrical ? Resources.TrackDriveElectrical :
                              (this.TurnoutDrive == TrackDrive.Mechanical ? Resources.TrackDriveMechanical : string.Empty);
                return $"{this.Article} {Resources.TrackDoubleTurnout} {drive}";
            }
        }

        public override void Update(TrackType trackType)
        {
            this.StraightLength = GetValue(trackType.Lengths, this.StraightLengthName);
            this.LeftTurnoutLength = GetValue(trackType.Lengths, this.LeftTurnoutLengthName);
            this.LeftTurnoutRadius = GetValue(trackType.Radii, this.LeftTurnoutRadiusName);
            this.LeftTurnoutAngle = GetValue(trackType.Angles, this.LeftTurnoutAngleName);
            this.RightTurnoutLength = GetValue(trackType.Lengths, this.RightTurnoutLengthName);
            this.RightTurnoutRadius = GetValue(trackType.Radii, this.RightTurnoutRadiusName);
            this.RightTurnoutAngle = GetValue(trackType.Angles, this.RightTurnoutAngleName);

            base.Update(trackType);
        }


        protected override Geometry CreateGeometry()
        {
            return new CombinedGeometry(
               StraitGeometry(this.StraightLength, StraitOrientation.Center),
               new CombinedGeometry(
                    CurvedGeometry(this.LeftTurnoutAngle, this.LeftTurnoutRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, new Point(-this.StraightLength / 2, 0)),
                    CurvedGeometry(this.RightTurnoutAngle, this.RightTurnoutRadius, CurvedOrientation.Clockwise | CurvedOrientation.Right, new Point(-this.StraightLength / 2, 0))));
        }

        protected override Drawing CreateRailDrawing()
        {
            DrawingGroup drawingRail = new DrawingGroup();
            if (this.HasBallast)
            {
                drawingRail.Children.Add(StraitBallast(this.StraightLength));
                drawingRail.Children.Add(CurvedBallast(this.LeftTurnoutAngle, this.LeftTurnoutRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, new Point(-this.StraightLength / 2, 0)));
                drawingRail.Children.Add(CurvedBallast(this.RightTurnoutAngle, this.RightTurnoutRadius, CurvedOrientation.Clockwise | CurvedOrientation.Right, new Point(-this.StraightLength / 2, 0)));
            }
            drawingRail.Children.Add(StraitSleepers(this.StraightLength));
            drawingRail.Children.Add(CurvedSleepers(this.LeftTurnoutAngle, this.LeftTurnoutRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, new Point(-this.StraightLength / 2, 0)));
            drawingRail.Children.Add(CurvedSleepers(this.RightTurnoutAngle, this.RightTurnoutRadius, CurvedOrientation.Clockwise | CurvedOrientation.Right, new Point(-this.StraightLength / 2, 0)));
            drawingRail.Children.Add(StraitRail(this.StraightLength));
            drawingRail.Children.Add(CurvedRail(this.LeftTurnoutAngle, this.LeftTurnoutRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, new Point(-this.StraightLength / 2, 0)));
            drawingRail.Children.Add(CurvedRail(this.RightTurnoutAngle, this.RightTurnoutRadius, CurvedOrientation.Clockwise | CurvedOrientation.Right, new Point(-this.StraightLength / 2, 0)));
            return drawingRail;
        }

        protected override List<TrackDockPoint> CreateDockPoints()
        {
            Point circleCenterLeft = new Point(-this.StraightLength / 2, -this.LeftTurnoutRadius);
            Point circleCenterRight = new Point(-this.StraightLength / 2, this.RightTurnoutRadius);
            return new List<TrackDockPoint>
            {
                new TrackDockPoint(0, new Point(-this.StraightLength / 2.0, 0.0), 135, this.dockType),
                new TrackDockPoint(1, new Point( this.StraightLength / 2.0, 0.0), 315, this.dockType),
                new TrackDockPoint(2, new Point(-this.StraightLength / 2.0, 0.0).Rotate(-this.LeftTurnoutAngle, circleCenterLeft), -this.LeftTurnoutAngle - 45, this.dockType),
                new TrackDockPoint(3, new Point(-this.StraightLength / 2.0, 0.0).Rotate( this.RightTurnoutAngle, circleCenterRight), this.RightTurnoutAngle - 45, this.dockType)
            };
        }

        #endregion
    }
}
