using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Rail.Model
{
    public class TrackAdapter : TrackStraight
    {
        [XmlAttribute("DockType")]
        public string DockType { get; set; }
        
    }
}
