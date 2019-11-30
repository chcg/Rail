using Rail.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Rail.Model
{
    public class RailPlan
    {
        public RailPlan()
        {
            this.Width = 3000;
            this.Height = 1300;
            this.Zoom = 1.0;
            this.Tracks = new List<Rail>();
        }

        /// <summary>
        /// Width of the rail plan ground plate
        /// </summary>
        [XmlAttribute("Width")]
        public int Width { get; set; }

        /// <summary>
        /// Height of the rail plan ground plate
        /// </summary>
        [XmlAttribute("Height")]
        public int Height { get; set; }

        /// <summary>
        ///  Zoom factor to display the rail plan
        /// </summary>
        [XmlAttribute("Zoom")]
        public double Zoom { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlArray("Tracks")]
        [XmlArrayItem("Track")]
        public List<Rail> Tracks { get; set; }
    }
}
