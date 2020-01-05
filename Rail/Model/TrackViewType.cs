using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Rail.Model
{
    [Flags]
    public enum TrackViewType
    {
        None = 0,
        Ballast = 0x10,
        Sleepers = 0x0f,

        [XmlEnum("WoodenSleepers")]
        WoodenSleepers = 0x01,
        [XmlEnum("ConcreteSleepers")]
        ConcreteSleepers = 0x02,
        [XmlEnum("BallastWoodenSleepers")]
        BallastWoodenSleepers = Ballast | WoodenSleepers,
        [XmlEnum("BallastConcreteSleepers")]
        BallastConcreteSleepers = Ballast | ConcreteSleepers
    }
}
