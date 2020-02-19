using Rail.Misc;
using Rail.Mvvm;
using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Model
{
    [XmlRoot("RailPlan")]
    public class RailPlan : BaseProject
    {
        private RailPlan()
        {
        }

        public static RailPlan Create()
        {
            return new RailPlan
            {
                PlatePoints = new List<Point>()
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
                Layers = new List<RailLayer>
                {
                    new RailLayer{ Id = Guid.NewGuid(), Name = "Shadow Station", Height = 300, TrackColor = Colors.LightGray, PlateColor = Colors.Gray },
                    new RailLayer{ Id = Guid.NewGuid(), Name = "Ground Plate", Height = 100, TrackColor = Colors.White, PlateColor = Colors.Green },
                    new RailLayer{ Id = Guid.NewGuid(), Name = "Tunnel", Height = 100, TrackColor = Colors.Blue, PlateColor = Colors.Transparent },
                    new RailLayer{ Id = Guid.NewGuid(), Name = "Bridge", Height = 100, TrackColor = Colors.LightBlue, PlateColor = Colors.Transparent }
                },
                Rails = new List<RailBase>()
            };
        }

        public static RailPlan Load(string path, Dictionary<string, TrackBase> trackDict)
        {
            RailPlan railPlan = BaseProject.Load<RailPlan>(path);

            // link tracks
            foreach (RailBase item in railPlan.Rails)
            {
                if (item is RailItem railItem)
                {
                    // set track
                    railItem.Track = trackDict[railItem.TrackId];
                    // set dock points
                    railItem.DockPoints.ForEach((railDockPoint, index) =>
                    {
                        railDockPoint.Update(railItem, railItem.Track.DockPoints[index]);
                    });
                }
            }
            // link dock points
            var list = railPlan.Rails.SelectMany(r => r.DockPoints).Where(dp => dp.DockedWithId != Guid.Empty).ToList();
            foreach (RailDockPoint dp in list)
            {
                RailDockPoint x = list.Single(i => i.Id == dp.DockedWithId);
                x.Dock(dp);

            }
            return railPlan;
        }

        [XmlIgnore, JsonIgnore]
        public int Width { get { return (int)Math.Round(this.PlatePoints.Select(p => p.X).Max()); } }

        [XmlIgnore, JsonIgnore]
        public int Height { get { return (int)Math.Round(this.PlatePoints.Select(p => p.Y).Max()); } }

        /// <summary>
        /// 
        /// </summary>
        [XmlArray("PlatePoints")]
        [XmlArrayItem("PlatePoint")]
        public List<Point> PlatePoints { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlArray("Layers")]
        [XmlArrayItem("Layer")]
        public List<RailLayer> Layers { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlArray("Rails")]
        [XmlArrayItem("Rail", typeof(RailItem)),
         XmlArrayItem("Group", typeof(RailGroup))]
        public List<RailBase> Rails { get; set; }


        [XmlIgnore, JsonIgnore]
        public IEnumerable<RailBase> SelectedRails { get { return this.Rails.Where(r => r.IsSelected); } }

        [XmlIgnore, JsonIgnore]
        public IEnumerable<RailBase> SelectedRampRails 
        { 
            get 
            {
                // check if all items are RailItems
                var selectedRailBase = SelectedRails.ToList();
                if (!selectedRailBase.All(r => r is RailItem))
                {
                    // not all items are RailItems
                    return null;
                }
                var selected = selectedRailBase.Cast<RailItem>().ToList();

                // find singe extern docked RailItem
                var externDocked = selected.SelectMany(r => r.DockPoints).Where(d => d.DockedWith != null && !selected.Contains(d.DockedWith.RailItem)).ToList();
                if (!externDocked.One())
                {
                    // more or less than one external dock
                    return null;
                }
                RailItem item = (RailItem)externDocked.Single().RailItem;

                // sort ramp items


                List<RailItem> rampList = new List<RailItem>();
                while (item != null)
                {
                    rampList.Add(item);
                    item = (RailItem)item.DockPoints.Where(d => d.IsDocked && selected.Contains(d.DockedWith.RailItem) && !rampList.Contains(d.DockedWith.RailItem)).SingleOrDefault()?.DockedWith.RailItem;
                }
                return rampList;
            } 
        }

        public RailPlan Clone()
        {
            // clone RailPlan with rails tree
            var clone = new RailPlan()
            {
                PlatePoints = this.PlatePoints.ToList(),
                Layers = this.Layers.Select(l => l.Clone()).ToList(),
                Rails = this.Rails.Select(l => l.Clone()).ToList()
            };

            // clone dock point links
            RailDockPoint.CloneDockPointLinks();
            return clone;
        }
    }
}
