using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Model
{
    public class TrackEndPiece : TrackBase
    {
        protected override void Create()
        {
            double length = this.RailSpacing;

            // Tracks
            this.GeometryTracks = StraitGeometry(length, StraitOrientation.Left, this.RailSpacing);

            DrawingGroup drawingTracks = new DrawingGroup();
            drawingTracks.Children.Add(new GeometryDrawing(trackBrush, linePen, this.GeometryTracks));
            drawingTracks.Children.Add(this.textDrawing);
            this.drawingTracks = drawingTracks;

            DrawingGroup drawingTracksSelected = new DrawingGroup();
            drawingTracksSelected.Children.Add(new GeometryDrawing(trackBrushSelected, linePen, this.GeometryTracks));
            drawingTracksSelected.Children.Add(this.textDrawing);
            this.drawingTracksSelected = drawingTracksSelected;

            // Rail
            this.GeometryRail = StraitGeometry(length, StraitOrientation.Left, this.sleepersSpacing);

            DrawingGroup drawingRail = new DrawingGroup();
            if (this.Ballast)
            {
                drawingRail.Children.Add(StraitBallast(length, StraitOrientation.Left, 0, null));
            }
            drawingRail.Children.Add(StraitRail(false, length/2, StraitOrientation.Left, 0, null));
            this.drawingRail = drawingRail;

            DrawingGroup drawingRailSelected = new DrawingGroup();
            if (this.Ballast)
            {
                drawingRailSelected.Children.Add(StraitBallast(length, StraitOrientation.Left, 0, null));
            }
            drawingRailSelected.Children.Add(StraitRail(true, length / 2, StraitOrientation.Left, 0, null));
            this.drawingRailSelected = drawingRailSelected;

            // Terrain
            this.drawingTerrain = drawingRail;

            // dock points
            this.DockPoints = new List<TrackDockPoint>
            {
                new TrackDockPoint(0, new Point(this.RailSpacing / 2.0, 0.0), 315, this.dockType)
            };
        }
    }
}
