using Rail.Misc;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Rail.Model
{
    public class TrackRightTurnout : TrackTurnout
    {
        protected override void Create()
        {
            this.Geometry = new CombinedGeometry(
                CreateStraitTrackGeometry(this.Length),
                CreateRightTurnoutGeometry(this.Length, this.Angle, this.Radius));

            DrawingGroup drawing = new DrawingGroup();
            if (this.Ballast)
            {
                drawing.Children.Add(StraitBallast(this.Length));
                drawing.Children.Add(CurvedBallast(this.Angle, this.Radius, new Point(-this.Length / 2, 0), Direction.Right));
            }
            drawing.Children.Add(StraitRail(this.Length));
            drawing.Children.Add(CurvedRail(this.Angle, this.Radius, new Point(-this.Length / 2, 0), Direction.Right));
            this.RailDrawing = drawing;

            Point circleCenter = new Point(-this.Length / 2, this.Radius);
            this.DockPoints = new List<TrackDockPoint>
            {
                new TrackDockPoint(-this.Length / 2.0, 0.0, 90 + 45),
                new TrackDockPoint( this.Length / 2.0, 0.0, 180 + 90 + 45),
                new TrackDockPoint(new Point(-this.Length / 2.0, 0).Rotate(this.Angle, circleCenter), this.Angle - 45)
            };
        }
    }
}

