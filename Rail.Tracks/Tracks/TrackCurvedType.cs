using System.Collections.Generic;
using System.Text;

namespace Rail.Tracks
{
    public enum TrackCurvedType
    {
        No,
        Circuit,        // contact for locomotive/car pickup shoe
        Contact,        // contact track for middle contace tracks
        Uncoupler,
        Isolating,      // interrups one track
        Separation,     // interrups both tracks for e.q. reverse loop 
        Feeder,
        Rerailer,
        InterferenceSuppressor
    }
}

