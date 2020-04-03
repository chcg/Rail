using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Rail.Tracks
{
    public enum TrackGauge
    {
        [XmlEnum("T"), Description("T")]
        Gauge_T,
        [XmlEnum("Z"), Description("Z")]
        Gauge_Z,
        [XmlEnum("N"), Description("N")]
        Gauge_N,
        [XmlEnum("TT"), Description("TT")]
        Gauge_TT,
        [XmlEnum("H0"), Description("H0")]
        Gauge_H0,
        [XmlEnum("H0e"), Description("H0e")]
        Gauge_H0e,
        [XmlEnum("00"), Description("00")]
        Gauge_00,
        [XmlEnum("S"), Description("S")]
        Gauge_S,
        [XmlEnum("0"), Description("0")]
        Gauge_0,
        [XmlEnum("L"), Description("L")]
        Gauge_L,
        [XmlEnum("1"), Description("1")]
        Gauge_1,
        [XmlEnum("2"), Description("2")]
        Gauge_2,
        [XmlEnum("G"), Description("G / IIm")]
        Gauge_G,
    }
}
