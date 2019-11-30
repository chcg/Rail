using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Rail.Model
{
    public class RailDock
    {
        [XmlAttribute("Dock")]
        public int Dock { get; set; }

        [XmlAttribute("Track")]
        public int Track { get; set; }
    }
}
