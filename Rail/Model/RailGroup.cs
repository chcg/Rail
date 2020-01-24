using Rail.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Model
{
    public class RailGroup : RailBase
    {
        public RailGroup()
        { }

        public RailGroup(IEnumerable<RailBase> railItems)
        {
            this.Rails = railItems.Cast<RailItem>().ToList();
            this.Layer = this.Rails.First().Layer;
        }

        /// <summary>
        /// 
        /// </summary>
        [XmlArray("Rails")]
        [XmlArrayItem("Rail")]
        public List<RailItem> Rails { get; set; }

        [XmlIgnore]
        public override List<TrackMaterial> Materials
        {
            get { return this.Rails.SelectMany(r => r.Materials).ToList(); }
        }

        public override void DrawRailItem(DrawingContext drawingContext, RailViewMode viewMode, RailLayer layer)
        {
            this.Rails.ForEach(r => r.DrawRailItem(drawingContext, viewMode, layer));
        }

        public override void DrawDockPoints(DrawingContext drawingContext)
        { }

        public override bool IsInside(Point point, RailViewMode viewMode)
        {
            // TODO
            return false;
        }

        public override bool IsInside(Rect rec, RailViewMode viewMode)
        {
            // TODO
            return false;
        }
}
}
