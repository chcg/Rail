using Rail.Controls;
using Rail.Misc;
using Rail.Properties;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Model
{
    public class TrackGroup : TrackBase
    {
        [XmlElement("Name")]
        public XmlMultilanguageString GroupName { get; set; }

        [XmlElement("Item")]
        public List<TrackGroupItem> GroupItems { get; set; }

        [XmlIgnore, JsonIgnore]
        public override string Name { get { return this.GroupName; } }

        [XmlIgnore, JsonIgnore]
        public override string Description { get { return this.GroupName; } }

        [XmlIgnore, JsonIgnore]
        public override double RampLength { get { return 0; /* TODO sum length */ } }

        [XmlIgnore, JsonIgnore]
        public override List<TrackMaterial> Materials 
        {
            get { return null; /* this.GroupItems?.TODO;*/ } 
        }

        public override void Update(TrackType trackType)
        {
            this.RailSpacing = trackType.Spacing; 
        }

        public override void Render(DrawingContext drawingContext, RailViewMode viewMode, Brush trackBrush)
        {
            // TODO
        }

        public override void RenderSelection(DrawingContext drawingContext, RailViewMode viewMode)
        {

        }
    }
}
