using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Rail.Tracks
{
    public class TrackNamedValue
    {
        public TrackNamedValue()
        {
            this.Id = Guid.NewGuid();
        }

        [XmlAttribute("Id")]
        public Guid Id { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("Value")]
        public double Value { get; set; }
    }
}
