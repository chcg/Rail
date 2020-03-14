using Rail.Tracks.Properties;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Tracks
{
    public class TrackFlex : TrackStraight
    {
        [XmlElement("FlexType")]
        public TrackFlexType FlexType { get; set; }

        [XmlElement("MinLength")]
        public double MinLength { get; set; }

        [XmlElement("MaxLength")]
        public double MaxLength { get; set; }

        [XmlElement("MinRadius")]
        public double MinRadius { get; set; }

        public override void Update(TrackType trackType)
        {
            base.Update(trackType);

            this.Name = $"{Resources.TrackFlex}";
            this.Description = $"{this.Article} {Resources.TrackFlex}";
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
