using Rail.Tracks.Properties;
using Rail.Tracks.Trigonometry;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Tracks
{
    public class TrackThreeWayTurnout : TrackBaseSingle
    {
        #region store 

        [XmlElement("StraightLength")]
        public Guid StraightLengthId { get; set; }

        [XmlElement("LeftTurnoutLength")]
        public Guid LeftTurnoutLengthId { get; set; }

        [XmlElement("LeftTurnoutRadius")]
        public Guid LeftTurnoutRadiusId { get; set; }

        [XmlElement("LeftTurnoutAngle")]
        public Guid LeftTurnoutAngleId { get; set; }

        [XmlElement("LeftCounterCurveRadius")]
        public Guid LeftCounterCurveRadiusId { get; set; }

        [XmlElement("LeftCounterCurveAngle")]
        public Guid LeftCounterCurveAngleId { get; set; }

        [XmlElement("RightTurnoutLength")]
        public Guid RightTurnoutLengthId { get; set; }

        [XmlElement("RightTurnoutRadius")]
        public Guid RightTurnoutRadiusId { get; set; }

        [XmlElement("RightTurnoutAngle")]
        public Guid RightTurnoutAngleId { get; set; }

        [XmlElement("RightCounterCurveRadius")]
        public Guid RightCounterCurveRadiusId { get; set; }

        [XmlElement("RightCounterCurveAngle")]
        public Guid RightCounterCurveAngleId { get; set; }

        [XmlElement("TurnoutDrive")]
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
        public double LeftCounterCurveRadius { get; set; }

        [XmlIgnore, JsonIgnore]
        public double LeftCounterCurveAngle { get; set; }

        [XmlIgnore, JsonIgnore]
        public double RightTurnoutLength { get; set; }

        [XmlIgnore, JsonIgnore]
        public double RightTurnoutRadius { get; set; }

        [XmlIgnore, JsonIgnore]
        public double RightTurnoutAngle { get; set; }

        [XmlIgnore, JsonIgnore]
        public double RightCounterCurveRadius { get; set; }

        [XmlIgnore, JsonIgnore]
        public double RightCounterCurveAngle { get; set; }

        #endregion

        #region override

        [XmlIgnore, JsonIgnore]
        public override double RampLength { get { return this.StraightLength; } }

        public override void Update(TrackType trackType)
        {
            this.StraightLength = GetValue(trackType.Lengths, this.StraightLengthId);
            this.LeftTurnoutLength = GetValueOrNull(trackType.Lengths, this.LeftTurnoutLengthId);
            this.LeftTurnoutRadius = GetValue(trackType.Radii, this.LeftTurnoutRadiusId);
            this.LeftTurnoutAngle = GetValue(trackType.Angles, this.LeftTurnoutAngleId);
            this.LeftCounterCurveRadius = GetValueOrNull(trackType.Radii, this.LeftCounterCurveRadiusId);
            this.LeftCounterCurveAngle = GetValueOrNull(trackType.Angles, this.LeftCounterCurveAngleId);
            this.RightTurnoutLength = GetValueOrNull(trackType.Lengths, this.RightTurnoutLengthId);
            this.RightTurnoutRadius = GetValue(trackType.Radii, this.RightTurnoutRadiusId);
            this.RightTurnoutAngle = GetValue(trackType.Angles, this.RightTurnoutAngleId);
            this.RightCounterCurveRadius = GetValueOrNull(trackType.Radii, this.RightCounterCurveRadiusId);
            this.RightCounterCurveAngle = GetValueOrNull(trackType.Angles, this.RightCounterCurveAngleId);

            string drive = this.TurnoutDrive switch
            {
                TrackDrive.Electrical => Resources.TrackDriveElectrical,
                TrackDrive.Mechanical => Resources.TrackDriveMechanical,
                _ => string.Empty
            }; 
            this.Name = $"{Resources.TrackThreeWayTurnout} {drive}";
            this.Description = $"{this.Article} {Resources.TrackThreeWayTurnout} {drive}";

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
