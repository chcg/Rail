using Rail.Misc;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Model
{
    public class TrackCurved : TrackBase
    {
        [XmlAttribute("Radius")]
        public double Radius { get; set; }

        [XmlAttribute("Angle")]
        public double Angle { get; set; }

        protected override void Create()
        {

            // Tracks
            this.GeometryTracks = CurvedGeometry(this.Angle, this.Radius, CurvedOrientation.Center, this.RailSpacing, new Point(0,0));

            DrawingGroup drawingTracks = new DrawingGroup();
            drawingTracks.Children.Add(new GeometryDrawing(trackBrush, this.linePen, this.GeometryTracks));
            drawingTracks.Children.Add(this.textDrawing);
            this.drawingTracks = drawingTracks;

            DrawingGroup drawingTracksSelected = new DrawingGroup();
            drawingTracksSelected.Children.Add(new GeometryDrawing(trackBrushSelected, this.linePen, this.GeometryTracks));
            drawingTracksSelected.Children.Add(this.textDrawing);
            this.drawingTracksSelected = drawingTracksSelected;

            // Rail
            this.GeometryRail = CurvedGeometry(this.Angle, this.Radius, CurvedOrientation.Center, this.sleepersSpacing, new Point(0, 0));
            
            DrawingGroup drawingRail = new DrawingGroup();
            if (this.Ballast)
            {
                drawingRail.Children.Add(CurvedBallast(this.Angle, this.Radius, CurvedOrientation.Center, new Point(0, 0)));
            }
            drawingRail.Children.Add(CurvedRail(false, this.Angle, this.Radius, CurvedOrientation.Center, new Point(0, 0)));
            this.drawingRail = drawingRail;

            DrawingGroup drawingRailSelected = new DrawingGroup();
            if (this.Ballast)
            {
                drawingRailSelected.Children.Add(CurvedBallast(this.Angle, this.Radius, CurvedOrientation.Center, new Point(0, 0)));
            }
            drawingRailSelected.Children.Add(CurvedRail(true, this.Angle, this.Radius, CurvedOrientation.Center, new Point(0, 0)));
            this.drawingRailSelected = drawingRailSelected;

            // Terrain
            this.drawingTerrain = drawingRail;

            // dock points
            Point circleCenter = new Point(0, this.Radius);
            this.DockPoints = new List<TrackDockPoint>
            {
                new TrackDockPoint(0, circleCenter - PointExtentions.Circle(-this.Angle / 2, this.Radius),  this.Angle / 2 -  45, this.dockType),
                new TrackDockPoint(1, circleCenter - PointExtentions.Circle( this.Angle / 2, this.Radius), -this.Angle / 2 + 135, this.dockType)
            };
        }
    }
}
