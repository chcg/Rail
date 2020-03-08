using Rail.Tracks.Properties;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Tracks
{
    public class TrackYTurnout : TrackBaseSingle
    {
        #region store

        [XmlElement("LeftTurnoutLength")]
        public Guid LeftTurnoutLengthId { get; set; }

        [XmlElement("LeftTurnoutRadius")]
        public Guid LeftTurnoutRadiusId { get; set; }

        [XmlElement("LeftTurnoutAngle")]
        public Guid LeftTurnoutAngleId { get; set; }

        [XmlElement("RightTurnoutLength")]
        public Guid RightTurnoutLengthId { get; set; }

        [XmlElement("RightTurnoutRadius")]
        public Guid RightTurnoutRadiusId { get; set; }

        [XmlElement("RightTurnoutAngle")]
        public Guid RightTurnoutAngleId { get; set; }

        [XmlElement("TurnoutDrive")]
        public TrackDrive TurnoutDrive { get; set; }

        #endregion

        #region internal

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
        public override double RampLength { get { return 0; /* TODO clac length */ } }
        
        public override void Update(TrackType trackType)
        {
            this.LeftTurnoutLength = GetValue(trackType.Lengths, this.LeftTurnoutLengthId);
            this.LeftTurnoutRadius = GetValue(trackType.Radii, this.LeftTurnoutRadiusId);
            this.LeftTurnoutAngle = GetValue(trackType.Angles, this.LeftTurnoutAngleId);
            this.RightTurnoutLength = GetValue(trackType.Lengths, this.RightTurnoutLengthId);
            this.RightTurnoutRadius = GetValue(trackType.Radii, this.RightTurnoutRadiusId);
            this.RightTurnoutAngle = GetValue(trackType.Angles, this.RightTurnoutAngleId);

            string drive = this.TurnoutDrive == TrackDrive.Electrical ? Resources.TrackDriveElectrical :
                              (this.TurnoutDrive == TrackDrive.Mechanical ? Resources.TrackDriveMechanical : string.Empty);
            this.Name= $"{Resources.TrackYTurnout} {drive}";
            this.Description = $"{this.Article} {Resources.TrackYTurnout} {drive}";
            
            base.Update(trackType);
        }


        protected override Geometry CreateGeometry()
        {
            double leftLength = this.LeftTurnoutRadius * 2 * Math.PI * this.LeftTurnoutAngle / 360.0;
            double rightLength = this.RightTurnoutRadius * 2 * Math.PI * this.RightTurnoutAngle / 360.0;

            return new CombinedGeometry(
                CurvedGeometry(this.LeftTurnoutAngle, this.LeftTurnoutRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, new Point(-leftLength / 2, 0)),
                CurvedGeometry(this.RightTurnoutAngle, this.RightTurnoutRadius, CurvedOrientation.Clockwise | CurvedOrientation.Right, new Point(-rightLength / 2, 0)));
        }

        protected override Drawing CreateRailDrawing()
        {
            double leftLength = this.LeftTurnoutRadius * 2 * Math.PI * this.LeftTurnoutAngle / 360.0;
            double rightLength = this.RightTurnoutRadius * 2 * Math.PI * this.RightTurnoutAngle / 360.0;

            DrawingGroup drawingRail = new DrawingGroup();
            if (this.HasBallast)
            {
                drawingRail.Children.Add(CurvedBallast(this.LeftTurnoutAngle, this.LeftTurnoutRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, new Point(-leftLength / 2, 0)));
                drawingRail.Children.Add(CurvedBallast(this.RightTurnoutAngle, this.RightTurnoutRadius, CurvedOrientation.Clockwise | CurvedOrientation.Right, new Point(-rightLength / 2, 0)));
            }
            drawingRail.Children.Add(CurvedSleepers(this.LeftTurnoutAngle, this.LeftTurnoutRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, new Point(-leftLength / 2, 0)));
            drawingRail.Children.Add(CurvedSleepers(this.RightTurnoutAngle, this.RightTurnoutRadius, CurvedOrientation.Clockwise | CurvedOrientation.Right, new Point(-rightLength / 2, 0)));
            drawingRail.Children.Add(CurvedRail(this.LeftTurnoutAngle, this.LeftTurnoutRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, new Point(-leftLength / 2, 0)));
            drawingRail.Children.Add(CurvedRail(this.RightTurnoutAngle, this.RightTurnoutRadius, CurvedOrientation.Clockwise | CurvedOrientation.Right, new Point(-rightLength / 2, 0)));
            return drawingRail;
        }

        protected override List<TrackDockPoint> CreateDockPoints()
        {
            return new List<TrackDockPoint>
            {
                //new TrackDockPoint(-this.Length / 2.0, 0.0, 135),
                //new TrackDockPoint( this.Length / 2.0, 0.0, 315),
                //new TrackDockPoint(new Point(-this.Length / 2.0, 0).Rotate(-this.Angle, circleCenterLeft), -this.Angle - 45),
                //new TrackDockPoint(new Point(-this.Length / 2.0, 0).Rotate( this.Angle, circleCenterRight), this.Angle - 45)
            };
        }

        #endregion
    }
 
}
