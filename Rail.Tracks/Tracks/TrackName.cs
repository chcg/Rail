using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Xml.Serialization;

namespace Rail.Tracks
{
    [DebuggerDisplay("{Id} {Name}")]
    public class TrackName
    {
        public TrackName()
        {
            this.Id = Guid.NewGuid();
        }

        [XmlAttribute("Id")]
        public Guid Id { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }

        
    }
}

