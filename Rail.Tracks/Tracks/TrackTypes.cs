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
        Crossing,
        EndPiece,
        Flex,

        // turnout 
        Turnout,
        CurvedTurnout,
        DoubleSlipSwitch,
        DoubleCrossover,

        // special        
        Table,
        
        // group
        Group,
    }
}
