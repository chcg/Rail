using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Rail.Model
{
    public class TrackTransferTable : TrackBase
    {

        [XmlAttribute("RailsA")]
        public int RailsA { get; set; }

        [XmlAttribute("RailsB")]
        public int RailsB { get; set; }

        [XmlAttribute("Width")]
        public double Width { get; set; }

        [XmlAttribute("Height")]
        public double Height { get; set; }

        [XmlAttribute("Length")]
        public double Length { get; set; }

        protected override void Create()
        {
            
        }
    }
}
