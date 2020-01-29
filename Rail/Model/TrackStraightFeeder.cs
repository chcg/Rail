using Rail.Properties;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Rail.Model
{
    public class TrackStraightFeeder : TrackStraight
    {
        [XmlIgnore, JsonIgnore]
        public override string Name
        {
            get
            {
                return $"{Resources.TrackStraightFeeder} {Length} mm";
            }
        }

        [XmlIgnore, JsonIgnore]
        public override string Description
        {
            get
            {
                return $"{this.Article} {Resources.TrackStraightFeeder} {Length} mm";
            }
        }
    }
}
