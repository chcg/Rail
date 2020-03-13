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
        StraightAdjustment,
        Flex,

        // turnout 
        Turnout,
        CurvedTurnout,
        DoubleSlipSwitch,
        ThreeWayTurnout,
        YTurnout,
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
