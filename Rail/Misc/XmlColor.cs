using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Rail.Misc
{
    [Serializable]
    public class XmlColor : IXmlSerializable
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

        //[XmlText]
        //public string Default
        //{
        //    get { return color.ToString(); }
        //    set { color = (Color)ColorConverter.ConvertFromString(value); }
        //}

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            string value = reader.ReadContentAsString();
            this.color = (Color)ColorConverter.ConvertFromString(value); 
        }

        public void WriteXml(XmlWriter writer)
        {
            string value = color.ToString(); 
            writer.WriteString(value);
        }
    }
}
