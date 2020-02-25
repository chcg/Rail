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

        [XmlAttribute("InnerLength")]
        public string InnerLengthName { get; set; }

        [XmlAttribute("InnerRadius")]
        public string InnerRadiusName { get; set; }

        [XmlAttribute("InnerAngle")]
        public string InnerAngleName { get; set; }

        [XmlAttribute("OuterLength")]
        public string OuterLengthName { get; set; }

        [XmlAttribute("OuterRadius")]
        public string OuterRadiusName { get; set; }

        [XmlAttribute("OuterAngle")]
        public string OuterAngleName { get; set; }

        [XmlAttribute("Direction")]
        public TrackDirection Direction { get; set; }

        [XmlAttribute("Drive")]
        public TrackDrive Drive { get; set; }

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
               
        [XmlIgnore, JsonIgnore]
        public override double RampLength { get { return this.OuterLength; } }
        
        [XmlIgnore, JsonIgnore]
        public override string Name
        {
            get
            {
                string drive = this.Drive == TrackDrive.Electrical ? Resources.TrackDriveElectrical :
                              (this.Drive == TrackDrive.Mechanical ? Resources.TrackDriveMechanical : string.Empty);
                return Direction == TrackDirection.Left ?
                    $"{Resources.TrackCurvedTurnoutLeft} {drive}" :
                    $"{Resources.TrackCurvedTurnoutRight} {drive}";
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
                    $"{this.Article} {Resources.TrackCurvedTurnoutLeft} {drive}" :
                    $"{this.Article} {Resources.TrackCurvedTurnoutRight} {drive}";
            }
        }

        #endregion

        public override void Update(TrackType trackType)
        {
            this.InnerLength = GetValue(trackType.Lengths, this.InnerLengthName);
            this.InnerRadius = GetValue(trackType.Radii, this.InnerRadiusName);
            this.InnerAngle = GetValue(trackType.Angles, this.InnerAngleName);
            this.OuterLength = GetValue(trackType.Lengths, this.OuterLengthName);
            this.OuterRadius = GetValue(trackType.Radii, this.OuterRadiusName);
            this.OuterAngle = GetValue(trackType.Angles, this.OuterAngleName);

            base.Update(trackType);
        }

        protected override Geometry CreateGeometry()
        {
            double width = (this.OuterRadius * 2 * Math.PI * this.OuterAngle / 360.0 + this.OuterLength) / 2;
            //double hight = this.OuterRadius * 2 * Math.PI * (this.OuterAngle - 90) / 360.0;

            //Point centerLeft = CurveCenter(this.OuterAngle, this.OuterRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left) + new Vector(this.Length / 2, 0);
            //Point centerRight = CurveCenter(this.OuterAngle, this.OuterRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right) + new Vector(this.Length / 2, 0);

            Point centerLeft = new Point(-width, 0);
            Point centerRight = new Point(width, 0);

            return this.Direction == TrackDirection.Left ?
                new CombinedGeometry(
                    CurvedGeometry(this.InnerAngle, this.InnerRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, centerLeft),
                    new CombinedGeometry(
                        StraitGeometry(this.OuterLength, StraitOrientation.Left, 0, centerLeft),
                        CurvedGeometry(this.OuterAngle, this.OuterRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, centerLeft + new Vector(this.OuterLength, 0)))
                    ) :
                new CombinedGeometry(
                    CurvedGeometry(this.InnerAngle, this.InnerRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right, centerRight),
                    new CombinedGeometry(
                        StraitGeometry(this.OuterLength, StraitOrientation.Right, 0, centerRight),
                        CurvedGeometry(this.OuterAngle, this.OuterRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right, centerRight - new Vector(this.OuterLength, 0)))
                    );
        }

        protected override Drawing CreateRailDrawing()
        {
            double width = (this.OuterRadius * 2 * Math.PI * this.OuterAngle / 360.0 + this.OuterLength) / 2;
            //double hight = this.OuterRadius * 2 * Math.PI * (this.OuterAngle - 90) / 360.0;

            //Point centerLeft = CurveCenter(this.OuterAngle, this.OuterRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left) + new Vector(this.Length / 2, 0);
            //Point centerRight = CurveCenter(this.OuterAngle, this.OuterRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right) + new Vector(this.Length / 2, 0);

            Point centerLeft = new Point(-width, 0);
            Point centerRight = new Point(width, 0);

            DrawingGroup drawingRail = new DrawingGroup();
            if (this.HasBallast)
            {
                if (this.Direction == TrackDirection.Left)
                {
                    drawingRail.Children.Add(CurvedBallast(this.InnerAngle, this.InnerRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, centerLeft));
                    drawingRail.Children.Add(StraitBallast(this.OuterLength, StraitOrientation.Left, 0, centerLeft));
                    drawingRail.Children.Add(CurvedBallast(this.OuterAngle, this.OuterRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, centerLeft + new Vector(this.OuterLength, 0)));
                }
                else
                {
                    drawingRail.Children.Add(CurvedBallast(this.InnerAngle, this.InnerRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right, centerRight));
                    drawingRail.Children.Add(StraitBallast(this.OuterLength, StraitOrientation.Right, 0, centerRight));
                    drawingRail.Children.Add(CurvedBallast(this.OuterAngle, this.OuterRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right, centerRight - new Vector(this.OuterLength, 0)));
                }
            }
            if (this.Direction == TrackDirection.Left)
            {
                drawingRail.Children.Add(CurvedSleepers(this.InnerAngle, this.InnerRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, centerLeft));
                drawingRail.Children.Add(StraitSleepers(this.OuterLength, StraitOrientation.Left, 0, centerLeft));
                drawingRail.Children.Add(CurvedSleepers(this.OuterAngle, this.OuterRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, centerLeft + new Vector(this.OuterLength, 0)));
                drawingRail.Children.Add(CurvedRail(this.InnerAngle, this.InnerRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, centerLeft));
                drawingRail.Children.Add(StraitRail(this.OuterLength, StraitOrientation.Left, 0, centerLeft));
                drawingRail.Children.Add(CurvedRail(this.OuterAngle, this.OuterRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, centerLeft + new Vector(this.OuterLength, 0)));
            }
            else
            {
                drawingRail.Children.Add(CurvedSleepers(this.InnerAngle, this.InnerRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right, centerRight));
                drawingRail.Children.Add(StraitSleepers(this.OuterLength, StraitOrientation.Right, 0, centerRight));
                drawingRail.Children.Add(CurvedSleepers(this.OuterAngle, this.OuterRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right, centerRight - new Vector(this.OuterLength, 0)));
                drawingRail.Children.Add(CurvedRail(this.InnerAngle, this.InnerRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right, centerRight));
                drawingRail.Children.Add(StraitRail(this.OuterLength, StraitOrientation.Right, 0, centerRight));
                drawingRail.Children.Add(CurvedRail(this.OuterAngle, this.OuterRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right, centerRight - new Vector(this.OuterLength, 0)));
            }
            return drawingRail;
        }

        protected override List<TrackDockPoint> CreateDockPoints()
        {
            double innerWidth = (this.InnerRadius * 2 * Math.PI * this.InnerAngle / 360.0 + this.OuterLength) / 2;
            double outerWidth = (this.OuterRadius * 2 * Math.PI * this.OuterAngle / 360.0 + this.OuterLength) / 2;
            //double hight = this.OuterRadius * 2 * Math.PI * (this.OuterAngle - 90) / 360.0;

            //Point centerLeft = CurveCenter(this.OuterAngle, this.OuterRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left) + new Vector(this.Length / 2, 0);
            //Point centerRight = CurveCenter(this.OuterAngle, this.OuterRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right) + new Vector(this.Length / 2, 0);

            Point innerCircleCenterLeft = new Point(-innerWidth, -this.InnerRadius);
            Point outerCircleCenterLeft = new Point(-outerWidth + OuterLength, -this.OuterRadius);

            Point innerCircleCenterRight = new Point(innerWidth, -this.InnerRadius);
            Point outerCircleCenterRight = new Point(outerWidth - OuterLength, -this.OuterRadius);
            
            

            return this.Direction == TrackDirection.Left ? 
                new List<TrackDockPoint>
                {
                    new TrackDockPoint(0, new Point(-innerWidth, 0.0), 90 + 45, this.dockType),
                    new TrackDockPoint(1, new Point(-innerWidth, 0.0).Rotate(-this.InnerAngle, innerCircleCenterLeft), -this.InnerAngle - 45, this.dockType),
                    new TrackDockPoint(2, new Point(-outerWidth + this.OuterLength, 0.0).Rotate(-this.OuterAngle, outerCircleCenterLeft), -this.OuterAngle -45, this.dockType),
                } :
                new List<TrackDockPoint>
                {
                    new TrackDockPoint(0, new Point(+innerWidth, 0.0), 90 + 45 + 180, this.dockType),
                    new TrackDockPoint(1, new Point(+innerWidth, 0.0).Rotate(this.InnerAngle, innerCircleCenterRight), this.InnerAngle + 135, this.dockType),
                    new TrackDockPoint(2, new Point(+outerWidth - this.OuterLength, 0.0).Rotate(this.OuterAngle, outerCircleCenterRight), this.OuterAngle + 135, this.dockType),

                    //new TrackDockPoint(0, new Point(this.InnerRadius, 0), 225.0, this.dockType),
                    //new TrackDockPoint(1, new Point(this.InnerRadius, 0), 45.0, this.dockType)
                };
        }
    }
}
