using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Rail.Misc
{
    [Serializable]
    public class XmlPoint : IXmlSerializable
    {
        private Point point;

        public XmlPoint()
        { }

        public XmlPoint(Point point)
        {
            this.point = point;
        }

        public static implicit operator Point(XmlPoint p)
        {
            return p.point;
        }

        public static implicit operator XmlPoint(Point p)
        {
            return new XmlPoint(p);
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            string value = reader.ReadContentAsString();
            this.point = Point.Parse(value);
        }

        public void WriteXml(XmlWriter writer)
        {
            string value = this.point.ToString();
            writer.WriteString(value);
        }
    }
}
