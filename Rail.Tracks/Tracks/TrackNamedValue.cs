using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Rail.Tracks
{
    public class TrackNamedValue
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("Value")]
        public double Value { get; set; }
    }
}
