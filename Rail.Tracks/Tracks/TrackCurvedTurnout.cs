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
    public class TrackCurvedTurnout : TrackBaseSingle
    {
        #region store

        [XmlElement("InnerLength")]
        public Guid InnerLengthId { get; set; }

        [XmlElement("InnerRadius")]
        public Guid InnerRadiusId { get; set; }

        [XmlElement("InnerAngle")]
        public Guid InnerAngleId { get; set; }

        [XmlElement("OuterLength")]
        public Guid OuterLengthId { get; set; }

        [XmlElement("OuterRadius")]
        public Guid OuterRadiusId { get; set; }

        [XmlElement("OuterAngle")]
        public Guid OuterAngleId { get; set; }

        [XmlElement("TurnoutDirection")]
        public TrackDirection TurnoutDirection { get; set; }

        [XmlElement("TurnoutDrive")]
        public TrackDrive TurnoutDrive { get; set; }

        #endregion

        #region internal

        [XmlIgnore, JsonIgnore]
        public double InnerLength { get; set; }

        [XmlIgnore, JsonIgnore]
        public double InnerRadius { get; set; }

        [XmlIgnore, JsonIgnore]
        public double InnerAngle { get; set; }

        [XmlIgnore, JsonIgnore]
        public double OuterLength { get; set; }

        [XmlIgnore, JsonIgnore]
        public double OuterRadius { get; set; }

        [XmlIgnore, JsonIgnore]
        public double OuterAngle { get; set; }

        #endregion

        #region override

        [XmlIgnore, JsonIgnore]
        public override double RampLength { get { return this.OuterLength; } }
        
        public override void Update(TrackType trackType)
        {
            this.InnerLength = GetValueOrNull(trackType.Lengths, this.InnerLengthId);
            this.InnerRadius = GetValue(trackType.Radii, this.InnerRadiusId);
            this.InnerAngle = GetValue(trackType.Angles, this.InnerAngleId);
            this.OuterLength = GetValueOrNull(trackType.Lengths, this.OuterLengthId);
            this.OuterRadius = GetValue(trackType.Radii, this.OuterRadiusId);
            this.OuterAngle = GetValue(trackType.Angles, this.OuterAngleId);

            string drive = this.TurnoutDrive switch
            {
                TrackDrive.Electrical => Resources.TrackDriveElectrical,
                TrackDrive.Mechanical => Resources.TrackDriveMechanical,
                _ => string.Empty
            };
            this.Name = TurnoutDirection == TrackDirection.Left ?
                    $"{Resources.TrackCurvedTurnoutLeft} {drive}" :
                    $"{Resources.TrackCurvedTurnoutRight} {drive}";
            this. Description = TurnoutDirection == TrackDirection.Left ?
                    $"{this.Article} {Resources.TrackCurvedTurnoutLeft} {drive}" :
                    $"{this.Article} {Resources.TrackCurvedTurnoutRight} {drive}";

            base.Update(trackType);
        }

        protected override Geometry CreateGeometry()
        {
            double maxLength = Math.Max(this.InnerLength, this.OuterLength);
            double width = (this.OuterRadius * 2 * Math.PI * this.OuterAngle / 360.0 + maxLength) / 2;
            //double hight = this.OuterRadius * 2 * Math.PI * (this.OuterAngle - 90) / 360.0;

            //Point centerLeft = CurveCenter(this.OuterAngle, this.OuterRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left) + new Vector(this.Length / 2, 0);
            //Point centerRight = CurveCenter(this.OuterAngle, this.OuterRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right) + new Vector(this.Length / 2, 0);

            Point centerLeft = new Point(-width, 0);
            Point centerRight = new Point(width, 0);

            return this.TurnoutDirection == TrackDirection.Left ?
                new CombinedGeometry(
                    StraitGeometry(maxLength, StraitOrientation.Left, 0, centerLeft),
                    new CombinedGeometry(
                        CurvedGeometry(this.InnerAngle, this.InnerRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, centerLeft + new Vector(this.InnerLength, 0)),
                        CurvedGeometry(this.OuterAngle, this.OuterRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, centerLeft + new Vector(this.OuterLength, 0)))
                    ) :
                new CombinedGeometry(
                    StraitGeometry(maxLength, StraitOrientation.Right, 0, centerRight),
                    new CombinedGeometry(
                        CurvedGeometry(this.InnerAngle, this.InnerRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right, centerRight - new Vector(this.InnerLength, 0)),
                        CurvedGeometry(this.OuterAngle, this.OuterRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right, centerRight - new Vector(this.OuterLength, 0)))
                    );
        }

        protected override Drawing CreateRailDrawing()
        {
            double maxLength = Math.Max(this.InnerLength, this.OuterLength);
            double width = (this.OuterRadius * 2 * Math.PI * this.OuterAngle / 360.0 + maxLength) / 2;
            //double hight = this.OuterRadius * 2 * Math.PI * (this.OuterAngle - 90) / 360.0;

            //Point centerLeft = CurveCenter(this.OuterAngle, this.OuterRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left) + new Vector(this.Length / 2, 0);
            //Point centerRight = CurveCenter(this.OuterAngle, this.OuterRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right) + new Vector(this.Length / 2, 0);

            Point centerLeft = new Point(-width, 0);
            Point centerRight = new Point(width, 0);

            DrawingGroup drawingRail = new DrawingGroup();
            if (this.HasBallast)
            {
                if (this.TurnoutDirection == TrackDirection.Left)
                {
                    drawingRail.Children.Add(StraitBallast(maxLength, StraitOrientation.Left, 0, centerLeft));
                    drawingRail.Children.Add(CurvedBallast(this.InnerAngle, this.InnerRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, centerLeft + new Vector(this.InnerLength, 0)));
                    drawingRail.Children.Add(CurvedBallast(this.OuterAngle, this.OuterRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, centerLeft + new Vector(this.OuterLength, 0)));
                }
                else
                {
                    drawingRail.Children.Add(StraitBallast(maxLength, StraitOrientation.Right, 0, centerRight));
                    drawingRail.Children.Add(CurvedBallast(this.InnerAngle, this.InnerRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right, centerRight - new Vector(this.InnerLength, 0)));
                    drawingRail.Children.Add(CurvedBallast(this.OuterAngle, this.OuterRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right, centerRight - new Vector(this.OuterLength, 0)));
                }
            }
            if (this.TurnoutDirection == TrackDirection.Left)
            {
                drawingRail.Children.Add(StraitSleepers(maxLength, StraitOrientation.Left, 0, centerLeft));
                drawingRail.Children.Add(CurvedSleepers(this.InnerAngle, this.InnerRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, centerLeft + new Vector(this.InnerLength, 0)));
                drawingRail.Children.Add(CurvedSleepers(this.OuterAngle, this.OuterRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, centerLeft + new Vector(this.OuterLength, 0)));
                drawingRail.Children.Add(StraitRail(maxLength, StraitOrientation.Left, 0, centerLeft));
                drawingRail.Children.Add(CurvedRail(this.InnerAngle, this.InnerRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, centerLeft + new Vector(this.InnerLength, 0)));
                drawingRail.Children.Add(CurvedRail(this.OuterAngle, this.OuterRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, centerLeft + new Vector(this.OuterLength, 0)));
            }
            else
            {
                drawingRail.Children.Add(StraitSleepers(maxLength, StraitOrientation.Right, 0, centerRight));
                drawingRail.Children.Add(CurvedSleepers(this.InnerAngle, this.InnerRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right, centerRight - new Vector(this.InnerLength, 0)));
                drawingRail.Children.Add(CurvedSleepers(this.OuterAngle, this.OuterRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right, centerRight - new Vector(this.OuterLength, 0)));
                drawingRail.Children.Add(StraitRail(maxLength, StraitOrientation.Right, 0, centerRight));
                drawingRail.Children.Add(CurvedRail(this.InnerAngle, this.InnerRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right, centerRight - new Vector(this.InnerLength, 0)));
                drawingRail.Children.Add(CurvedRail(this.OuterAngle, this.OuterRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right, centerRight - new Vector(this.OuterLength, 0)));
            }
            return drawingRail;
        }

        protected override List<TrackDockPoint> CreateDockPoints()
        {
            double maxLength = Math.Max(this.InnerLength, this.OuterLength);

            double innerWidth = (this.InnerRadius * 2 * Math.PI * this.InnerAngle / 360.0 + maxLength) / 2;
            double outerWidth = (this.OuterRadius * 2 * Math.PI * this.OuterAngle / 360.0 + maxLength) / 2;
            //double hight = this.OuterRadius * 2 * Math.PI * (this.OuterAngle - 90) / 360.0;

            //Point centerLeft = CurveCenter(this.OuterAngle, this.OuterRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left) + new Vector(this.Length / 2, 0);
            //Point centerRight = CurveCenter(this.OuterAngle, this.OuterRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right) + new Vector(this.Length / 2, 0);

            Point innerCircleCenterLeft = new Point(-innerWidth + InnerLength, -this.InnerRadius);
            Point outerCircleCenterLeft = new Point(-outerWidth + OuterLength, -this.OuterRadius);

            Point innerCircleCenterRight = new Point(innerWidth - InnerLength, -this.InnerRadius);
            Point outerCircleCenterRight = new Point(outerWidth - OuterLength, -this.OuterRadius);

            return this.TurnoutDirection == TrackDirection.Left ? 
                new List<TrackDockPoint>
                {
                    new TrackDockPoint(0, new Point(-outerWidth, 0.0), 90 + 45, this.dockType),
                    new TrackDockPoint(1, new Point(-innerWidth + this.InnerLength, 0.0).Rotate(-this.InnerAngle, innerCircleCenterLeft), -this.InnerAngle -45, this.dockType),
                    new TrackDockPoint(2, new Point(-outerWidth + this.OuterLength, 0.0).Rotate(-this.OuterAngle, outerCircleCenterLeft), -this.OuterAngle -45, this.dockType),
                } :
                new List<TrackDockPoint>
                {
                    new TrackDockPoint(0, new Point(+outerWidth, 0.0), 90 + 45 + 180, this.dockType),
                    new TrackDockPoint(1, new Point(+innerWidth - this.InnerLength, 0.0).Rotate(this.InnerAngle, innerCircleCenterRight), this.InnerAngle + 135, this.dockType),
                    new TrackDockPoint(2, new Point(+outerWidth - this.OuterLength, 0.0).Rotate(this.OuterAngle, outerCircleCenterRight), this.OuterAngle + 135, this.dockType),

                    //new TrackDockPoint(0, new Point(this.InnerRadius, 0), 225.0, this.dockType),
                    //new TrackDockPoint(1, new Point(this.InnerRadius, 0), 45.0, this.dockType)
                };
        }

        #endregion
    }
}
