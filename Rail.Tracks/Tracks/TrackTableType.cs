using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.Tracks
{
    [Flags]
    public enum TrackTableType
    {
        // turntable (24 connection tracks)
        Turntable24 = 0x0100000,
        Turntable30 = 0x0100001,
        Turntable40 = 0x0100002,
        Turntable48 = 0x0100003,

        // transfer table (5 of 7 connection tracks)
        Transfer55 = 0x0200000,
        Transfer57 = 0x0200001,

        // segment turntable (3 of 20 connection tracks)
        Segment320 = 0x0400000
    }
}
