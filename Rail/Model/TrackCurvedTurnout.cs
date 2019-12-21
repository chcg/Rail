using Rail.Misc;
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
        public double InnerRadius { get; set; }

        [XmlAttribute("InnerAngle")]
        public double InnerAngle { get; set; }

        [XmlAttribute("OuterRadius")]
        public double OuterRadius { get; set; }

        [XmlAttribute("OuterAngle")]
        public double OuterAngle { get; set; }

        [XmlAttribute("Length")]
        public double Length { get; set; }

        [XmlAttribute("Direction")]
        public TrackDirection Direction { get; set; }

        protected override Geometry CreateGeometry(double spacing)
        {
            double width = (this.OuterRadius * 2 * Math.PI * this.OuterAngle / 360.0 + this.Length) / 2;
            double hight = this.OuterRadius * 2 * Math.PI * (this.OuterAngle - 90) / 360.0;

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

        protected override Drawing CreateRailDrawing(bool isSelected)
        {
            double width = (this.OuterRadius * 2 * Math.PI * this.OuterAngle / 360.0 + this.Length) / 2;
            double hight = this.OuterRadius * 2 * Math.PI * (this.OuterAngle - 90) / 360.0;

            //Point centerLeft = CurveCenter(this.OuterAngle, this.OuterRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left) + new Vector(this.Length / 2, 0);
            //Point centerRight = CurveCenter(this.OuterAngle, this.OuterRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right) + new Vector(this.Length / 2, 0);

            Point centerLeft = new Point(-width, 0);
            Point centerRight = new Point(width, 0);

            DrawingGroup drawingRail = new DrawingGroup();
            if (this.Ballast)
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
                drawingRail.Children.Add(CurvedRail(isSelected, this.InnerAngle, this.InnerRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, centerLeft));
                drawingRail.Children.Add(StraitRail(isSelected, this.Length, StraitOrientation.Left, 0, centerLeft));
                drawingRail.Children.Add(CurvedRail(isSelected, this.OuterAngle, this.OuterRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, centerLeft + new Vector(this.Length, 0)));
            }
            else
            {
                drawingRail.Children.Add(CurvedRail(isSelected, this.InnerAngle, this.InnerRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right, centerRight));
                drawingRail.Children.Add(StraitRail(isSelected, this.Length, StraitOrientation.Right, 0, centerRight));
                drawingRail.Children.Add(CurvedRail(isSelected, this.OuterAngle, this.OuterRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right, centerRight - new Vector(this.Length, 0)));
            }
            return drawingRail;
        }

        protected override List<TrackDockPoint> CreateDockPoints()
        {
            return new List<TrackDockPoint>
            {
                new TrackDockPoint(0, new Point(this.InnerRadius, 0), 225.0, this.dockType),
                new TrackDockPoint(1, new Point(this.InnerRadius, 0), 45.0, this.dockType)
            };
        }
    }
}
