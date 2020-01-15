using System;
using System.Collections.Generic;
using System.Text;
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

        [XmlAttribute("Show")]
        public bool Show { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("Height")]
        public int Height { get; set; }

        [XmlAttribute("TrackColor")]
        public Color TrackColor { get; set; }

        [XmlAttribute("PlateColor")]
        public Color PlateColor { get; set; }

        [XmlIgnore]
        public Brush TrackBrush {  get { return new SolidColorBrush(TrackColor); } }

    }
}
