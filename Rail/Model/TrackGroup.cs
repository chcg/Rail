using Rail.Properties;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Model
{
    public class TrackGroup : TrackBase
    {

        [XmlIgnore]
        public override string Name
        {
            get
            {
                return $"Group";
            }

        }

        protected override List<TrackDockPoint> CreateDockPoints()
        {
            return null;
        }

        protected override Geometry CreateGeometry(double spacing)
        {
            return null;
        }

        protected override Drawing CreateRailDrawing(bool isSelected)
        {
            return null;
        }
    }
}
