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
    public class TrackDoubleSlipSwitch : TrackBaseSingle
    {
        #region store

        [XmlElement("TurnoutDrive")]
        public TrackDrive TurnoutDrive { get; set; }

        [XmlElement("Length")]
        public Guid LengthId { get; set; }

        [XmlElement("CrossingAngle")]
        public Guid CrossingAngleId { get; set; }

        [XmlElement("SlipRadius")]
        public Guid SlipRadiusId { get; set; }

        public bool ShouldSerializeTurnoutDrive() { return this.TurnoutDrive != TrackDrive.Unknown; }

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
        public override TrackTypes TrackType { get { return TrackTypes.DoubleSlipSwitch; } }

        [XmlIgnore, JsonIgnore]
        public override double RampLength { get { return this.Length; } }

        public override TrackBase Clone()
        {
            TrackDoubleSlipSwitch track = new TrackDoubleSlipSwitch
            {
                Article = this.Article,
                TurnoutDrive = this.TurnoutDrive,
                LengthId = this.LengthId,
                CrossingAngle = this.CrossingAngle,
                SlipRadius = this.SlipRadius
            };
            track.Update(this.trackType);
            return track;
        }

        public override void Update(TrackType trackType)
        {
            this.Length = GetValue(trackType.Lengths, this.LengthId);
            this.CrossingAngle = GetValue(trackType.Angles, this.CrossingAngleId);
            this.SlipRadius = GetValue(trackType.Radii, this.SlipRadiusId);

            string drive = this.TurnoutDrive switch
            {
                TrackDrive.Electrical => Resources.TrackDriveElectrical,
                TrackDrive.Mechanical => Resources.TrackDriveMechanical,
                _ => string.Empty
            };
            this.Name = $"{Resources.TrackDoubleSlipSwitch} {drive}";
            this.Description = $"{this.Article} {Resources.TrackDoubleSlipSwitch} {drive}";
            
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
