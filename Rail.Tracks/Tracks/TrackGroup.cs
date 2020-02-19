using Rail.Tracks.Misc;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Tracks
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
            this.RailWidth = trackType.Parameter.RailWidth; 
        }

        public override void RenderTrack(DrawingContext drawingContext, Brush trackBrush)
        {
            // TODO
        }

        public override void RenderRail(DrawingContext drawingContext)
        {
            // TODO
        }

        public override void RenderSelection(DrawingContext drawingContext)
        {

        }
    }
}
