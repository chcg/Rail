using System.IO;
using System.Text;
using System.Text.Json;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Rail.Tracks
{
    public class BaseProject
    {
        /// <summary>
        /// Load the file 
        /// </summary>
        /// <typeparam name="T">Type of the project class</typeparam>
        /// <param name="path">Path to load</param>
        /// <returns>Instance of the loaded project class</returns>
        public static T Load<T>(string path) where T : BaseProject
        {
            BaseProject project = null;
            using (XmlTextReader reader = new XmlTextReader(path))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                project = (BaseProject)serializer.Deserialize(reader);
            }
            return (T)project;
        }

        public static T Load<T>(string path, XmlSchema schema) where T : BaseProject
        {
            BaseProject project = null;
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.CheckCharacters = true;
            settings.Schemas.Add(schema);
            settings.ValidationType = ValidationType.Schema;
            using (XmlReader reader = XmlReader.Create(path, settings)) 
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                project = (BaseProject)serializer.Deserialize(reader);
            }
            return (T)project;
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
    }
}
