using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;

namespace Rail.Mvvm
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
        }
    }
}
