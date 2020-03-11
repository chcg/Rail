﻿using Rail.Tracks.Properties;
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

        [XmlElement("TurnoutLength")]
        public Guid TurnoutLengthId { get; set; }

        [XmlElement("TurnoutRadius")]
        public Guid TurnoutRadiusId { get; set; }

        [XmlElement("TurnoutAngle")]
        public Guid TurnoutAngleId { get; set; }

        [XmlElement("CounterCurveRadius")]
        public Guid CounterCurveRadiusId { get; set; }

        [XmlElement("CounterCurveAngle")]
        public Guid CounterCurveAngleId { get; set; }

        [XmlElement("TurnoutDirection")]
        public TrackDirection TurnoutDirection { get; set; }

        [XmlElement("TurnoutDrive")]
        public TrackDrive TurnoutDrive { get; set; }

        #endregion

        #region internal

        [XmlIgnore, JsonIgnore]
        public double StraightLength { get; set; }

        [XmlIgnore, JsonIgnore]
        public double TurnoutLength { get; set; }        

        [XmlIgnore, JsonIgnore]
        public double TurnoutRadius { get; set; }

        [XmlIgnore, JsonIgnore]
        public double TurnoutAngle { get; set; }

        [XmlIgnore, JsonIgnore]
        public double CounterCurveRadius { get; set; }

        [XmlIgnore, JsonIgnore]
        public double CounterCurveAngle { get; set; }

        #endregion

        #region override

        [XmlIgnore, JsonIgnore]
        public override double RampLength { get { return this.StraightLength; } }

        public override void Update(TrackType trackType)
        {
            this.StraightLength = GetValue(trackType.Lengths, this.StraightLengthId);
            this.TurnoutLength = GetValueOrNull(trackType.Lengths, this.TurnoutLengthId);
            this.TurnoutRadius = GetValue(trackType.Radii, this.TurnoutRadiusId);
            this.TurnoutAngle = GetValue(trackType.Angles, this.TurnoutAngleId);
            this.CounterCurveRadius = GetValueOrNull(trackType.Radii, this.CounterCurveRadiusId);
            this.CounterCurveAngle = GetValueOrNull(trackType.Angles, this.CounterCurveAngleId);

            string turnoutRadiusName = GetName(trackType.Radii, this.TurnoutRadiusId);

            string drive = this.TurnoutDrive switch
            {
                TrackDrive.Electrical => Resources.TrackDriveElectrical,
                TrackDrive.Mechanical => Resources.TrackDriveMechanical,
                _ => string.Empty
            };
            this.Name = TurnoutDirection == TrackDirection.Left ?
                    $"{Resources.TrackTurnoutLeft} {turnoutRadiusName} {drive}" :
                    $"{Resources.TrackTurnoutRight} {turnoutRadiusName} {drive}";
            this.Description = TurnoutDirection == TrackDirection.Left ?
                    $"{this.Article} {Resources.TrackTurnoutLeft} {turnoutRadiusName} {drive}" :
                    $"{this.Article} {Resources.TrackTurnoutRight} {turnoutRadiusName} {drive}";
            
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
