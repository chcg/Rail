using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Xml.Serialization;

namespace Rail.Model
{
    public class TrackItem
    {
        [XmlAttribute("Id")]
        public string Id { get; set; }

        [XmlIgnore]
        public Point Position;

        [XmlAttribute("X")]
        public double X
        {
            get { return this.Position.X; }
            set { this.Position.X = value; }
        }

        [XmlAttribute("Y")]
        public double Y
        {
            get { return this.Position.Y; }
            set { this.Position.Y = value; }
        }

        [XmlAttribute("Angle")]
        public double Angle { get; set; }
    }
}
