using Rail.Properties;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Rail.Model
{
    public class TrackStraightIsolating : TrackStraight
    {
        [XmlIgnore, JsonIgnore]
        public override string Name
        {
            get
            {
                return $"{Resources.TrackStraightIsolating} {Length} mm";
            }
        }

        [XmlIgnore, JsonIgnore]
        public override string Description
        {
            get
            {
                return $"{this.Article} {Resources.TrackStraightIsolating} {Length} mm";
            }
        }
    }
}
