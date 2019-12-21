using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace Rail.Model
{
    public class TrackGroup : TrackBase
    {
        

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
