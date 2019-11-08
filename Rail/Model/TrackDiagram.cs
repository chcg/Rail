using Rail.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Rail.Model
{
    public class TrackDiagram
    {
        public TrackDiagram()
        {
            this.Width = 3000;
            this.Height = 1300;
            this.Zoom = 1.0;
            this.Tracks = new List<Track>();
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
        public List<Track> Tracks { get; set; }
    }
}
