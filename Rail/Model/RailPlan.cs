using Rail.Controls;
using Rail.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Rail.Model
{
    [XmlRoot("RailPlan")]
    public class RailPlan : BaseVersionedProject
    {
        private RailPlan()
        {
            this.Width = 3000;
            this.Height = 1300;
            this.Zoom = 1.0;
            this.Rails = new List<RailItem>();
        }

        public static RailPlan Create()
        {
            return new RailPlan();
        }

        public static RailPlan Load(string path)
        {
            RailPlan railPlan = BaseVersionedProject.Load<RailPlan>(path);
            return railPlan;
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
        [XmlArray("Rails")]
        [XmlArrayItem("Rail")]
        public List<RailItem> Rails { get; set; }
    }
}
