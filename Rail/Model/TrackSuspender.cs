using Rail.Properties;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Rail.Model
{
    public class TrackSuspender : TrackCrossing
    {
        [XmlIgnore]
        public override string Name
        {
            get
            {
                return $"{Resources.TrackSuspender} ";
            }
        }

        [XmlIgnore]
        public override string Description
        {
            get
            {
                return $"{this.Article} {Resources.TrackSuspender}";
            }
        }

    }
}
