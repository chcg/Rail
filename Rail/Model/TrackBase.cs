using Rail.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Model
{
    public abstract class TrackBase
    {
        [XmlIgnore]
        public string Id { get; set; }

        [XmlIgnore]
        public abstract string Name { get; }

        [XmlIgnore]
        public abstract string Description { get; }

        [XmlIgnore]
        public double RailSpacing { get; protected set; }

        [XmlIgnore]
        public abstract List<TrackMaterial> Materials { get; }

        [XmlIgnore]
        public Geometry GeometryTracks { get; protected set; }

        [XmlIgnore]
        public Geometry GeometryRail { get; protected set; }

        [XmlIgnore]
        public List<TrackDockPoint> DockPoints { get; protected set; }

        public abstract void Update(TrackType trackType);

        public abstract void Render(DrawingContext drawingContext, RailViewMode viewMode, bool isSelected, Brush trackBrush);
    }
}
