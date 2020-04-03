using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Rail.Tracks
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
        public Guid DockType { get; set; }

        [XmlElement("RailType"), JsonPropertyName("RailType")]
        public TrackRailType RailType { get; set; }

        [XmlElement("SleeperType"), JsonPropertyName("SleeperType")]
        public TrackSleeperType SleeperType { get; set; }
        
        [XmlElement("SleeperWidth"), JsonPropertyName("SleeperWidth")]
        public double SleeperWidth { get; set; }

        [XmlElement("BallastType"), JsonPropertyName("BallastType")]
        public TrackBallastType BallastType { get; set; }
        
        [XmlElement("BallastWidth"), JsonPropertyName("BallastWidth")]
        public double BallastWidth { get; set; }

        [XmlElement("WagonMaxWidth"), JsonPropertyName("WagonMaxWidth")]
        public double WagonMaxWidth { get; set; }

        [XmlElement("WagonMaxBogieDistance"), JsonPropertyName("WagonMaxBogieDistance")]
        public double WagonMaxBogieDistance { get; set; }

        [XmlElement("WagonMaxBogieFrontDistance"), JsonPropertyName("WagonMaxBogieFrontDistance")]
        public double WagonMaxBogieFrontDistance { get; set; }

        /// <summary>
        /// Rail Width
        /// </summary>
        /// <remarks>
        /// This is not the distance yout find in descriptions, this is the
        /// width from one rail center to the other rail center on a strait track.
        /// width = inner + (outer - inner) / 2
        /// </remarks>
        [XmlIgnore, JsonIgnore]
        public double RailWidth
        {
            get
            {
                return this.Gauge switch
                {
                    TrackGauge.Gauge_T => 3,                //  3.0 mm :
                    TrackGauge.Gauge_Z => 7.1,              //  6.5 mm :  6.5  7.7 ->  7.1
                    TrackGauge.Gauge_N => 9.8,              //  9.0 mm :  9.0 10.5 ->  9.8
                    TrackGauge.Gauge_TT => 13.3,            // 12.0 mm : 12.0 14.6 -> 13.3
                    TrackGauge.Gauge_H0 => 17.5,            // 16.5 mm : 16.5 18.5 -> 17.5
                    TrackGauge.Gauge_H0e => 9,              //  9.0 mm :
                    TrackGauge.Gauge_00 => 15.5,            // 15.5 mm :
                    TrackGauge.Gauge_S => 22.5,             // 22.5 mm :
                    TrackGauge.Gauge_0 => 32,               // 32.0 mm : 
                    TrackGauge.Gauge_L => 40.3,             // 37.8 mm : 37.8 42.8 -> 40.3 
                    TrackGauge.Gauge_1 => 47.5,             // 45.0 mm : 45.0 50.0 -> 47.5
                    TrackGauge.Gauge_2 => 64,               // 64.0 mm :
                    TrackGauge.Gauge_G => 48.5,             // 45.0 mm : 45.0 52.0 -> 48.4
                    _ => 0
                };
            }
        }
    }
}
