using Rail.Controls;
using Rail.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace Rail.Model
{
    [XmlRoot("RailPlan")]
    public class RailPlan : BaseVersionedProject
    {
        private RailPlan()
        {
        }

        public static RailPlan Create()
        {
            RailPlan railPlan = new RailPlan();
            railPlan.PlatePoints = new ObservableCollection<Point>()
            {
                new Point(0, 0),
                new Point(3000, 0),
                new Point(3000, 1500),
                new Point(2000, 1500),
                new Point(2000, 1000),
                new Point(1000, 1000),
                new Point(1000, 1500),
                new Point(0, 1500),
            };
            railPlan.Rails = new ObservableCollection<RailItem>();

            return railPlan;
        }

        public static RailPlan Load(string path)
        {
            RailPlan railPlan = BaseVersionedProject.Load<RailPlan>(path);
            return railPlan;
        }

        [XmlIgnore]
        public int Width { get { return (int)Math.Round(this.PlatePoints.Select(p => p.X).Max()); } }

        [XmlIgnore]
        public int Height { get { return (int)Math.Round(this.PlatePoints.Select(p => p.Y).Max()); } }
        
        /// <summary>
        /// 
        /// </summary>
        [XmlArray("PlatePoints")]
        [XmlArrayItem("PlatePoint")]
        public ObservableCollection<Point> PlatePoints { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [XmlArray("Rails")]
        [XmlArrayItem("Rail")]
        public ObservableCollection<RailItem> Rails { get; set; }
    }
}
