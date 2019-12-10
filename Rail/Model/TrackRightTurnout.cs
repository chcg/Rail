using Rail.Misc;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Rail.Model
{
    public class TrackRightTurnout : TrackTurnout
    {
        public override void Update(double spacing, bool ballast)
        {
            base.Update(spacing, ballast);

            this.Geometry = new CombinedGeometry(
                CreateStraitTrackGeometry(this.Length),
                CreateRightTurnoutGeometry(this.Length, this.Angle, this.Radius));

            DrawingGroup ballastDrawing = new DrawingGroup();
            ballastDrawing.Children.Add(CreateStraitBallastDrawing(this.Length));
            ballastDrawing.Children.Add(CreateRightTurnoutBallastDrawing(this.Length, this.Angle, this.Radius));
            this.BallastDrawing = ballastDrawing;

            DrawingGroup railDrawing = new DrawingGroup();
            railDrawing.Children.Add(CreateStraitTrackDrawing(this.Length));
            railDrawing.Children.Add(CreateRightTurnoutDrawing(this.Length, this.Angle, this.Radius));
            this.RailDrawing = railDrawing;

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

