using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.Model
{
    public class RailDockPointDockComparer : IEqualityComparer<RailDockPoint>
    {
        private readonly double distance;
        private Guid layer;
        public RailDockPointDockComparer(double distance, Guid layer)
        {
            this.distance = distance;
            this.layer = layer;
        }

        public bool Equals(RailDockPoint x, RailDockPoint y)
        {
            var res = x.Distance(y) < distance && !x.IsDocked && !y.IsDocked && x.DockType == y.DockType && (layer == Guid.Empty ? true : (x.Layer == layer && x.Layer == layer));
            //if (res)
            //{
            //    var r = !x.IsDocked && !y.IsDocked;
            //    var r2 = x.DockType == y.DockType;
            //    var r3 = (layer == Guid.Empty ? true : (x.Layer == layer && x.Layer == layer));
            //}
            return res;
        }

        public int GetHashCode(RailDockPoint obj)
        {
            // call always Equals
            return 0;
        }
    }
}
