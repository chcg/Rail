using Rail.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.TrackEditor.ViewModel
{
    public class TrackTypeNameViewModel : BaseViewModel
    {
        private string language;
        private string name;
       
        public TrackTypeNameViewModel()
        { }

        public TrackTypeNameViewModel(KeyValuePair<string, string> keyValuePair)
        {
            this.Language = keyValuePair.Key;
            this.Name = keyValuePair.Value;
        }

        public string Language
        {
            get { return this.language; }
            set { this.language = value; NotifyPropertyChanged(nameof(Language)); }
        }

        public string Name
        {
            get { return this.name; }
            set { this.name = value; NotifyPropertyChanged(nameof(Name)); }
        }
    }
}
