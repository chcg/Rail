using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.Tracks
{
    public enum TrackTypes
    {
        // single
        Straight,
        Curved,
        EndPiece,
        Adapter,
        Adjustment,
        Flex,

        // turnout 
        Turnout,
        CurvedTurnout,
        DoubleSlipSwitch,
        DoubleCrossover,

        // crossing
        Crossing,
        Star,

        // special        
        Turntable,
        TransferTable,
        
        // group
        Group,
    }
}
