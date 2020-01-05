using Rail.Properties;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Rail.Model
{
    public class TrackStraightAdjustment : TrackStraight
    {
        [XmlAttribute("LengthTo")]
        public double LengthTo { get; set; }

        [XmlIgnore]
        public override string Name
        {
            get
            {
                return $"{Resources.TrackStraightAdjustment} {Length} - {LengthTo} mm";
            }
        }

        [XmlIgnore]
        public override string Description
        {
            get
            {
                return $"{this.Article} {Resources.TrackStraightAdjustment} {Length} - {LengthTo} mm";
            }
        }
    }
}
