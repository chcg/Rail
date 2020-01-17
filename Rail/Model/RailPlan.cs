using Rail.Controls;
using Rail.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
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
            return new RailPlan
            {
                PlatePoints = new ObservableCollection<Point>()
                {
                    new Point(0, 0),
                    new Point(3000, 0),
                    new Point(3000, 1500),
                    new Point(2000, 1500),
                    new Point(2000, 1000),
                    new Point(1000, 1000),
                    new Point(1000, 1500),
                    new Point(0, 1500),
                },
                Layers = new ObservableCollection<RailLayer>
                {
                    new RailLayer{ Id = Guid.NewGuid(), Name = "Shadow Station", Height = 300, TrackColor = Colors.LightGray, PlateColor = Colors.Gray },
                    new RailLayer{ Id = Guid.NewGuid(), Name = "Ground Plate", Height = 100, TrackColor = Colors.White, PlateColor = Colors.Green },
                    new RailLayer{ Id = Guid.NewGuid(), Name = "Tunnel", Height = 100, TrackColor = Colors.Blue, PlateColor = Colors.Transparent },
                    new RailLayer{ Id = Guid.NewGuid(), Name = "Bridge", Height = 100, TrackColor = Colors.Yellow, PlateColor = Colors.Transparent }
                },
                Rails = new ObservableCollection<RailItem>()
            };
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
        [XmlArray("Layers")]
        [XmlArrayItem("Layer")]
        public ObservableCollection<RailLayer> Layers { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlArray("Rails")]
        [XmlArrayItem("Rail")]
        public ObservableCollection<RailItem> Rails { get; set; }
    }
}
