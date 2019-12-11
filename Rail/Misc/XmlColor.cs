using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Misc
{
    public class XmlColor
    {
        private Color color = Colors.Black;

        public XmlColor()
        { }

        public XmlColor(Color c) 
        { 
            color = c; 
        }
        
        public static implicit operator Color(XmlColor x)
        {
            return x.color;
        }

        public static implicit operator XmlColor(Color c)
        {
            return new XmlColor(c);
        }

        [XmlText]
        public string Default
        {
            get { return color.ToString(); }
            set { color = (Color)ColorConverter.ConvertFromString(value); }
        }
    }
}
