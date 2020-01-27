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
        public string TrackColorInt 
        { 
            get { return TrackColor.ToString(); }
            set { this.TrackColor = (Color)ColorConverter.ConvertFromString(value); }
        }

        [XmlIgnore]
        public Color TrackColor { get; set; }

        [XmlIgnore]
        public Brush TrackBrush { get { return new SolidColorBrush(TrackColor); } }

        [XmlAttribute("PlateColor")]
        public string PlateColorInt
        {
            get { return PlateColor.ToString(); }
            set { this.PlateColor = (Color)ColorConverter.ConvertFromString(value); }
        }
        
        [XmlIgnore]
        public Color PlateColor { get; set; }

        [XmlIgnore]
        public Brush PlateBrush { get { return new SolidColorBrush(PlateColor); } }



    }
}
