using Rail.Misc;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Model
{
    public class TrackCurvedTurnout : TrackBase
    {
        [XmlAttribute("InnerRadius")]
        public double InnerRadius { get; set; }

        [XmlAttribute("InnerAngle")]
        public double InnerAngle { get; set; }

        [XmlAttribute("OuterRadius")]
        public double OuterRadius { get; set; }

        [XmlAttribute("OuterAngle")]
        public double OuterAngle { get; set; }

        [XmlAttribute("Length")]
        public double Length { get; set; }

        [XmlAttribute("Direction")]
        public TrackDirection Direction { get; set; }

        protected override void Create()
        {
            double width = (this.OuterRadius * 2 * Math.PI * this.OuterAngle / 360.0 + this.Length) / 2;
            double hight = this.OuterRadius * 2 * Math.PI * (this.OuterAngle - 90) / 360.0;

            //Point centerLeft = CurveCenter(this.OuterAngle, this.OuterRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left) + new Vector(this.Length / 2, 0);
            //Point centerRight = CurveCenter(this.OuterAngle, this.OuterRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right) + new Vector(this.Length / 2, 0);

            Point centerLeft = new Point(-width, 0);
            Point centerRight = new Point(width, 0);

            // Tracks
            //this.GeometryTracks = this.Direction == TrackDirection.Left ?
            //    new CombinedGeometry(
            //        CurvedGeometry(this.InnerAngle, this.InnerRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, this.RailSpacing, new Point(-width, 0)),
            //        new CombinedGeometry(
            //            StraitGeometry(this.Length, StraitOrientation.Left, this.RailSpacing, 0, new Point(-width, 0)),
            //            CurvedGeometry(this.OuterAngle, this.OuterRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, this.RailSpacing, new Point(-width + this.Length, 0)))
            //        ) :
            //    new CombinedGeometry(
            //        CurvedGeometry(this.InnerAngle, this.InnerRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right, this.RailSpacing, new Point(width, 0)),
            //        new CombinedGeometry(
            //            StraitGeometry(this.Length, StraitOrientation.Right, this.RailSpacing, 0, new Point(width, 0)),
            //            CurvedGeometry(this.OuterAngle, this.OuterRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right, this.RailSpacing, new Point(width - this.Length, 0)))
            //        );

            this.GeometryTracks = this.Direction == TrackDirection.Left ?
                new CombinedGeometry(
                    CurvedGeometry(this.InnerAngle, this.InnerRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, this.RailSpacing, centerLeft),
                    new CombinedGeometry(
                        StraitGeometry(this.Length, StraitOrientation.Left, this.RailSpacing, 0, centerLeft),
                        CurvedGeometry(this.OuterAngle, this.OuterRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, this.RailSpacing, centerLeft + new Vector(this.Length, 0)))
                    ) :
                new CombinedGeometry(
                    CurvedGeometry(this.InnerAngle, this.InnerRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right, this.RailSpacing, centerRight),
                    new CombinedGeometry(
                        StraitGeometry(this.Length, StraitOrientation.Right, this.RailSpacing, 0, centerRight),
                        CurvedGeometry(this.OuterAngle, this.OuterRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right, this.RailSpacing, centerRight - new Vector(this.Length, 0)))
                    );

            DrawingGroup drawingTracks = new DrawingGroup();
            drawingTracks.Children.Add(new GeometryDrawing(trackBrush, linePen, this.GeometryTracks));
            drawingTracks.Children.Add(this.textDrawing);
            this.drawingTracks = drawingTracks;

            DrawingGroup drawingTracksSelected = new DrawingGroup();
            drawingTracksSelected.Children.Add(new GeometryDrawing(trackBrushSelected, linePen, this.GeometryTracks));
            drawingTracksSelected.Children.Add(this.textDrawing);
            this.drawingTracksSelected = drawingTracksSelected;

            // Rail
            this.GeometryRail = this.Direction == TrackDirection.Left ?
                new CombinedGeometry(
                    CurvedGeometry(this.InnerAngle, this.InnerRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, this.sleepersSpacing, centerLeft),
                    new CombinedGeometry(
                        StraitGeometry(this.Length, StraitOrientation.Left, this.sleepersSpacing, 0, centerLeft),
                        CurvedGeometry(this.OuterAngle, this.OuterRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, this.sleepersSpacing, centerLeft + new Vector(this.Length, 0)))
                    ) :
                new CombinedGeometry(
                    CurvedGeometry(this.InnerAngle, this.InnerRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right, this.sleepersSpacing, centerRight),
                    new CombinedGeometry(
                        StraitGeometry(this.Length, StraitOrientation.Right, this.sleepersSpacing, 0, centerRight),
                        CurvedGeometry(this.OuterAngle, this.OuterRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right, this.sleepersSpacing, centerRight - new Vector(this.Length, 0)))
                    );

            DrawingGroup drawingRail = new DrawingGroup();
            if (this.Ballast)
            {
                if (this.Direction == TrackDirection.Left)
                {
                    drawingRail.Children.Add(CurvedBallast(this.InnerAngle, this.InnerRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, centerLeft));
                    drawingRail.Children.Add(StraitBallast(this.Length, StraitOrientation.Left, 0, centerLeft));
                    drawingRail.Children.Add(CurvedBallast(this.OuterAngle, this.OuterRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, centerLeft + new Vector(this.Length, 0)));
                }
                else
                {
                    drawingRail.Children.Add(CurvedBallast(this.InnerAngle, this.InnerRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right, centerRight));
                    drawingRail.Children.Add(StraitBallast(this.Length, StraitOrientation.Right, 0, centerRight));
                    drawingRail.Children.Add(CurvedBallast(this.OuterAngle, this.OuterRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right, centerRight - new Vector(this.Length, 0)));
                }
            }
            if (this.Direction == TrackDirection.Left)
            {
                drawingRail.Children.Add(CurvedRail(false, this.InnerAngle, this.InnerRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, centerLeft));
                drawingRail.Children.Add(StraitRail(false, this.Length, StraitOrientation.Left, 0, centerLeft));
                drawingRail.Children.Add(CurvedRail(false, this.OuterAngle, this.OuterRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, centerLeft + new Vector(this.Length, 0)));
            }
            else
            {
                drawingRail.Children.Add(CurvedRail(false, this.InnerAngle, this.InnerRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right, centerRight));
                drawingRail.Children.Add(StraitRail(false, this.Length, StraitOrientation.Right, 0, centerRight));
                drawingRail.Children.Add(CurvedRail(false, this.OuterAngle, this.OuterRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right, centerRight - new Vector(this.Length, 0)));
            }
            this.drawingRail = drawingRail;

            DrawingGroup drawingRailSelected = new DrawingGroup();
            if (this.Ballast)
            {
                if (this.Direction == TrackDirection.Left)
                {
                    drawingRailSelected.Children.Add(CurvedBallast(this.InnerAngle, this.InnerRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, centerLeft));
                    drawingRailSelected.Children.Add(StraitBallast(this.Length, StraitOrientation.Left, 0, centerLeft));
                    drawingRailSelected.Children.Add(CurvedBallast(this.OuterAngle, this.OuterRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, centerLeft + new Vector(this.Length, 0)));
                }
                else
                {
                    drawingRailSelected.Children.Add(CurvedBallast(this.InnerAngle, this.InnerRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right, centerRight));
                    drawingRailSelected.Children.Add(StraitBallast(this.Length, StraitOrientation.Right, 0, centerRight));
                    drawingRailSelected.Children.Add(CurvedBallast(this.OuterAngle, this.OuterRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right, centerRight - new Vector(this.Length, 0)));
                }
            }
            if (this.Direction == TrackDirection.Left)
            {
                drawingRailSelected.Children.Add(CurvedRail(true, this.InnerAngle, this.InnerRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, centerLeft));
                drawingRailSelected.Children.Add(StraitRail(true, this.Length, StraitOrientation.Left, 0, centerLeft));
                drawingRailSelected.Children.Add(CurvedRail(true, this.OuterAngle, this.OuterRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Left, centerLeft + new Vector(this.Length, 0)));
            }
            else
            {
                drawingRailSelected.Children.Add(CurvedRail(true, this.InnerAngle, this.InnerRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right, centerRight));
                drawingRailSelected.Children.Add(StraitRail(true, this.Length, StraitOrientation.Right, 0, centerRight));
                drawingRailSelected.Children.Add(CurvedRail(true, this.OuterAngle, this.OuterRadius, CurvedOrientation.Counterclockwise | CurvedOrientation.Right, centerRight - new Vector(this.Length, 0)));
            }
            this.drawingRailSelected = drawingRailSelected;

            // Terrain
            this.drawingTerrain = drawingRail;

            this.DockPoints = new List<TrackDockPoint>
            {
                new TrackDockPoint(0, new Point(this.InnerRadius, 0), 225.0, this.dockType),
                new TrackDockPoint(1, new Point(this.InnerRadius, 0), 45.0, this.dockType)
            };
        }
    }
}
