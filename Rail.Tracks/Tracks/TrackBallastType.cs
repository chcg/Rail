using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.Tracks
{
    public enum TrackBallastType
    {
        Unknown,
        /// <summary>
        /// No ballast
        /// </summary>
        No,
        /// <summary>
        /// Light ballast color like Märklin M-Track
        /// </summary>
        Light,
        /// <summary>
        /// Medium ballast color like Roco, Fleischmann and Rohuhan tracks
        /// </summary>
        Medium,
        /// <summary>
        /// Dark ballast color like Märklin C-Track
        /// </summary>
        Dark
    }
}
