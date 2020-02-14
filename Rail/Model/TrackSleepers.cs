using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Rail.Model
{
    public enum TrackSleepers
    {
        Unknown,
        [XmlEnum("WoodenSleepers")]
        WoodenSleepers,
        [XmlEnum("ConcreteSleepers")]
        ConcreteSleepers
    }
}
