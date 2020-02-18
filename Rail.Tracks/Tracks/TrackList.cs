using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Resources;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Rail.Model
{
    [XmlRoot("TrackList")]
    public class TrackList : BaseProject
    {
        public TrackList()
        {
        }

        [XmlElement("TrackType")]
        public List<TrackType> TrackTypes { get; set; }

        public static TrackList Load(string path)
        {
            StreamResourceInfo info = Application.GetResourceStream(new Uri("Tracks.xsd", UriKind.Relative));
            XmlSchema schema = XmlSchema.Read(info.Stream, Validation);
            TrackList trackList = BaseProject.Load<TrackList>(path, schema);
            trackList.TrackTypes.ForEach(trackType => trackType.Update());
            return trackList;
        }

        private static void Validation(object sender, ValidationEventArgs e)
        {
            MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
