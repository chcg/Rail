using Rail.Misc;
using Rail.Properties;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Model
{
    public class TrackDoubleTurnout : TrackTurnout
    {
        [XmlIgnore]
        public override string Name
        {
            get
            {
                return $"{Resources.TrackDoubleTurnout}";
            }
        }

        protected override Geometry CreateGeometry(double spacing)
        {
            return new CombinedGeometry(
               StraitGeometry(this.Length, StraitOrientation.Center, spacing),
               new CombinedGeometry(
                    CurvedGeometry(this.Angle, this.Radius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, spacing, new Point(-this.Length / 2, 0)),
                    CurvedGeometry(this.Angle, this.Radius, CurvedOrientation.Clockwise | CurvedOrientation.Right, spacing, new Point(-this.Length / 2, 0))));
        }

        protected override Drawing CreateRailDrawing(bool isSelected)
        {
            DrawingGroup drawingRail = new DrawingGroup();
            if (this.ViewType.HasFlag(TrackViewType.Ballast))
            {
                drawingRail.Children.Add(StraitBallast(this.Length));
                drawingRail.Children.Add(CurvedBallast(this.Angle, this.Radius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, new Point(-this.Length / 2, 0)));
                drawingRail.Children.Add(CurvedBallast(this.Angle, this.Radius, CurvedOrientation.Clockwise | CurvedOrientation.Right, new Point(-this.Length / 2, 0)));
            }
            drawingRail.Children.Add(StraitSleepers(isSelected, this.Length));
            drawingRail.Children.Add(CurvedSleepers(isSelected, this.Angle, this.Radius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, new Point(-this.Length / 2, 0)));
            drawingRail.Children.Add(CurvedSleepers(isSelected, this.Angle, this.Radius, CurvedOrientation.Clockwise | CurvedOrientation.Right, new Point(-this.Length / 2, 0)));
            drawingRail.Children.Add(StraitRail(isSelected, this.Length));
            drawingRail.Children.Add(CurvedRail(isSelected, this.Angle, this.Radius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, new Point(-this.Length / 2, 0)));
            drawingRail.Children.Add(CurvedRail(isSelected, this.Angle, this.Radius, CurvedOrientation.Clockwise | CurvedOrientation.Right, new Point(-this.Length / 2, 0)));
            return drawingRail;
        }

        protected override List<TrackDockPoint> CreateDockPoints()
        {
            Point circleCenterLeft = new Point(-this.Length / 2, -this.Radius);
            Point circleCenterRight = new Point(-this.Length / 2, this.Radius);
            return new List<TrackDockPoint>
            {
                new TrackDockPoint(0, new Point(-this.Length / 2.0, 0.0), 135, this.dockType),
                new TrackDockPoint(1, new Point( this.Length / 2.0, 0.0), 315, this.dockType),
                new TrackDockPoint(2, new Point(-this.Length / 2.0, 0.0).Rotate(-this.Angle, circleCenterLeft), -this.Angle - 45, this.dockType),
                new TrackDockPoint(3, new Point(-this.Length / 2.0, 0.0).Rotate( this.Angle, circleCenterRight), this.Angle - 45, this.dockType)
            };
        }
    }
}
