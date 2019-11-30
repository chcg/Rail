using Rail.Controls;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Model
{

    public class TrackStraight : TrackBase
    {

        [XmlAttribute("Length")]
        public double Length { get; set; }

        public override void Update(double spacing)
        {
            base.Update(spacing);

            this.geometry = CreateStraitTrackGeometry(this.Length);
        }
    }
}
