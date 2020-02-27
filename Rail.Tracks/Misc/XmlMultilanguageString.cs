using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Rail.Tracks.Misc
{
    [Serializable]
    public class XmlMultilanguageString : IXmlSerializable
    {
        private const string defaultKey = "default";
        private const string defaultLanguage = "en-US";
        private const string elementName = "LanguageString";
        private const string attributetName = "Lang";
        private Dictionary<string, string> languageDictionary;

        public XmlMultilanguageString()
        {
            this.languageDictionary = new Dictionary<string, string>();
        }

        public XmlMultilanguageString(string val)
        {
            this.languageDictionary = new Dictionary<string, string> { { defaultKey, val } };
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            this.languageDictionary = new Dictionary<string, string>();

            while (reader.NodeType != XmlNodeType.Element)
            {
                reader.Read();
            }
            string arrayName = reader.Name;
            reader.Read();

            while (!(reader.NodeType == XmlNodeType.EndElement && reader.Name == arrayName))
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == elementName)
                {
                    string lang = reader.GetAttribute(attributetName) ?? defaultKey;
                    string value = reader.ReadElementContentAsString();
                    this.languageDictionary.Add(lang, value);
                }
                reader.Read();
            }
            reader.Read();
        }

        public void WriteXml(XmlWriter writer)
        {
            //writer.WriteStartElement("Name");
            foreach (var entry in this.languageDictionary)
            {
                writer.WriteStartElement(elementName);
                if (!string.IsNullOrEmpty(entry.Key) && entry.Key != defaultKey)
                {
                    writer.WriteAttributeString(attributetName, entry.Key);
                }
                writer.WriteString(entry.Value);
                writer.WriteEndElement();
            }
            //writer.WriteEndElement();
        }

        public string Value
        {
            get
            {
                string lang = CultureInfo.CurrentUICulture?.Name ?? defaultLanguage;
                if (languageDictionary.TryGetValue(lang, out string value))
                {
                    return value;
                }
                if (languageDictionary.TryGetValue(defaultKey, out value))
                {
                    return value;
                }
                if (languageDictionary.TryGetValue(defaultLanguage, out value))
                {
                    return value;
                }
                return String.Empty;        
            }
        }

        public static implicit operator string(XmlMultilanguageString str)
        {
            return str.Value;
        }

        public override string ToString()
        {
            return this.Value;
        }

        public Dictionary<string, string> LanguageDictionary 
        {
            get { return this.languageDictionary; }
            set { this.languageDictionary = value; }
        }
    }
}
