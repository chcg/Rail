using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Rail.Model
{
    public class TrackParameter
    {
        /// <summary>
        /// Manufacturer of the track
        /// </summary>
        [XmlElement("Manufacturer"), JsonPropertyName("Manufacturer")]
        public string Manufacturer { get; set; }

        /// <summary>
        /// Gauge of the Track H0, TT, N, 1, Z, G, H0e
        /// </summary>
        [XmlElement("Gauge"), JsonPropertyName("Gauge")]
        public TrackGauge Gauge { get; set; }

        [XmlElement("DockType"), JsonPropertyName("DockType")]
        public string DockType { get; set; }

        [XmlElement("RailType"), JsonPropertyName("RailType")]
        public TrackRailType RailType { get; set; }

        [XmlElement("RailWidth"), JsonPropertyName("RailWidth")]
        public double RailWidth { get; set; }

        [XmlElement("SleeperType"), JsonPropertyName("SleeperType")]
        public TrackSleeperType SleeperType { get; set; }
        
        [XmlElement("SleeperWidth"), JsonPropertyName("SleeperWidth")]
        public double SleeperWidth { get; set; }

        [XmlElement("BallastType"), JsonPropertyName("BallastType")]
        public TrackBallastType BallastType { get; set; }
        
        [XmlElement("BallastWidth"), JsonPropertyName("BallastWidth")]
        public double BallastWidth { get; set; }

        

        
    }
}
