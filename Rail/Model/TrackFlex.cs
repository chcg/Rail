using Rail.Properties;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Model
{
    public class TrackFlex : TrackStraight
    {
        [XmlIgnore]
        public override string Name
        {
            get
            {
                return $"{Resources.TrackFlex}";
            }
        }

        [XmlIgnore]
        public override string Description
        {
            get
            {
                return $"{this.Article} {Resources.TrackFlex}";
            }
        }

        protected override Geometry CreateGeometry(double spacing)
        {
            return CurvedGeometry(20, 360, CurvedOrientation.Center, spacing, new Point());
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
