using Rail.Misc;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Rail.Model
{
    public class TrackDoubleTurnout : TrackTurnout
    {
        public override void Update(double spacing, bool ballast)
        {
            base.Update(spacing, ballast);

            this.Geometry = new CombinedGeometry(
                CreateStraitTrackGeometry(this.Length),
                new CombinedGeometry(
                    CreateLeftTurnoutGeometry(this.Length, this.Angle, this.Radius),
                    CreateRightTurnoutGeometry(this.Length, this.Angle, this.Radius)));

            DrawingGroup ballastDrawing = new DrawingGroup();
            ballastDrawing.Children.Add(CreateStraitBallastDrawing(this.Length));
            ballastDrawing.Children.Add(CreateLeftTurnoutBallastDrawing(this.Length, this.Angle, this.Radius));
            ballastDrawing.Children.Add(CreateRightTurnoutBallastDrawing(this.Length, this.Angle, this.Radius));
            this.BallastDrawing = ballastDrawing;

            DrawingGroup railDrawing = new DrawingGroup();
            railDrawing.Children.Add(CreateStraitTrackDrawing(this.Length));
            railDrawing.Children.Add(CreateLeftTurnoutDrawing(this.Length, this.Angle, this.Radius));
            railDrawing.Children.Add(CreateRightTurnoutDrawing(this.Length, this.Angle, this.Radius));
            this.RailDrawing = railDrawing;


            Point circleCenterLeft  = new Point(-this.Length / 2, -this.Radius);
            Point circleCenterRight = new Point(-this.Length / 2,  this.Radius);
            this.DockPoints = new List<TrackDockPoint> 
            { 
                new TrackDockPoint(-this.Length / 2.0, 0.0, 135), 
                new TrackDockPoint( this.Length / 2.0, 0.0, 315), 
                new TrackDockPoint(new Point(-this.Length / 2.0, 0).Rotate(-this.Angle, circleCenterLeft), -this.Angle - 45),
                new TrackDockPoint(new Point(-this.Length / 2.0, 0).Rotate( this.Angle, circleCenterRight), this.Angle - 45)
            };
        }
    }
}
