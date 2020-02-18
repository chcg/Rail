using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Rail.Model
{
    public enum TrackGauge
    {
        [XmlEnum("T")]
        Gauge_T,
        [XmlEnum("Z")]
        Gauge_Z,
        [XmlEnum("N")]
        Gauge_N,
        [XmlEnum("TT")]
        Gauge_TT,
        [XmlEnum("H0")]
        Gauge_H0,
        [XmlEnum("H0e")]
        Gauge_H0e,
        [XmlEnum("00")]
        Gauge_00,
        [XmlEnum("S")]
        Gauge_S,
        [XmlEnum("0")]
        Gauge_0,
        [XmlEnum("1")]
        Gauge_1,
        [XmlEnum("2")]
        Gauge_2,
        [XmlEnum("G")]
        Gauge_G,
        [XmlEnum("Lego")]
        Gauge_Lego,
    }
}
