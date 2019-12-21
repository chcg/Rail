using Rail.Misc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Model
{
    public class TrackYTurnout : TrackBase
    {
        [XmlAttribute("Radius")]
        public double Radius { get; set; }

        [XmlAttribute("Angle")]
        public double Angle { get; set; }

        protected override void Create()
        {
            double length = this.Radius * 2 * Math.PI * this.Angle / 360.0;

            // Tracks

            this.GeometryTracks = new CombinedGeometry(
                CurvedGeometry(this.Angle, this.Radius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, this.RailSpacing, new Point(-length / 2, 0)),
                CurvedGeometry(this.Angle, this.Radius, CurvedOrientation.Clockwise | CurvedOrientation.Right, this.RailSpacing, new Point(-length / 2, 0)));
            
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
                CurvedGeometry(this.Angle, this.Radius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, this.sleepersSpacing, new Point(-length / 2, 0)),
                CurvedGeometry(this.Angle, this.Radius, CurvedOrientation.Clockwise | CurvedOrientation.Right, this.sleepersSpacing, new Point(-length / 2, 0)));

            DrawingGroup drawingRail = new DrawingGroup();
            if (this.Ballast)
            {
                drawingRail.Children.Add(CurvedBallast(this.Angle, this.Radius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, new Point(-length / 2, 0)));
                drawingRail.Children.Add(CurvedBallast(this.Angle, this.Radius, CurvedOrientation.Clockwise | CurvedOrientation.Right, new Point(-length / 2, 0)));
            }
            drawingRail.Children.Add(CurvedRail(false, this.Angle, this.Radius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, new Point(-length / 2, 0)));
            drawingRail.Children.Add(CurvedRail(false, this.Angle, this.Radius, CurvedOrientation.Clockwise | CurvedOrientation.Right, new Point(-length / 2, 0)));
            this.drawingRail = drawingRail;

            DrawingGroup drawingRailSelected = new DrawingGroup();
            if (this.Ballast)
            {
                drawingRailSelected.Children.Add(CurvedBallast(this.Angle, this.Radius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, new Point(-length / 2, 0)));
                drawingRailSelected.Children.Add(CurvedBallast(this.Angle, this.Radius, CurvedOrientation.Clockwise | CurvedOrientation.Right, new Point(-length / 2, 0)));
            }
            drawingRailSelected.Children.Add(CurvedRail(true, this.Angle, this.Radius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, new Point(-length / 2, 0)));
            drawingRailSelected.Children.Add(CurvedRail(true, this.Angle, this.Radius, CurvedOrientation.Clockwise | CurvedOrientation.Right, new Point(-length / 2, 0)));
            this.drawingRailSelected = drawingRailSelected;

            // Terrain
            this.drawingTerrain = drawingRail;

            //Point circleCenterLeft = new Point(-this.Length / 2, -this.Radius);
            //Point circleCenterRight = new Point(-this.Length / 2, this.Radius);
            this.DockPoints = new List<TrackDockPoint>
            {
                //new TrackDockPoint(-this.Length / 2.0, 0.0, 135),
                //new TrackDockPoint( this.Length / 2.0, 0.0, 315),
                //new TrackDockPoint(new Point(-this.Length / 2.0, 0).Rotate(-this.Angle, circleCenterLeft), -this.Angle - 45),
                //new TrackDockPoint(new Point(-this.Length / 2.0, 0).Rotate( this.Angle, circleCenterRight), this.Angle - 45)
            };
        }
    }
 
}
