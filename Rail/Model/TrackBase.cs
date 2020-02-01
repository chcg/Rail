using Rail.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Model
{
    public abstract class TrackBase
    {
        [XmlIgnore, JsonIgnore]
        public string Id { get; set; }

        [XmlIgnore, JsonIgnore]
        public abstract double RampLength { get; }

        [XmlIgnore, JsonIgnore]
        public abstract string Name { get; }

        [XmlIgnore, JsonIgnore]
        public abstract string Description { get; }

        [XmlIgnore, JsonIgnore]
        public double RailSpacing { get; protected set; }

        [XmlIgnore, JsonIgnore]
        public abstract List<TrackMaterial> Materials { get; }

        [XmlIgnore, JsonIgnore]
        public Geometry GeometryTracks { get; protected set; }

        [XmlIgnore, JsonIgnore]
        public Geometry GeometryRail { get; protected set; }

        [XmlIgnore, JsonIgnore]
        public List<TrackDockPoint> DockPoints { get; protected set; }

        public abstract void Update(TrackType trackType);

        public abstract void Render(DrawingContext drawingContext, RailViewMode viewMode, Brush trackBrush);

        public abstract void RenderSelection(DrawingContext drawingContext, RailViewMode viewMode);
    }
}
