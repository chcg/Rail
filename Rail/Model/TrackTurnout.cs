﻿using Rail.Misc;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Model
{
    public class TrackTurnout : TrackBase
    {
        [XmlAttribute("Length")]
        public double Length { get; set; }

        [XmlAttribute("Radius")]
        public double Radius { get; set; }

        [XmlAttribute("Angle")]
        public double Angle { get; set; }

        [XmlAttribute("Direction")]
        public TrackDirection Direction { get; set; }

        protected override void Create()
        {

            CurvedOrientation orientation = this.Direction == TrackDirection.Left ? CurvedOrientation.Left : CurvedOrientation.Right;

            // Tracks
            this.GeometryTracks = new CombinedGeometry(
                StraitGeometry(this.Length, StraitOrientation.Center, this.RailSpacing),
                this.Direction == TrackDirection.Left ?
                    CurvedGeometry(this.Angle, this.Radius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, this.RailSpacing, new Point(-this.Length / 2, 0)) :
                    CurvedGeometry(this.Angle, this.Radius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right, this.RailSpacing, new Point(this.Length / 2, 0)));

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
                this.Direction == TrackDirection.Left ?
                    CurvedGeometry(this.Angle, this.Radius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, this.sleepersSpacing, new Point(-this.Length / 2, 0)) :
                    CurvedGeometry(this.Angle, this.Radius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right, this.sleepersSpacing, new Point(this.Length / 2, 0)));


            DrawingGroup drawingRail = new DrawingGroup();
            if (this.Ballast)
            {
                drawingRail.Children.Add(StraitBallast(this.Length));
                drawingRail.Children.Add(this.Direction == TrackDirection.Left ?
                    CurvedBallast(this.Angle, this.Radius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, new Point(-this.Length / 2, 0)) :
                    CurvedBallast(this.Angle, this.Radius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right, new Point(this.Length / 2, 0)));
            }
            drawingRail.Children.Add(StraitRail(false, this.Length));
            drawingRail.Children.Add(this.Direction == TrackDirection.Left ?
                    CurvedRail(false, this.Angle, this.Radius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, new Point(-this.Length / 2, 0)) :
                    CurvedRail(false, this.Angle, this.Radius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right, new Point(this.Length / 2, 0)));
            this.drawingRail = drawingRail;

            DrawingGroup drawingRailSelected = new DrawingGroup();
            if (this.Ballast)
            {
                drawingRailSelected.Children.Add(StraitBallast(this.Length));
                drawingRailSelected.Children.Add(this.Direction == TrackDirection.Left ?
                    CurvedBallast(this.Angle, this.Radius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, new Point(-this.Length / 2, 0)) :
                    CurvedBallast(this.Angle, this.Radius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right, new Point(this.Length / 2, 0)));
            }
            drawingRailSelected.Children.Add(StraitRail(true, this.Length));
            drawingRailSelected.Children.Add(this.Direction == TrackDirection.Left ?
                    CurvedRail(true, this.Angle, this.Radius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, new Point(-this.Length / 2, 0)) :
                    CurvedRail(true, this.Angle, this.Radius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right, new Point(this.Length / 2, 0)));
            this.drawingRailSelected = drawingRailSelected;

            // Terrain
            this.drawingTerrain = drawingRail;

            // dock points
            if (this.Direction == TrackDirection.Left)
            {
                Point circleCenter = new Point(-this.Length / 2, -this.Radius);
                this.DockPoints = new List<TrackDockPoint>
                {
                    new TrackDockPoint(0, new Point(-this.Length / 2.0, 0.0), 90 + 45, this.dockType),
                    new TrackDockPoint(1, new Point( this.Length / 2.0, 0.0), 180 + 90 + 45, this.dockType),
                    new TrackDockPoint(2, new Point(-this.Length / 2.0, 0.0).Rotate(-this.Angle, circleCenter), -this.Angle - 45, this.dockType)
                };
            }
            else
            {
                Point circleCenter = new Point(-this.Length / 2, this.Radius);
                this.DockPoints = new List<TrackDockPoint>
                {
                    new TrackDockPoint(0, new Point(-this.Length / 2.0, 0.0), 90 + 45, this.dockType),
                    new TrackDockPoint(1, new Point( this.Length / 2.0, 0.0), 180 + 90 + 45, this.dockType),
                    new TrackDockPoint(2, new Point(-this.Length / 2.0, 0.0).Rotate(this.Angle, circleCenter), this.Angle - 45, this.dockType)
                };
            }
        }
    }
}
