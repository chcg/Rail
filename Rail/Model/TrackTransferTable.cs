using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Model
{
    public class TrackTransferTable : TrackBase
    {

        [XmlAttribute("RailsA")]
        public int RailsA { get; set; }

        [XmlAttribute("RailsB")]
        public int RailsB { get; set; }

        [XmlAttribute("Width")]
        public double Width { get; set; }

        [XmlAttribute("Height")]
        public double Height { get; set; }

        [XmlAttribute("Length")]
        public double Length { get; set; }

        protected override void Create()
        {
            this.GeometryTracks = new RectangleGeometry(new Rect(-this.Width / 2, -this.Height / 2, this.Width, this.Height));

            // Tracks
            DrawingGroup drawingTracks = new DrawingGroup();
            drawingTracks.Children.Add(new GeometryDrawing(trackBrush, linePen, this.GeometryTracks));
            drawingTracks.Children.Add(this.textDrawing);
            this.drawingTracks = drawingTracks;

            // Rail
            DrawingGroup drawingRail = new DrawingGroup();
            drawingRail.Children.Add(TransferTableBackground(this.Width, this.Height, this.Length));
            if (this.Ballast)
            {
                //drawingRail.Children.Add(StraitBallast(this.Length, StraitOrientation.Center, 0, null));
            }
            //drawingRail.Children.Add(StraitRail(this.Length));
            this.drawingRail = drawingRail;

            // Terrain
            this.drawingTerrain = drawingRail;

            this.DockPoints = new List<TrackDockPoint>
            {
                new TrackDockPoint(0, new Point(-this.Length / 2.0, 0.0), 135, this.dockType),
                new TrackDockPoint(1, new Point(+this.Length / 2.0, 0.0), 315, this.dockType)
            };

        }

        private Drawing TransferTableBackground(double width, double height, double length)
        {
            double rim = (width - length) / 2; 
            var drawing = new DrawingGroup();
            drawing.Children.Add(new GeometryDrawing(new SolidColorBrush(Colors.DarkGray), linePen, new RectangleGeometry(new Rect(-width / 2, -height / 2, width, height ))));
            drawing.Children.Add(new GeometryDrawing(new SolidColorBrush(Colors.Gray), linePen, new RectangleGeometry(new Rect(-width / 2, -height / 2, rim, height))));
            drawing.Children.Add(new GeometryDrawing(new SolidColorBrush(Colors.Gray), linePen, new RectangleGeometry(new Rect(width / 2 - rim, -height / 2, rim, height))));
            return drawing;
        }
    }
}
