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
        [XmlElement("GaugeName"), JsonPropertyName("GaugeName")]
        public string GaugeName { get; set; }

        //[XmlAttribute("Type"), JsonPropertyName("Type")]
        //public string Type { get; set; }

        [XmlElement("DockType"), JsonPropertyName("DockType")]
        public string DockType { get; set; }

        [XmlElement("GaugeWidth"), JsonPropertyName("GaugeWidth")]
        public double GaugeWidth { get; set; }

        [XmlElement("TrackWidth"), JsonPropertyName("TrackWidth")]
        public double TrackWidth { get; set; }

        //[XmlElement("ViewType"), JsonPropertyName("ViewType")]
        //public TrackViewType ViewType { get; set; }

        [XmlElement("Sleepers"), JsonPropertyName("Sleepers")]
        public TrackSleepers Sleepers { get; set; }

        [XmlElement("Bedding"), JsonPropertyName("Bedding")]
        public bool HasBedding { get; set; }
    }
}
