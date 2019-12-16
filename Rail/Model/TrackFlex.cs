using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Rail.Model
{
    public class TrackFlex : TrackBase
    {
        [XmlAttribute("MaxLength")]
        public double MaxLength { get; set; }

        protected override void Create()
        {
            this.Geometry = CreateCurvedTrackGeometry(20, 360);

        }
    }
}
