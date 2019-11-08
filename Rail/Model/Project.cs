using Rail.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Rail.Model
{
    [XmlRoot("Rail")]
    public class Project : BaseVersionedProject
    {
        public Project()
        {
            this.TrackDiagram = new TrackDiagram();
        }

        public static Project CreateProject()
        {
            return new Project();
        }

        public static Project Load(string path)
        {
            return BaseVersionedProject.Load<Project>(path);
        }

        [XmlElement("TrackDiagram")]
        public TrackDiagram TrackDiagram { get; set; }
    }
}
