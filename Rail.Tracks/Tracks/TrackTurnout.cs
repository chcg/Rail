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

        [XmlElement("TurnoutType")]
        public TrackTurnoutType TurnoutType { get; set; }

        [XmlElement("TurnoutDrive")]
        public TrackDrive TurnoutDrive { get; set; }

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

        public bool ShouldSerializeTurnoutDrive() { return this.TurnoutDrive != TrackDrive.Unknown; }

        public bool ShouldSerializeStraightLengthId() { return this.TurnoutType != TrackTurnoutType.Y; }

        public bool ShouldSerializeLeftTurnoutLengthId() { return this.TurnoutType != TrackTurnoutType.Right && this.LeftTurnoutLengthId != Guid.Empty; }
        public bool ShouldSerializeLeftTurnoutRadiusId() { return this.TurnoutType != TrackTurnoutType.Right; }
        public bool ShouldSerializeLeftTurnoutAngleId() { return this.TurnoutType != TrackTurnoutType.Right; }
        public bool ShouldSerializeLeftCounterCurveRadiusId() { return this.TurnoutType != TrackTurnoutType.Right && this.LeftCounterCurveRadiusId != Guid.Empty; }
        public bool ShouldSerializeLeftCounterCurveAngleId() { return this.TurnoutType != TrackTurnoutType.Right && this.LeftCounterCurveAngleId != Guid.Empty; }

        public bool ShouldSerializeRightTurnoutLengthId() { return this.TurnoutType != TrackTurnoutType.Left && this.RightTurnoutLengthId != Guid.Empty; }
        public bool ShouldSerializeRightTurnoutRadiusId() { return this.TurnoutType != TrackTurnoutType.Left; }
        public bool ShouldSerializeRightTurnoutAngleId() { return this.TurnoutType != TrackTurnoutType.Left; }
        public bool ShouldSerializeRightCounterCurveRadiusId() { return this.TurnoutType != TrackTurnoutType.Left && this.RightCounterCurveRadiusId != Guid.Empty; }
        public bool ShouldSerializeRightCounterCurveAngleId() { return this.TurnoutType != TrackTurnoutType.Left && this.RightCounterCurveAngleId != Guid.Empty; }

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
        public override TrackTypes TrackType { get { return TrackTypes.Turnout; } }

        [XmlIgnore, JsonIgnore]
        public override double RampLength { get { return this.StraightLength; } }

        public override TrackBase Clone()
        {
            TrackTurnout track = new TrackTurnout
            {
                Article = this.Article,
                TurnoutType = this.TurnoutType,
                TurnoutDrive = this.TurnoutDrive,
                StraightLengthId = this.StraightLengthId,
                LeftTurnoutLengthId = this.LeftTurnoutLengthId,
                LeftTurnoutRadiusId = this.LeftTurnoutRadiusId,
                LeftTurnoutAngleId = this.LeftTurnoutAngleId,
                LeftCounterCurveRadiusId = this.LeftCounterCurveRadiusId,
                LeftCounterCurveAngleId = this.LeftCounterCurveAngleId,
                RightTurnoutLengthId = this.RightTurnoutLengthId,
                RightTurnoutRadiusId = this.RightTurnoutRadiusId,
                RightTurnoutAngleId = this.RightTurnoutAngleId,
                RightCounterCurveRadiusId = this.RightCounterCurveRadiusId,
                RightCounterCurveAngleId = this.RightCounterCurveAngleId
            };
            track.Update(this.trackType);
            return track;
        }
        public override void Update(TrackType trackType)
        {
            this.StraightLength = GetValueOrNull(trackType.Lengths, this.StraightLengthId);
            this.LeftTurnoutLength = GetValueOrNull(trackType.Lengths, this.LeftTurnoutLengthId);
            this.LeftTurnoutRadius = GetValueOrNull(trackType.Radii, this.LeftTurnoutRadiusId);
            this.LeftTurnoutAngle = GetValueOrNull(trackType.Angles, this.LeftTurnoutAngleId);
            this.LeftCounterCurveRadius = GetValueOrNull(trackType.Radii, this.LeftCounterCurveRadiusId);
            this.LeftCounterCurveAngle = GetValueOrNull(trackType.Angles, this.LeftCounterCurveAngleId);
            this.RightTurnoutLength = GetValueOrNull(trackType.Lengths, this.RightTurnoutLengthId);
            this.RightTurnoutRadius = GetValueOrNull(trackType.Radii, this.RightTurnoutRadiusId);
            this.RightTurnoutAngle = GetValueOrNull(trackType.Angles, this.RightTurnoutAngleId);
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

            this.Name = this.TurnoutType switch
            {
                TrackTurnoutType.Left => $"{Resources.TrackTurnoutLeft} {leftRadiusName} {drive}",
                TrackTurnoutType.Right => $"{Resources.TrackTurnoutRight} {rightRadiusName} {drive}",
                TrackTurnoutType.Y => $"{Resources.TrackYTurnout} {drive}",
                TrackTurnoutType.Three => $"{Resources.TrackThreeWayTurnout} {drive}",
                _ => string.Empty
            };
                       
            this.Description = $"{this.Article} {this.Name}";
            
            base.Update(trackType);
        }

        protected override Geometry CreateGeometry()
        {
            double leftLength = this.LeftTurnoutRadius * 2 * Math.PI * this.LeftTurnoutAngle / 360.0;
            double rightLength = this.RightTurnoutRadius * 2 * Math.PI * this.RightTurnoutAngle / 360.0;

            return this.TurnoutType switch
            {
                TrackTurnoutType.Left =>
                    new CombinedGeometry(
                        StraitGeometry(this.StraightLength, StraitOrientation.Center),
                        CurvedGeometry(this.LeftTurnoutAngle, this.LeftTurnoutRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, new Point(-this.StraightLength / 2 + this.LeftTurnoutLength, 0))), 

                TrackTurnoutType.Right =>
                    new CombinedGeometry(
                        StraitGeometry(this.StraightLength, StraitOrientation.Center),
                        CurvedGeometry(this.RightTurnoutAngle, this.RightTurnoutRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right, new Point(this.StraightLength / 2 - this.RightTurnoutLength, 0))),

                TrackTurnoutType.Y =>
                    new CombinedGeometry(
                        CurvedGeometry(this.LeftTurnoutAngle, this.LeftTurnoutRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, new Point(-leftLength / 2, 0)),
                        CurvedGeometry(this.RightTurnoutAngle, this.RightTurnoutRadius, CurvedOrientation.Clockwise | CurvedOrientation.Right, new Point(-rightLength / 2, 0))),

                TrackTurnoutType.Three =>
                    new CombinedGeometry(
                        StraitGeometry(this.StraightLength, StraitOrientation.Center),
                        new CombinedGeometry(
                            CurvedGeometry(this.LeftTurnoutAngle, this.LeftTurnoutRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, new Point(-this.StraightLength / 2, 0)),
                            CurvedGeometry(this.RightTurnoutAngle, this.RightTurnoutRadius, CurvedOrientation.Clockwise | CurvedOrientation.Right, new Point(-this.StraightLength / 2, 0)))),

                _ => null
            };
        }

        protected override Drawing CreateRailDrawing()
        {
            double leftLength = this.LeftTurnoutRadius * 2 * Math.PI * this.LeftTurnoutAngle / 360.0;
            double rightLength = this.RightTurnoutRadius * 2 * Math.PI * this.RightTurnoutAngle / 360.0;

            DrawingGroup drawingRail = new DrawingGroup();
            switch (this.TurnoutType)
            {
            case TrackTurnoutType.Left:
                if (this.HasBallast)
                {
                    drawingRail.Children.Add(StraitBallast(this.StraightLength));
                    drawingRail.Children.Add(CurvedBallast(this.LeftTurnoutAngle, this.LeftTurnoutRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, new Point(-this.StraightLength / 2 + this.LeftTurnoutLength, 0)));
                }
                drawingRail.Children.Add(StraitSleepers(this.StraightLength));
                drawingRail.Children.Add(CurvedSleepers(this.LeftTurnoutAngle, this.LeftTurnoutRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, new Point(-this.StraightLength / 2 + this.LeftTurnoutLength, 0)));
                        
                drawingRail.Children.Add(StraitRail(this.StraightLength));
                drawingRail.Children.Add(CurvedRail(this.LeftTurnoutAngle, this.LeftTurnoutRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, new Point(-this.StraightLength / 2 + this.LeftTurnoutLength, 0)));
                break;

            case TrackTurnoutType.Right:
                if (this.HasBallast)
                {
                    drawingRail.Children.Add(StraitBallast(this.StraightLength));
                    drawingRail.Children.Add(CurvedBallast(this.RightTurnoutAngle, this.RightTurnoutRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right, new Point(this.StraightLength / 2 - this.RightTurnoutLength, 0)));
                }
                drawingRail.Children.Add(StraitSleepers(this.StraightLength));
                drawingRail.Children.Add(CurvedSleepers(this.RightTurnoutAngle, this.RightTurnoutRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right, new Point(this.StraightLength / 2 - this.RightTurnoutLength, 0)));

                drawingRail.Children.Add(StraitRail(this.StraightLength));
                drawingRail.Children.Add(CurvedRail(this.RightTurnoutAngle, this.RightTurnoutRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right, new Point(this.StraightLength / 2 - this.RightTurnoutLength, 0)));
                break;

            case TrackTurnoutType.Y:
                if (this.HasBallast)
                {
                    drawingRail.Children.Add(CurvedBallast(this.LeftTurnoutAngle, this.LeftTurnoutRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, new Point(-leftLength / 2, 0)));
                    drawingRail.Children.Add(CurvedBallast(this.RightTurnoutAngle, this.RightTurnoutRadius, CurvedOrientation.Clockwise | CurvedOrientation.Right, new Point(-rightLength / 2, 0)));
                }
                drawingRail.Children.Add(CurvedSleepers(this.LeftTurnoutAngle, this.LeftTurnoutRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, new Point(-leftLength / 2, 0)));
                drawingRail.Children.Add(CurvedSleepers(this.RightTurnoutAngle, this.RightTurnoutRadius, CurvedOrientation.Clockwise | CurvedOrientation.Right, new Point(-rightLength / 2, 0)));
                
                drawingRail.Children.Add(CurvedRail(this.LeftTurnoutAngle, this.LeftTurnoutRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, new Point(-leftLength / 2, 0)));
                drawingRail.Children.Add(CurvedRail(this.RightTurnoutAngle, this.RightTurnoutRadius, CurvedOrientation.Clockwise | CurvedOrientation.Right, new Point(-rightLength / 2, 0)));
                break;

            case TrackTurnoutType.Three:
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
                break;
            }
            return drawingRail;
        }

        protected override List<TrackDockPoint> CreateDockPoints()
        {
            Point circleCenterLeft = new Point(-this.StraightLength / 2, -this.LeftTurnoutRadius);
            Point circleCenterRight = new Point(this.StraightLength / 2, -this.RightTurnoutRadius);

            return this.TurnoutType switch
            {
                TrackTurnoutType.Left =>
                    new List<TrackDockPoint>
                    {
                        new TrackDockPoint(0, new Point(-this.StraightLength / 2.0, 0.0), 90 + 45, this.dockType),
                        new TrackDockPoint(1, new Point(this.StraightLength / 2.0, 0.0), 180 + 90 + 45, this.dockType),
                        new TrackDockPoint(2, new Point(-this.StraightLength / 2.0, 0.0).Rotate(-this.LeftTurnoutAngle, circleCenterLeft).Move(this.LeftTurnoutLength, 0), -this.LeftTurnoutAngle - 45, this.dockType),
                    },

                TrackTurnoutType.Right =>
                    new List<TrackDockPoint>
                    {
                        new TrackDockPoint(0, new Point(-this.StraightLength / 2.0, 0.0), 90 + 45, this.dockType),
                        new TrackDockPoint(1, new Point(this.StraightLength / 2.0, 0.0), 180 + 90 + 45, this.dockType),
                        new TrackDockPoint(3, new Point(this.StraightLength / 2.0, 0.0).Rotate(+this.RightTurnoutAngle, circleCenterRight).Move(-this.RightTurnoutLength, 0), this.RightTurnoutAngle + 135, this.dockType)
                    },

                TrackTurnoutType.Y =>
                    new List<TrackDockPoint>
                    {
                        //new TrackDockPoint(-this.Length / 2.0, 0.0, 135),
                        //new TrackDockPoint( this.Length / 2.0, 0.0, 315),
                        //new TrackDockPoint(new Point(-this.Length / 2.0, 0).Rotate(-this.Angle, circleCenterLeft), -this.Angle - 45),
                        //new TrackDockPoint(new Point(-this.Length / 2.0, 0).Rotate( this.Angle, circleCenterRight), this.Angle - 45)
                    },

                TrackTurnoutType.Three =>
                    new List<TrackDockPoint>
                    {
                        new TrackDockPoint(0, new Point(-this.StraightLength / 2.0, 0.0), 135, this.dockType),
                        new TrackDockPoint(1, new Point(this.StraightLength / 2.0, 0.0), 315, this.dockType),
                        new TrackDockPoint(2, new Point(-this.StraightLength / 2.0, 0.0).Rotate(-this.LeftTurnoutAngle, circleCenterLeft), -this.LeftTurnoutAngle - 45, this.dockType),
                        new TrackDockPoint(3, new Point(-this.StraightLength / 2.0, 0.0).Rotate(this.RightTurnoutAngle, circleCenterRight), this.RightTurnoutAngle - 45, this.dockType)
                    },

                _ => null
            };
        }

        #endregion
    }
}
