using Rail.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Model
{
    public class TrackType 
    {
        /// <summary>
        /// Manufacturer of the track
        /// </summary>
        [XmlAttribute("Manufacturer")]
        public string Manufacturer { get; set; }

        /// <summary>
        /// Gauge of the Track H0, TT, N, 1, Z, G, H0e
        /// </summary>
        [XmlAttribute("Gauge")]
        public string Gauge { get; set; }

        [XmlAttribute("Type")]
        public string Type { get; set; }

        [XmlAttribute("DockType")]
        public string DockType { get; set; }

        [XmlAttribute("Spacing")]
        public double Spacing { get; set; }
        
        [XmlAttribute("ViewType")]
        public TrackViewType ViewType { get; set; }
        
        //[XmlAttribute("BallastColor", Type = typeof(XmlColor))]
        //public Color BallastColor { get; set; }        
        
        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("Description")]
        public string Description { get; set; }
                
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
         XmlArrayItem(typeof(TrackSuspender), ElementName = "Suspender"),
         XmlArrayItem(typeof(TrackFlex), ElementName = "Flex")]
        public List<TrackBase> Tracks { get; set; }

        [XmlArray("Groups")]
        [XmlArrayItem("Group")]
        public List<TrackGroup> Groups { get; set; }

        public void Update()
        {
            this.Tracks.ForEach(track => track.Update(this));
        }
    }
}
