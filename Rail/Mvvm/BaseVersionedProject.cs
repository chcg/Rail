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
    public class BaseVersionedProject
    {
        //private readonly Version projectVersion;

        //public BaseVersionedProject()
        //{
        //    //AssemblyProjectVersionAttribute projectVersionAttribute = Assembly.GetCallingAssembly().GetCustomAttribute<AssemblyProjectVersionAttribute>();
        //    //if (projectVersionAttribute == null)
        //    //{
        //    //    throw new Exception("AssemblyProjectVersionAttribute not set!");
        //    //}
        //    //this.projectVersion = new Version(projectVersionAttribute.Version);
        //    this.Version = this.projectVersion.ToString(2);
        //}

        ///// <summary>
        ///// Version of the file
        ///// </summary>
        //[XmlAttribute("Version")]
        //public string Version { get; set; }
        
        /// <summary>
        /// Load the file with version check
        /// </summary>
        /// <typeparam name="T">Type of the project class</typeparam>
        /// <param name="path">Path to load</param>
        /// <returns>Instance of the loaded project class</returns>
        public static T Load<T>(string path) where T : BaseVersionedProject
        {
            BaseVersionedProject project = null;
            using (XmlTextReader reader = new XmlTextReader(path))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                project = (BaseVersionedProject)serializer.Deserialize(reader);
            }
            //if (project != null)
            //{
            //    //Version ver = new Version(project.Version);
            //    //if (ver > project.projectVersion)
            //    //{
            //    //    MessageBox.Show(string.Format("{0}\r\nis from a newer app version.\r\nPlease update this app to a newer version.", path), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //    //    throw new Exception(string.Format("{0} is from a newer version than this app. Please update this app to a newer version.", path));
            //    //}
            //}
            return (T)project;
        }

        /// <summary>
        /// Save project to file
        /// </summary>
        /// <param name="path">Path to save to.</param>
        public void Save(string path)
        {
            // set current file version
            //this.Version = this.projectVersion.ToString(2);
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
