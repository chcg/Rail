using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Rail.Model
{
    public class TrackLength
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("Length")]
        public double Length { get; set; }
    }
}
