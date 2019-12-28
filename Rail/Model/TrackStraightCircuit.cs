using Rail.Properties;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Rail.Model
{
    public class TrackStraightCircuit : TrackStraight
    {
        [XmlIgnore]
        public override string Name
        {
            get
            {
                return $"{Resources.TrackStraightCircuit}";
            }
        }
    }
}
