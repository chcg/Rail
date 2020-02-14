using Rail.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Model
{
    public class TrackType 
    {
        [XmlElement("Name")]
        public XmlMultilanguageString Name { get; set; }

        [XmlElement("Parameter")]
        public TrackParameter Parameter { get; set; }

        [XmlArray("Radii")]
        [XmlArrayItem("Radius")]
        public List<TrackLength> Radii { get; set; } 

        [XmlArray("Lengths")]
        [XmlArrayItem("Length")]
        public List<TrackLength> Lengths { get; set; }

        [XmlArray("Tracks")]
        [XmlArrayItem(typeof(TrackStraight), ElementName = "Straight"),
         XmlArrayItem(typeof(TrackCurved), ElementName = "Curved"),
         XmlArrayItem(typeof(TrackTurnout), ElementName = "Turnout"),
         XmlArrayItem(typeof(TrackCurvedTurnout), ElementName = "CurvedTurnout"),
         XmlArrayItem(typeof(TrackDoubleSlipSwitch), ElementName = "DoubleSlipSwitch"),
         XmlArrayItem(typeof(TrackDoubleTurnout), ElementName = "DoubleTurnout"),
         XmlArrayItem(typeof(TrackYTurnout), ElementName = "YTurnout"),
         XmlArrayItem(typeof(TrackCrossing), ElementName = "Crossing"),
         XmlArrayItem(typeof(TrackBumper), ElementName = "Bumper"),
         XmlArrayItem(typeof(TrackAdapter), ElementName = "Adapter"),
         XmlArrayItem(typeof(TrackTurntable), ElementName = "Turntable"),
         XmlArrayItem(typeof(TrackTransferTable), ElementName = "TransferTable"),
         XmlArrayItem(typeof(TrackEndPiece), ElementName = "EndPiece"),
         XmlArrayItem(typeof(TrackCurvedCircuit), ElementName = "CurvedCircuit"),
         XmlArrayItem(typeof(TrackStraightCircuit), ElementName = "StraightCircuit"),
         XmlArrayItem(typeof(TrackStraightContact), ElementName = "StraightContact"),
         XmlArrayItem(typeof(TrackStraightUncoupler), ElementName = "StraightUncoupler"),
         XmlArrayItem(typeof(TrackStraightIsolating), ElementName = "StraightIsolating"),
         XmlArrayItem(typeof(TrackStraightFeeder), ElementName = "StraightFeeder"),
         XmlArrayItem(typeof(TrackStraightAdjustment), ElementName = "StraightAdjustment"),
         XmlArrayItem(typeof(TrackDoubleCrossover), ElementName = "Suspender"),
         XmlArrayItem(typeof(TrackFlex), ElementName = "Flex"),
         XmlArrayItem(typeof(TrackGroup), ElementName = "Group")]
        public List<TrackBase> Tracks { get; set; }

        [XmlArray("Groups")]
        [XmlArrayItem("Group")]
        public List<TrackGroup> Groups { get; set; }

        public void Update()
        {
            _ = this.Parameter ?? throw new Exception($"Parameter not set");
            this.Tracks.ForEach(track => track.Update(this));
        }
    }
}
