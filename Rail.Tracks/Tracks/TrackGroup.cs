using Rail.Tracks.Misc;
using Rail.Tracks.Properties;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Tracks
{
    public class TrackGroup : TrackBase
    {
        public TrackGroup()
        {
            this.GroupName = new XmlMultilanguageString(Resources.TrackNewGroup);
        }

        [XmlElement("Name")]
        public XmlMultilanguageString GroupName { get; set; }

        [XmlElement("Item")]
        public List<TrackGroupItem> GroupItems { get; set; }

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

            this.Name = this.GroupName;
            this.Description = this.GroupName;
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
