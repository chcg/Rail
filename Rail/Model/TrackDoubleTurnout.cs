using Rail.Misc;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Rail.Model
{
    public class TrackDoubleTurnout : TrackTurnout
    {
        protected override void Create()
        {


            // Tracks
            this.GeometryTracks = new CombinedGeometry(
               StraitGeometry(this.Length, StraitOrientation.Center, this.RailSpacing),
               new CombinedGeometry(
                   CreateLeftTurnoutGeometry(this.Length, this.Angle, this.Radius),
                   CreateRightTurnoutGeometry(this.Length, this.Angle, this.Radius))); 
            
            DrawingGroup drawingTracks = new DrawingGroup();
            drawingTracks.Children.Add(new GeometryDrawing(trackBrush, linePen, this.GeometryTracks));
            drawingTracks.Children.Add(this.textDrawing);
            this.drawingTracks = drawingTracks;

            // Rail
            this.GeometryRail = new CombinedGeometry(
               StraitGeometry(this.Length, StraitOrientation.Center, this.sleepersSpacing),
               new CombinedGeometry(
                   CreateLeftTurnoutGeometry(this.Length, this.Angle, this.Radius),
                   CreateRightTurnoutGeometry(this.Length, this.Angle, this.Radius)));

            DrawingGroup drawingRail = new DrawingGroup();
            if (this.Ballast)
            {
                drawingRail.Children.Add(StraitBallast(this.Length));
                //drawingRail.Children.Add(CurvedBallast(this.Angle, this.Radius, CurvedOrientation.Center | CurvedOrientation.Clockwise, 0, new Point(-this.Length / 2, 0)));
                //drawingRail.Children.Add(CurvedBallast(this.Angle, this.Radius, CurvedOrientation.Center | CurvedOrientation.Counterclockwise, 0, new Point(-this.Length / 2, 0)));
            }
            drawingRail.Children.Add(StraitRail(false, this.Length));
            drawingRail.Children.Add(CurvedRail(this.Angle, this.Radius, CurvedOrientation.Clockwise));
            drawingRail.Children.Add(CurvedRail(this.Angle, this.Radius, CurvedOrientation.Counterclockwise));
            this.drawingRail = drawingRail;

            // Terrain
            this.drawingTerrain = drawingRail;

            Point circleCenterLeft  = new Point(-this.Length / 2, -this.Radius);
            Point circleCenterRight = new Point(-this.Length / 2,  this.Radius);
            this.DockPoints = new List<TrackDockPoint> 
            { 
                new TrackDockPoint(0, new Point(-this.Length / 2.0, 0.0), 135, this.dockType), 
                new TrackDockPoint(1, new Point( this.Length / 2.0, 0.0), 315, this.dockType), 
                new TrackDockPoint(2, new Point(-this.Length / 2.0, 0.0).Rotate(-this.Angle, circleCenterLeft), -this.Angle - 45, this.dockType),
                new TrackDockPoint(3, new Point(-this.Length / 2.0, 0.0).Rotate( this.Angle, circleCenterRight), this.Angle - 45, this.dockType)
            };
        }
    }
}
