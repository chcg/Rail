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
            this.GeometryTracks = new CombinedGeometry(
                CreateLeftTurnoutGeometry(0, this.Angle, this.Radius),
                CreateRightTurnoutGeometry(0, this.Angle, this.Radius));

            // Tracks
            DrawingGroup drawingTracks = new DrawingGroup();
            drawingTracks.Children.Add(new GeometryDrawing(trackBrush, linePen, this.GeometryTracks));
            drawingTracks.Children.Add(this.textDrawing);
            this.drawingTracks = drawingTracks;

            // Rail
            DrawingGroup drawingRail = new DrawingGroup();
            if (this.Ballast)
            {
                //drawingRail.Children.Add(CurvedBallast(this.Angle, this.Radius, CurvedOrientation.Center | CurvedOrientation.Clockwise, 0, new Point(-this.Length / 2, 0)));
                //drawingRail.Children.Add(CurvedBallast(this.Angle, this.Radius, CurvedOrientation.Center | CurvedOrientation.Counterclockwise, 0, new Point(-this.Length / 2, 0)));
            }
            //drawingRail.Children.Add(CurvedRail(this.Angle, this.Radius, new Point(-this.Length / 2, 0), TrackDirection.Left));
            //drawingRail.Children.Add(CurvedRail(this.Angle, this.Radius, new Point(-this.Length / 2, 0), TrackDirection.Right));
            //this.drawingRail = drawingRail;

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
