using Rail.Tracks.Properties;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Tracks
{
    public class TrackTransferTable : TrackStraight
    {

        [XmlElement("RailsA")]
        public int RailsA { get; set; }

        [XmlElement("RailsB")]
        public int RailsB { get; set; }

        [XmlElement("Width")]
        public double Width { get; set; }

        [XmlElement("Height")]
        public double Height { get; set; }

        public override void Update(TrackType trackType)
        {
            base.Update(trackType);

            this.Name = $"{Resources.TrackTransferTable}";
            this.Description = $"{this.Article} {Resources.TrackTransferTable}";
        }

        protected override Geometry CreateGeometry()
        {
            return new RectangleGeometry(new Rect(-this.Width / 2, -this.Height / 2, this.Width, this.Height));
        }

        protected override Drawing CreateRailDrawing()
        {
            double rim = (this.Width - this.Length) / 2;

            DrawingGroup drawingRail = new DrawingGroup();
            // background
            drawingRail.Children.Add(new GeometryDrawing(new SolidColorBrush(Colors.DarkGray), linePen, new RectangleGeometry(new Rect(-Width / 2, -Height / 2, Width, Height))));
            drawingRail.Children.Add(new GeometryDrawing(new SolidColorBrush(Colors.Gray), linePen, new RectangleGeometry(new Rect(-Width / 2, -Height / 2, rim, Height))));
            drawingRail.Children.Add(new GeometryDrawing(new SolidColorBrush(Colors.Gray), linePen, new RectangleGeometry(new Rect(Width / 2 - rim, -Height / 2, rim, Height))));
            if (this.HasBallast)
            {
                //drawingRail.Children.Add(StraitBallast(this.Length, StraitOrientation.Center, 0, null));
            }
            //drawingRail.Children.Add(StraitRail(this.Length));
            return drawingRail;
        }

        protected override List<TrackDockPoint> CreateDockPoints()
        {
            return new List<TrackDockPoint>
            {
                new TrackDockPoint(0, new Point(-this.Length / 2.0, 0.0), 135, this.dockType),
                new TrackDockPoint(1, new Point(+this.Length / 2.0, 0.0), 315, this.dockType)
            };
        }
    }
}
