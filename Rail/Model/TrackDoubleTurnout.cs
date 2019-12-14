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
            this.Geometry = new CombinedGeometry(
                CreateStraitTrackGeometry(this.Length),
                new CombinedGeometry(
                    CreateLeftTurnoutGeometry(this.Length, this.Angle, this.Radius),
                    CreateRightTurnoutGeometry(this.Length, this.Angle, this.Radius)));

            // Tracks
            DrawingGroup drawingTracks = new DrawingGroup();
            drawingTracks.Children.Add(new GeometryDrawing(trackBrush, linePen, this.Geometry));
            drawingTracks.Children.Add(this.textDrawing);
            this.drawingTracks = drawingTracks;

            // Rail
            DrawingGroup drawingRail = new DrawingGroup();
            if (this.Ballast)
            {
                drawingRail.Children.Add(StraitBallast(this.Length));
                drawingRail.Children.Add(CurvedBallast(this.Angle, this.Radius, CurvedOrientation.Center | CurvedOrientation.Clockwise, 0, new Point(-this.Length / 2, 0)));
                drawingRail.Children.Add(CurvedBallast(this.Angle, this.Radius, CurvedOrientation.Center | CurvedOrientation.Counterclockwise, 0, new Point(-this.Length / 2, 0)));
            }
            drawingRail.Children.Add(StraitRail(this.Length));
            drawingRail.Children.Add(CurvedRail(this.Angle, this.Radius, new Point(-this.Length / 2, 0), TrackDirection.Left));
            drawingRail.Children.Add(CurvedRail(this.Angle, this.Radius, new Point(-this.Length / 2, 0), TrackDirection.Right));
            this.drawingRail = drawingRail;

            // Terrain
            this.drawingTerrain = drawingRail;

            Point circleCenterLeft  = new Point(-this.Length / 2, -this.Radius);
            Point circleCenterRight = new Point(-this.Length / 2,  this.Radius);
            this.DockPoints = new List<TrackDockPoint> 
            { 
                new TrackDockPoint(-this.Length / 2.0, 0.0, 135, this.dockType), 
                new TrackDockPoint( this.Length / 2.0, 0.0, 315, this.dockType), 
                new TrackDockPoint(new Point(-this.Length / 2.0, 0).Rotate(-this.Angle, circleCenterLeft), -this.Angle - 45, this.dockType),
                new TrackDockPoint(new Point(-this.Length / 2.0, 0).Rotate( this.Angle, circleCenterRight), this.Angle - 45, this.dockType)
            };
        }
    }
}
