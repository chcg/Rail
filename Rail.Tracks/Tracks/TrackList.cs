using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Resources;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Rail.Tracks
{
    [XmlRoot("TrackList")]
    public class TrackList 
    {
        public TrackList()
        {
        }

        [XmlElement("TrackType")]
        public List<TrackType> TrackTypes { get; set; }

        public static TrackList Load()
        {
            Assembly trackAssembly = typeof(TrackList).Assembly;

            // read schema
            using Stream stream = trackAssembly.GetManifestResourceStream("Rail.Tracks.Tracks.xsd");
            XmlSchema schema = XmlSchema.Read(stream, Validation);

            XmlReaderSettings settings = new XmlReaderSettings
            {
                CheckCharacters = true,
                ValidationType = ValidationType.Schema
            };
            settings.Schemas.Add(schema);
            

            string file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Rail\\Tracks.xml");
            
            using Stream xmlStream = File.Exists(file) ? File.OpenRead(file) : trackAssembly.GetManifestResourceStream("Rail.Tracks.Tracks.xml");

            // read from file
            XmlSerializer serializer = new XmlSerializer(typeof(TrackList));
            TrackList trackList = (TrackList)serializer.Deserialize(XmlReader.Create(xmlStream, settings));
                        
            trackList.TrackTypes.ForEach(trackType => trackType.Update());
            return trackList;
        }

        
        public void Save()
        {
            string file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Rail\\Tracks.xml");

            if (File.Exists(file))
            {
                int i = 0;
                while (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), $"Rail\\Tracks{++i}.xml")));
                File.Move(file, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), $"Rail\\Tracks{i}.xml"));    
            }

            // set current file version
            using XmlTextWriter writer = new XmlTextWriter(file, Encoding.UTF8)
            {
                Formatting = Formatting.Indented,
                Indentation = 2
            };
            XmlSerializer serializer = new XmlSerializer(this.GetType());
            serializer.Serialize(writer, this);

            //JsonWriterOptions options = default;
            //options.Indented = true;
            //using (Utf8JsonWriter writer = new Utf8JsonWriter(File.Create(Path.ChangeExtension(path, "jrail")), options))
            //{
            //    JsonSerializer.Serialize(writer, this, this.GetType());
            //}
        }

        private static void Validation(object sender, ValidationEventArgs e)
        {
            MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
