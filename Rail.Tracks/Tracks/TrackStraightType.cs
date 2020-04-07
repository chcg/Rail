using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.Tracks
{
    public enum TrackStraightType
    {
        // same as TrackCurvedType
        No,
        Circuit,        // contact for locomotive/car pickup shoe
        Contact,        // contact track for middle contace tracks
        Uncoupler,
        Isolating,      // interrups one track
        Separation,     // interrups both tracks for e.q. reverse loop 
        Feeder,       
        Rerailer,
        InterferenceSuppressor,
        // straight additional
        Crossing,
        Adapter
    }
}
