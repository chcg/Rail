using Rail.Tracks.Properties;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Tracks
{
    public class TrackEndPiece : TrackBaseSingle
    {
        [XmlIgnore, JsonIgnore]
        public override double RampLength { get { return 0; } }

        [XmlIgnore, JsonIgnore]
        public override string Name
        {
            get
            {
                return $"{Resources.TrackEndPiece}";
            }
        }

        [XmlIgnore, JsonIgnore]
        public override string Description
        {
            get
            {
                return $"{this.Article} {Resources.TrackEndPiece}";
            }
        }

        protected override Geometry CreateGeometry()
        {
            double length = this.RailWidth;

            return StraitGeometry(length, StraitOrientation.Left);
        }

        protected override Drawing CreateRailDrawing()
        {
            double length = this.RailWidth;

            DrawingGroup drawingRail = new DrawingGroup();
            if (this.HasBallast)
            {
                drawingRail.Children.Add(StraitBallast(length, StraitOrientation.Left, 0, null));
            }
            drawingRail.Children.Add(StraitSleepers(length / 2, StraitOrientation.Left, 0, null));
            drawingRail.Children.Add(StraitRail(length / 2, StraitOrientation.Left, 0, null));
            return drawingRail;
        }

        protected override List<TrackDockPoint> CreateDockPoints()
        {
            return new List<TrackDockPoint>
            {
                new TrackDockPoint(0, new Point(this.RailWidth / 2.0, 0.0), 315, this.dockType)
            };
        }
    }
}
