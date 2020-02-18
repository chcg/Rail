using Rail.Tracks.Properties;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Model
{
    public class TrackFlex : TrackStraight
    {
        [XmlIgnore, JsonIgnore]
        public override string Name
        {
            get
            {
                return $"{Resources.TrackFlex}";
            }
        }

        [XmlIgnore, JsonIgnore]
        public override string Description
        {
            get
            {
                return $"{this.Article} {Resources.TrackFlex}";
            }
        }

        protected override Geometry CreateGeometry()
        {
            return CurvedGeometry(20, 360, CurvedOrientation.Center, new Point());
        }

        protected override Drawing CreateRailDrawing()
        {
            return null;
        }

        protected override List<TrackDockPoint> CreateDockPoints()
        {
            return null;
        }
    }
}
