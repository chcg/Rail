using Rail.Misc;
using Rail.Properties;
using Rail.Trigonometry;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Model
{
    public class TrackCurvedTurnout : TrackBase
    {
        [XmlAttribute("InnerRadius")]
        public string InnerRadiusNameOrValue { get; set; }

        [XmlIgnore]
        public double InnerRadius { get; set; }

        [XmlIgnore]
        public string InnerRadiusName { get; set; }
        
        [XmlAttribute("InnerAngle")]
        public double InnerAngle { get; set; }

        [XmlAttribute("OuterRadius")]
        public string OuterRadiusNameOrValue { get; set; }

        [XmlIgnore]
        public double OuterRadius { get; set; }

        [XmlIgnore]
        public string OuterRadiusName { get; set; }

        [XmlAttribute("OuterAngle")]
        public double OuterAngle { get; set; }

        [XmlAttribute("Length")]
        public string LengthNameOrValue { get; set; }

        [XmlIgnore]
        public double Length { get; set; }

        [XmlIgnore]
        public string LengthName { get; set; }

        [XmlAttribute("Direction")]
        public TrackDirection Direction { get; set; }

        [XmlAttribute("Drive")]
        public TrackDrive Drive { get; set; }

        [XmlIgnore]
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

        [XmlIgnore]
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

        public override void Update(TrackType trackType)
        {
            this.InnerRadius = GetValue(trackType.Radii, this.InnerRadiusNameOrValue);
            this.InnerRadiusName = GetName(this.InnerRadiusNameOrValue);
            this.OuterRadius = GetValue(trackType.Radii, this.OuterRadiusNameOrValue);
            this.OuterRadiusName = GetName(this.OuterRadiusNameOrValue);
            this.Length = GetValue(trackType.Lengths, this.LengthNameOrValue);
            this.LengthName = GetName(this.LengthNameOrValue);
            base.Update(trackType);
        }

        protected override Geometry CreateGeometry(double spacing)
        {
            double width = (this.OuterRadius * 2 * Math.PI * this.OuterAngle / 360.0 + this.Length) / 2;
            //double hight = this.OuterRadius * 2 * Math.PI * (this.OuterAngle - 90) / 360.0;

            //Point centerLeft = CurveCenter(this.OuterAngle, this.OuterRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left) + new Vector(this.Length / 2, 0);
            //Point centerRight = CurveCenter(this.OuterAngle, this.OuterRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right) + new Vector(this.Length / 2, 0);

            Point centerLeft = new Point(-width, 0);
            Point centerRight = new Point(width, 0);

            return this.Direction == TrackDirection.Left ?
                new CombinedGeometry(
                    CurvedGeometry(this.InnerAngle, this.InnerRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, spacing, centerLeft),
                    new CombinedGeometry(
                        StraitGeometry(this.Length, StraitOrientation.Left, spacing, 0, centerLeft),
                        CurvedGeometry(this.OuterAngle, this.OuterRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, spacing, centerLeft + new Vector(this.Length, 0)))
                    ) :
                new CombinedGeometry(
                    CurvedGeometry(this.InnerAngle, this.InnerRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right, spacing, centerRight),
                    new CombinedGeometry(
                        StraitGeometry(this.Length, StraitOrientation.Right, spacing, 0, centerRight),
                        CurvedGeometry(this.OuterAngle, this.OuterRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right, spacing, centerRight - new Vector(this.Length, 0)))
                    );
        }

        protected override Drawing CreateRailDrawing()
        {
            double width = (this.OuterRadius * 2 * Math.PI * this.OuterAngle / 360.0 + this.Length) / 2;
            //double hight = this.OuterRadius * 2 * Math.PI * (this.OuterAngle - 90) / 360.0;

            //Point centerLeft = CurveCenter(this.OuterAngle, this.OuterRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left) + new Vector(this.Length / 2, 0);
            //Point centerRight = CurveCenter(this.OuterAngle, this.OuterRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right) + new Vector(this.Length / 2, 0);

            Point centerLeft = new Point(-width, 0);
            Point centerRight = new Point(width, 0);

            DrawingGroup drawingRail = new DrawingGroup();
            if (this.ViewType.HasFlag(TrackViewType.Ballast))
            {
                if (this.Direction == TrackDirection.Left)
                {
                    drawingRail.Children.Add(CurvedBallast(this.InnerAngle, this.InnerRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, centerLeft));
                    drawingRail.Children.Add(StraitBallast(this.Length, StraitOrientation.Left, 0, centerLeft));
                    drawingRail.Children.Add(CurvedBallast(this.OuterAngle, this.OuterRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, centerLeft + new Vector(this.Length, 0)));
                }
                else
                {
                    drawingRail.Children.Add(CurvedBallast(this.InnerAngle, this.InnerRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right, centerRight));
                    drawingRail.Children.Add(StraitBallast(this.Length, StraitOrientation.Right, 0, centerRight));
                    drawingRail.Children.Add(CurvedBallast(this.OuterAngle, this.OuterRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right, centerRight - new Vector(this.Length, 0)));
                }
            }
            if (this.Direction == TrackDirection.Left)
            {
                drawingRail.Children.Add(CurvedSleepers(this.InnerAngle, this.InnerRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, centerLeft));
                drawingRail.Children.Add(StraitSleepers(this.Length, StraitOrientation.Left, 0, centerLeft));
                drawingRail.Children.Add(CurvedSleepers(this.OuterAngle, this.OuterRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, centerLeft + new Vector(this.Length, 0)));
                drawingRail.Children.Add(CurvedRail(this.InnerAngle, this.InnerRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, centerLeft));
                drawingRail.Children.Add(StraitRail(this.Length, StraitOrientation.Left, 0, centerLeft));
                drawingRail.Children.Add(CurvedRail(this.OuterAngle, this.OuterRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, centerLeft + new Vector(this.Length, 0)));
            }
            else
            {
                drawingRail.Children.Add(CurvedSleepers(this.InnerAngle, this.InnerRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right, centerRight));
                drawingRail.Children.Add(StraitSleepers(this.Length, StraitOrientation.Right, 0, centerRight));
                drawingRail.Children.Add(CurvedSleepers(this.OuterAngle, this.OuterRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right, centerRight - new Vector(this.Length, 0)));
                drawingRail.Children.Add(CurvedRail(this.InnerAngle, this.InnerRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right, centerRight));
                drawingRail.Children.Add(StraitRail(this.Length, StraitOrientation.Right, 0, centerRight));
                drawingRail.Children.Add(CurvedRail(this.OuterAngle, this.OuterRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right, centerRight - new Vector(this.Length, 0)));
            }
            return drawingRail;
        }

        protected override List<TrackDockPoint> CreateDockPoints()
        {
            double innerWidth = (this.InnerRadius * 2 * Math.PI * this.InnerAngle / 360.0 + this.Length) / 2;
            double outerWidth = (this.OuterRadius * 2 * Math.PI * this.OuterAngle / 360.0 + this.Length) / 2;
            //double hight = this.OuterRadius * 2 * Math.PI * (this.OuterAngle - 90) / 360.0;

            //Point centerLeft = CurveCenter(this.OuterAngle, this.OuterRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left) + new Vector(this.Length / 2, 0);
            //Point centerRight = CurveCenter(this.OuterAngle, this.OuterRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right) + new Vector(this.Length / 2, 0);

            Point innerCircleCenterLeft = new Point(-innerWidth, -this.InnerRadius);
            Point outerCircleCenterLeft = new Point(-outerWidth + Length, -this.OuterRadius);

            Point innerCircleCenterRight = new Point(innerWidth, -this.InnerRadius);
            Point outerCircleCenterRight = new Point(outerWidth - Length, -this.OuterRadius);
            
            

            return this.Direction == TrackDirection.Left ? 
                new List<TrackDockPoint>
                {
                    new TrackDockPoint(0, new Point(-innerWidth, 0.0), 90 + 45, this.dockType),
                    new TrackDockPoint(1, new Point(-innerWidth, 0.0).Rotate(-this.InnerAngle, innerCircleCenterLeft), -this.InnerAngle - 45, this.dockType),
                    new TrackDockPoint(2, new Point(-outerWidth + this.Length, 0.0).Rotate(-this.OuterAngle, outerCircleCenterLeft), -this.OuterAngle -45, this.dockType),
                } :
                new List<TrackDockPoint>
                {
                    new TrackDockPoint(0, new Point(+innerWidth, 0.0), 90 + 45 + 180, this.dockType),
                    new TrackDockPoint(1, new Point(+innerWidth, 0.0).Rotate(this.InnerAngle, innerCircleCenterRight), this.InnerAngle + 135, this.dockType),
                    new TrackDockPoint(2, new Point(+outerWidth - this.Length, 0.0).Rotate(this.OuterAngle, outerCircleCenterRight), this.OuterAngle + 135, this.dockType),

                    //new TrackDockPoint(0, new Point(this.InnerRadius, 0), 225.0, this.dockType),
                    //new TrackDockPoint(1, new Point(this.InnerRadius, 0), 45.0, this.dockType)
                };
        }
    }
}
