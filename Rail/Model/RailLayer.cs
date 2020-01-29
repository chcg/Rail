using Rail.Misc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Model
{
    public class RailLayer
    {
        public RailLayer()
        {
            this.Show = true;
        }

        [XmlAttribute("Id")]
        public Guid Id { get; set; }

        [XmlElement("Show")]
        public bool Show { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Height")]
        public int Height { get; set; }

        //[XmlElement("TrackColor", typeof(XmlColor))]
        [XmlElement("TrackColor")]
        public Color TrackColor { get; set; }

        //[XmlElement("PlateColor", typeof(XmlColor))]
        [XmlElement("PlateColor")]
        public Color PlateColor { get; set; }

        [XmlIgnore, JsonIgnore]
        public Brush TrackBrush { get { return new SolidColorBrush(TrackColor); } }

        [XmlIgnore, JsonIgnore]
        public Brush PlateBrush { get { return new SolidColorBrush(PlateColor); } }

        public RailLayer Clone()
        {
            return new RailLayer()
            {
                Id = this.Id,
                Show = this.Show,
                Name = this.Name,
                Height = this.Height,
                TrackColor = this.TrackColor,
                PlateColor = this.PlateColor
            };
        }
    }
}
