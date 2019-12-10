using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Rail.Model
{
    public class TrackType 
    {
        [XmlAttribute("Gauge")]
        public string Gauge { get; set; }

        [XmlAttribute("Type")]
        public string Type { get; set; }
        
        [XmlAttribute("Spacing")]
        public double Spacing { get; set; }
        
        [XmlAttribute("Ballast")]
        public bool Ballast { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("Description")]
        public string Description { get; set; }
                
        [XmlArray("Tracks")]
        [XmlArrayItem(typeof(TrackStraight), ElementName = "Straight"),
         XmlArrayItem(typeof(TrackCurved), ElementName = "Curved"),
         XmlArrayItem(typeof(TrackLeftTurnout), ElementName = "LeftTurnout"),
         XmlArrayItem(typeof(TrackRightTurnout), ElementName = "RightTurnout"),
         XmlArrayItem(typeof(TrackLeftCurvedTurnout), ElementName = "LeftCurvedTurnout"),
         XmlArrayItem(typeof(TrackRightCurvedTurnout), ElementName = "RightCurvedTurnout"),
         XmlArrayItem(typeof(TrackDoubleSlipSwitch), ElementName = "DoubleSlipSwitch"),
         XmlArrayItem(typeof(TrackDoubleTurnout), ElementName = "DoubleTurnout"),
         XmlArrayItem(typeof(TrackYTurnout), ElementName = "YTurnout"),
         XmlArrayItem(typeof(TrackCrossing), ElementName = "Crossing"),
         XmlArrayItem(typeof(TrackBumper), ElementName = "Bumper"),
         XmlArrayItem(typeof(TrackAdapter), ElementName = "Adapter"),
         XmlArrayItem(typeof(TrackTurntable), ElementName = "Turntable"),
         XmlArrayItem(typeof(TrackTransferTable), ElementName = "TransferTable")]

        public List<TrackBase> Tracks { get; set; }

        public void Update()
        {
            this.Tracks.ForEach(track => track.Update(this.Spacing, this.Ballast));
        }
    }
}
