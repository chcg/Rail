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
                    CurvedGeometry(this.Angle, this.Radius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, this.RailSpacing, new Point(-this.Length / 2, 0)),
                    CurvedGeometry(this.Angle, this.Radius, CurvedOrientation.Clockwise | CurvedOrientation.Right, this.RailSpacing, new Point(-this.Length / 2, 0))));

            DrawingGroup drawingTracks = new DrawingGroup();
            drawingTracks.Children.Add(new GeometryDrawing(trackBrush, linePen, this.GeometryTracks));
            drawingTracks.Children.Add(this.textDrawing);
            this.drawingTracks = drawingTracks;

            DrawingGroup drawingTracksSelected = new DrawingGroup();
            drawingTracksSelected.Children.Add(new GeometryDrawing(trackBrushSelected, linePen, this.GeometryTracks));
            drawingTracksSelected.Children.Add(this.textDrawing);
            this.drawingTracksSelected = drawingTracksSelected;

            // Rail
            this.GeometryRail = new CombinedGeometry(
               StraitGeometry(this.Length, StraitOrientation.Center, this.sleepersSpacing),
               new CombinedGeometry(
                    CurvedGeometry(this.Angle, this.Radius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, this.sleepersSpacing, new Point(-this.Length / 2, 0)),
                    CurvedGeometry(this.Angle, this.Radius, CurvedOrientation.Clockwise | CurvedOrientation.Right, this.sleepersSpacing, new Point(-this.Length / 2, 0))));

            DrawingGroup drawingRail = new DrawingGroup();
            if (this.Ballast)
            {
                drawingRail.Children.Add(StraitBallast(this.Length));
                drawingRail.Children.Add(CurvedBallast(this.Angle, this.Radius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, new Point(-this.Length / 2, 0)));
                drawingRail.Children.Add(CurvedBallast(this.Angle, this.Radius, CurvedOrientation.Clockwise | CurvedOrientation.Right, new Point(-this.Length / 2, 0)));
            }
            drawingRail.Children.Add(StraitRail(false, this.Length));
            drawingRail.Children.Add(CurvedRail(false, this.Angle, this.Radius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, new Point(-this.Length / 2, 0)));
            drawingRail.Children.Add(CurvedRail(false, this.Angle, this.Radius, CurvedOrientation.Clockwise | CurvedOrientation.Right, new Point(-this.Length / 2, 0)));
            this.drawingRail = drawingRail;

            DrawingGroup drawingRailSelected = new DrawingGroup();
            if (this.Ballast)
            {
                drawingRailSelected.Children.Add(StraitBallast(this.Length));
                drawingRailSelected.Children.Add(CurvedBallast(this.Angle, this.Radius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, new Point(-this.Length / 2, 0)));
                drawingRailSelected.Children.Add(CurvedBallast(this.Angle, this.Radius, CurvedOrientation.Clockwise | CurvedOrientation.Right, new Point(-this.Length / 2, 0)));
            }
            drawingRailSelected.Children.Add(StraitRail(true, this.Length));
            drawingRailSelected.Children.Add(CurvedRail(true, this.Angle, this.Radius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, new Point(-this.Length / 2, 0)));
            drawingRailSelected.Children.Add(CurvedRail(true, this.Angle, this.Radius, CurvedOrientation.Clockwise | CurvedOrientation.Right, new Point(-this.Length / 2, 0)));
            this.drawingRailSelected = drawingRailSelected;

            // Terrain
            this.drawingTerrain = drawingRail;

            // dock points
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
