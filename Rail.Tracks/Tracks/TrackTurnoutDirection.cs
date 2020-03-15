using Rail.Tracks.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Rail.Tracks
{
    public enum TrackTurnoutDirection
    {
        [Description(nameof(Resources.TrackLeft))]
        Left,
        [Description(nameof(Resources.TrackRight))]
        Right,
        [Description(nameof(Resources.TrackY))]
        Y,
        [Description(nameof(Resources.TrackThree))]
        Three
    }
}
