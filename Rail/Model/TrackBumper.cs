using Rail.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Model
{
    public class TrackBumper : TrackStraight
    {
        [XmlAttribute("Lantern")]
        public bool Lantern { get; set; }

        [XmlIgnore, JsonIgnore]
        public override string Name
        {
            get
            {
                string lantern = this.Lantern ? Resources.TrackWithLantern : String.Empty;
                return $"{Resources.TrackBumper} {lantern}";
            }
        }

        [XmlIgnore, JsonIgnore]
        public override string Description
        {
            get
            {
                string lantern = this.Lantern ? Resources.TrackWithLantern : String.Empty;
                return $"{this.Article} {Resources.TrackBumper} {lantern}";
            }
        }

        protected override Geometry CreateGeometry(double spacing)
        {
            return StraitGeometry(this.Length, StraitOrientation.Center, spacing);
        }

        protected override Drawing CreateRailDrawing()
        {
            DrawingGroup drawingRail = new DrawingGroup();
            if (this.ViewType.HasFlag(TrackViewType.Ballast))
            {
                drawingRail.Children.Add(StraitBallast(this.Length, StraitOrientation.Center, 0, null));
            }
            drawingRail.Children.Add(StraitSleepers(this.Length));
            drawingRail.Children.Add(StraitRail(this.Length));
            return drawingRail;
        }

        protected override List<TrackDockPoint> CreateDockPoints()
        {
            return new List<TrackDockPoint>
            {
                new TrackDockPoint(0, new Point(this.Length / 2.0, 0.0), 315, this.dockType)
            };
        }
    }
}
