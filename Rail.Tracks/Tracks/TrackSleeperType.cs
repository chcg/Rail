using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Rail.Tracks
{
    public enum TrackSleeperType
    {
        [XmlEnum("WoodenSleepers")]
        Wooden,
        [XmlEnum("ConcreteSleepers")]
        Concrete
    }
}
