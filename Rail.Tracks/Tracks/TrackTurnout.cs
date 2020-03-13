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
    public class TrackTurnout : TrackBaseSingle
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

        [XmlElement("TurnoutDirection")]
        public TrackTurnoutDirection TurnoutDirection { get; set; }

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

            string leftRadiusName = GetName(trackType.Radii, this.LeftTurnoutRadiusId);
            string rightRadiusName = GetName(trackType.Radii, this.RightTurnoutRadiusId);

            this.Name = this.TurnoutDirection switch
            {
                TrackTurnoutDirection.Left => $"{Resources.TrackTurnoutLeft} {leftRadiusName} {drive}",
                TrackTurnoutDirection.Right => $"{Resources.TrackTurnoutRight} {rightRadiusName} {drive}",
                TrackTurnoutDirection.Y => $"{Resources.TrackYTurnout} {drive}",
                TrackTurnoutDirection.Three => $"{Resources.TrackThreeWayTurnout} {drive}",
                _ => string.Empty
            };
                       
            this.Description = $"{this.Article} {this.Name}";
            
            base.Update(trackType);
        }

        protected override Geometry CreateGeometry()
        {
            return new CombinedGeometry(
                StraitGeometry(this.StraightLength, StraitOrientation.Center),
                this.TurnoutDirection == TrackDirection.Left ?
                    CurvedGeometry(this.TurnoutAngle, this.TurnoutRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, new Point(-this.StraightLength / 2 + this.TurnoutLength, 0)) :
                    CurvedGeometry(this.TurnoutAngle, this.TurnoutRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right, new Point(this.StraightLength / 2 - this.TurnoutLength, 0)));
        }

        protected override Drawing CreateRailDrawing()
        {
            DrawingGroup drawingRail = new DrawingGroup();
            if (this.HasBallast)
            {
                drawingRail.Children.Add(StraitBallast(this.StraightLength));
                drawingRail.Children.Add(this.TurnoutDirection == TrackDirection.Left ?
                    CurvedBallast(this.TurnoutAngle, this.TurnoutRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, new Point(-this.StraightLength / 2 + this.TurnoutLength, 0)) :
                    CurvedBallast(this.TurnoutAngle, this.TurnoutRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right, new Point(this.StraightLength / 2 - this.TurnoutLength, 0)));
            }
            drawingRail.Children.Add(StraitSleepers(this.StraightLength));
            drawingRail.Children.Add(this.TurnoutDirection == TrackDirection.Left ?
                    CurvedSleepers(this.TurnoutAngle, this.TurnoutRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, new Point(-this.StraightLength / 2 + this.TurnoutLength, 0)) :
                    CurvedSleepers(this.TurnoutAngle, this.TurnoutRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right, new Point(this.StraightLength / 2 - this.TurnoutLength, 0)));
            drawingRail.Children.Add(StraitRail(this.StraightLength));
            drawingRail.Children.Add(this.TurnoutDirection == TrackDirection.Left ?
                    CurvedRail(this.TurnoutAngle, this.TurnoutRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, new Point(-this.StraightLength / 2 + this.TurnoutLength, 0)) :
                    CurvedRail(this.TurnoutAngle, this.TurnoutRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right, new Point(this.StraightLength / 2 - this.TurnoutLength, 0)));
            return drawingRail;
        }

        protected override List<TrackDockPoint> CreateDockPoints()
        {
            Point circleCenterLeft = new Point(-this.StraightLength / 2, -this.TurnoutRadius);
            Point circleCenterRight = new Point(this.StraightLength / 2, -this.TurnoutRadius);

            return this.TurnoutDirection == TrackDirection.Left ?
                new List<TrackDockPoint>
                {
                    new TrackDockPoint(0, new Point(-this.StraightLength / 2.0, 0.0), 90 + 45, this.dockType),
                    new TrackDockPoint(1, new Point( this.StraightLength / 2.0, 0.0), 180 + 90 + 45, this.dockType),
                    new TrackDockPoint(2, new Point(-this.StraightLength / 2.0, 0.0).Rotate(-this.TurnoutAngle, circleCenterLeft).Move(this.TurnoutLength, 0), -this.TurnoutAngle - 45, this.dockType),
                } :
                new List<TrackDockPoint>
                {
                    new TrackDockPoint(0, new Point(-this.StraightLength / 2.0, 0.0), 90 + 45, this.dockType),
                    new TrackDockPoint(1, new Point( this.StraightLength / 2.0, 0.0), 180 + 90 + 45, this.dockType),
                    new TrackDockPoint(3, new Point( this.StraightLength / 2.0, 0.0).Rotate(+this.TurnoutAngle, circleCenterRight).Move(-this.TurnoutLength, 0), this.TurnoutAngle + 135, this.dockType)
                };
        }

        #endregion
    }
}
