using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace Rail.Model
{
    public class Rail
    {
        public Rail()
        { }

        public Rail(int id, double x, double y, double angle, int[] docks)
        {
            this.Id = id;
            this.Position.X = x;
            this.Position.Y = y;
            this.Angle = angle;
        }

        [XmlAttribute("Id")]
        public int Id { get; set; }

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

        [XmlArray("Docks")]
        [XmlArrayItem("Dock")]
        public RailDock[] Docks { get; set; }
    }
}
