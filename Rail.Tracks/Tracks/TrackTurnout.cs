using Rail.Tracks.Properties;
using Rail.Tracks.Trigonometry;
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

        [XmlAttribute("StraightLength")]
        public string StraightLengthName { get; set; }

        [XmlAttribute("TurnoutLength")]
        public string TurnoutLengthName { get; set; }

        [XmlAttribute("TurnoutRadius")]
        public string TurnoutRadiusName { get; set; }

        [XmlAttribute("TurnoutAngle")]
        public string TurnoutAngleName { get; set; }

        [XmlAttribute("Direction")]
        public TrackDirection Direction { get; set; }

        [XmlAttribute("Drive")]
        public TrackDrive Drive { get; set; }

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
        public override double RampLength { get { return this.StraightLength; } }

        [XmlIgnore, JsonIgnore]
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

        [XmlIgnore, JsonIgnore]
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

        #endregion

        public override void Update(TrackType trackType)
        {
            this.StraightLength = GetValue(trackType.Lengths, this.StraightLengthName);
            this.TurnoutLength = GetValue(trackType.Lengths, this.TurnoutLengthName);
            this.TurnoutRadius = GetValue(trackType.Radii, this.TurnoutRadiusName);
            this.TurnoutAngle = GetValue(trackType.Angles, this.TurnoutAngleName);
            base.Update(trackType);
        }

        protected override Geometry CreateGeometry()
        {
            return new CombinedGeometry(
                StraitGeometry(this.StraightLength, StraitOrientation.Center),
                this.Direction == TrackDirection.Left ?
                    CurvedGeometry(this.TurnoutAngle, this.TurnoutRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, new Point(-this.StraightLength / 2, 0)) :
                    CurvedGeometry(this.TurnoutAngle, this.TurnoutRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right, new Point(this.StraightLength / 2, 0)));
        }

        protected override Drawing CreateRailDrawing()
        {
            DrawingGroup drawingRail = new DrawingGroup();
            if (this.HasBallast)
            {
                drawingRail.Children.Add(StraitBallast(this.StraightLength));
                drawingRail.Children.Add(this.Direction == TrackDirection.Left ?
                    CurvedBallast(this.TurnoutAngle, this.TurnoutRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, new Point(-this.StraightLength / 2, 0)) :
                    CurvedBallast(this.TurnoutAngle, this.TurnoutRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right, new Point(this.StraightLength / 2, 0)));
            }
            drawingRail.Children.Add(StraitSleepers(this.StraightLength));
            drawingRail.Children.Add(this.Direction == TrackDirection.Left ?
                    CurvedSleepers(this.TurnoutAngle, this.TurnoutRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, new Point(-this.StraightLength / 2, 0)) :
                    CurvedSleepers(this.TurnoutAngle, this.TurnoutRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right, new Point(this.StraightLength / 2, 0)));
            drawingRail.Children.Add(StraitRail(this.StraightLength));
            drawingRail.Children.Add(this.Direction == TrackDirection.Left ?
                    CurvedRail(this.TurnoutAngle, this.TurnoutRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, new Point(-this.StraightLength / 2, 0)) :
                    CurvedRail(this.TurnoutAngle, this.TurnoutRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right, new Point(this.StraightLength / 2, 0)));
            return drawingRail;
        }

        protected override List<TrackDockPoint> CreateDockPoints()
        {
            Point circleCenterLeft = new Point(-this.StraightLength / 2, -this.TurnoutRadius);
            Point circleCenterRight = new Point(this.StraightLength / 2, -this.TurnoutRadius);

            return this.Direction == TrackDirection.Left ?
                new List<TrackDockPoint>
                {
                    new TrackDockPoint(0, new Point(-this.StraightLength / 2.0, 0.0), 90 + 45, this.dockType),
                    new TrackDockPoint(1, new Point( this.StraightLength / 2.0, 0.0), 180 + 90 + 45, this.dockType),
                    new TrackDockPoint(2, new Point(-this.StraightLength / 2.0, 0.0).Rotate(-this.TurnoutAngle, circleCenterLeft), -this.TurnoutAngle - 45, this.dockType),
                } :
                new List<TrackDockPoint>
                {
                    new TrackDockPoint(0, new Point(-this.StraightLength / 2.0, 0.0), 90 + 45, this.dockType),
                    new TrackDockPoint(1, new Point( this.StraightLength / 2.0, 0.0), 180 + 90 + 45, this.dockType),
                    new TrackDockPoint(3, new Point( this.StraightLength / 2.0, 0.0).Rotate(+this.TurnoutAngle, circleCenterRight), this.TurnoutAngle + 135, this.dockType)
                };
        }
    }
}
