using Rail.Controls;
using Rail.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            this.Width1 = 1000;
            this.Width2 = 1000;
            this.Width3 = 1000;
            this.Height1 = 1500;
            this.Height2 = 1000;
            this.Height3 = 1500;
            //this.Zoom = 1.0;
            this.Rails = new ObservableCollection<RailItem>();
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
        /// Width 1 of the rail plan ground plate
        /// </summary>
        [XmlAttribute("Width1")]
        public int Width1 { get; set; }

        /// <summary>
        /// Width 1 of the rail plan ground plate
        /// </summary>
        [XmlAttribute("Width2")]
        public int Width2 { get; set; }

        /// <summary>
        /// Width 1 of the rail plan ground plate
        /// </summary>
        [XmlAttribute("Width3")]
        public int Width3 { get; set; }

        [XmlIgnore]
        public int Width { get { return this.Width1 + this.Width2 + this.Width3; } }

        /// <summary>
        /// Height 1 of the rail plan ground plate
        /// </summary>
        [XmlAttribute("Height1")]
        public int Height1 { get; set; }

        /// <summary>
        /// Height 2 of the rail plan ground plate
        /// </summary>
        [XmlAttribute("Height2")]
        public int Height2 { get; set; }

        /// <summary>
        /// Height 3 of the rail plan ground plate
        /// </summary>
        [XmlAttribute("Height3")]
        public int Height3 { get; set; }

        [XmlIgnore]
        public int Height { get { return Math.Max(this.Height1, Math.Max(this.Height2, this.Height3)); } }

        ///// <summary>
        /////  Zoom factor to display the rail plan
        ///// </summary>
        //[XmlAttribute("Zoom")]
        //public double Zoom { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlArray("Rails")]
        [XmlArrayItem("Rail")]
        public ObservableCollection<RailItem> Rails { get; set; }
    }
}
