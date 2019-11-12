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

        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("Description")]
        public string Description { get; set; }
                

        [XmlElement(typeof(StraightTrack), ElementName = "Straight"),
         XmlElement(typeof(CurvedTrack), ElementName = "Curved"),
         XmlElement(typeof(LeftTurnoutTrack), ElementName = "LeftTurnout"),
         XmlElement(typeof(RightTurnoutTrack), ElementName = "RightTurnout"),
         XmlElement(typeof(LeftCurvedTurnoutTrack), ElementName = "LeftCurvedTurnout"),
         XmlElement(typeof(RightCurvedTurnoutTrack), ElementName = "RightCurvedTurnout"),
         XmlElement(typeof(DoubleSlipSwitchTrack), ElementName = "DoubleSlipSwitch"),
         XmlElement(typeof(DoubleTurnoutTrack), ElementName = "DoubleTurnout"),
         XmlElement(typeof(CrossingTrack), ElementName = "Crossing"),
         XmlElement(typeof(BumperTrack), ElementName = "Bumper")]
        public BaseTrack[] Tracks { get; set; }
    }
}
