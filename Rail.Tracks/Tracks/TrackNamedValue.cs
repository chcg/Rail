using System;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Rail.Tracks
{
    [DebuggerDisplay("{Id} {Name} {Value}")]
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
