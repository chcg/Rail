using Rail.Properties;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Rail.Model
{
    public class TrackCurvedCircuit : TrackCurved
    {
        [XmlIgnore]
        public override string Name
        {
            get
            {
                return $"{Resources.TrackCurvedCircuit}";
            }
        }

        [XmlIgnore]
        public override string Description
        {
            get
            {
                
                return $"{this.Article} {Resources.TrackCurvedCircuit}";
            }
        }
    }
}
