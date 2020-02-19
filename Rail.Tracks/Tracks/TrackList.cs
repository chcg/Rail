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

        public static TrackList Load(string path)
        {
            using Stream stream = typeof(TrackList).Assembly.GetManifestResourceStream("Rail.Tracks.Tracks.xsd");
            XmlSchema schema = XmlSchema.Read(stream, Validation);
            TrackList trackList = TrackList.Load(path, schema);
            trackList.TrackTypes.ForEach(trackType => trackType.Update());
            return trackList;
        }

        ///// <summary>
        ///// Load the file 
        ///// </summary>
        ///// <typeparam name="T">Type of the project class</typeparam>
        ///// <param name="path">Path to load</param>
        ///// <returns>Instance of the loaded project class</returns>
        //public static TrackList Load(string path) 
        //{
        //    TrackList project = null;
        //    using (XmlTextReader reader = new XmlTextReader(path))
        //    {
        //        XmlSerializer serializer = new XmlSerializer(typeof(TrackList));
        //        project = (TrackList)serializer.Deserialize(reader);
        //    }
        //    return project;
        //}

        public static TrackList Load(string path, XmlSchema schema) 
        {
            TrackList project = null;
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.CheckCharacters = true;
            settings.Schemas.Add(schema);
            settings.ValidationType = ValidationType.Schema;
            using (XmlReader reader = XmlReader.Create(path, settings))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(TrackList));
                project = (TrackList)serializer.Deserialize(reader);
            }
            return project;
        }

        /// <summary>
        /// Save project to file
        /// </summary>
        /// <param name="path">Path to save to.</param>
        public void Save(string path)
        {
            // set current file version
            using (XmlTextWriter writer = new XmlTextWriter(path, Encoding.UTF8))
            {
                writer.Formatting = Formatting.Indented;
                writer.Indentation = 2;
                XmlSerializer serializer = new XmlSerializer(this.GetType());
                serializer.Serialize(writer, this);
            }

            JsonWriterOptions options = default;
            options.Indented = true;
            using (Utf8JsonWriter writer = new Utf8JsonWriter(File.Create(Path.ChangeExtension(path, "jrail")), options))
            {
                JsonSerializer.Serialize(writer, this, this.GetType());
            }
        }

        private static void Validation(object sender, ValidationEventArgs e)
        {
            MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
