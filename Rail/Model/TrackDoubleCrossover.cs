using Rail.Properties;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Rail.Model
{
    public class TrackDoubleCrossover : TrackCrossing
    {
        [XmlIgnore]
        public override string Name
        {
            get
            {
                return $"{Resources.TrackDoubleCrossover} ";
            }
        }

        [XmlIgnore]
        public override string Description
        {
            get
            {
                return $"{this.Article} {Resources.TrackDoubleCrossover}";
            }
        }

    }
}
