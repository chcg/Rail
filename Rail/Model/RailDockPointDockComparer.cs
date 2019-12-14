using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.Model
{
    public class RailDockPointDockComparer : IEqualityComparer<RailDockPoint>
    {
        private double distance;
        private ushort layer;
        public RailDockPointDockComparer(double distance, ushort layer)
        {
            this.distance = distance;
            this.layer = layer;
        }

        public bool Equals(RailDockPoint x, RailDockPoint y)
        {
            return x.Distance(y) < distance && !x.IsDocked && !y.IsDocked && x.DockType != y.DockType && (layer == 0 ? true : (x.Layer == layer && x.Layer == layer))
                ;
        }

        public int GetHashCode(RailDockPoint obj)
        {
            return obj.GetHashCode();
        }
    }
}
