using Rail.Mvvm;
using Rail.Tracks.Misc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Rail.TrackEditor.ViewModel
{
    //public class TrackTypeNameViewModel : BaseViewModel
    //{
    //    private string language;
    //    private string name;

    //    public TrackTypeNameViewModel()
    //    { }

    //    public TrackTypeNameViewModel(KeyValuePair<string, string> keyValuePair)
    //    {
    //        this.Language = keyValuePair.Key;
    //        this.Name = keyValuePair.Value;
    //    }

    //    public string Language
    //    {
    //        get { return this.language; }
    //        set { this.language = value.Trim(); NotifyPropertyChanged(nameof(Language)); }
    //    }

    //    public string Name
    //    {
    //        get { return this.name; }
    //        set { this.name = value.Trim(); NotifyPropertyChanged(nameof(Name)); }
    //    }
    //}

    public class MultilanguageStringViewModel : BaseViewModel
    {
        private const string defaultKey = "default";
        private const string defaultLanguage = "en-US";

        private XmlMultilanguageString multilanguageString;

        public MultilanguageStringViewModel(XmlMultilanguageString multilanguageString)
        {
            this.multilanguageString = multilanguageString;
            this.Items = new ObservableCollection<MultilanguageItemViewModel>(multilanguageString.LanguageDictionary.Select(n => new MultilanguageItemViewModel(n)));
            this.Items.ToList().ForEach(i => i.PropertyChanged += OnItemsPropertyChanged);
            this.Items.CollectionChanged += OnCollectionChanged;
            this.Value = this.Items.FirstOrDefault()?.Name;
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
            case NotifyCollectionChangedAction.Add:
                e.NewItems.Cast<MultilanguageItemViewModel>().ToList().ForEach(i => i.PropertyChanged += OnItemsPropertyChanged);
                break;
            case NotifyCollectionChangedAction.Remove:
                e.OldItems.Cast<MultilanguageItemViewModel>().ToList().ForEach(i => i.PropertyChanged -= OnItemsPropertyChanged);
                break;
            case NotifyCollectionChangedAction.Replace:
                e.NewItems.Cast<MultilanguageItemViewModel>().ToList().ForEach(i => i.PropertyChanged += OnItemsPropertyChanged);
                e.OldItems.Cast<MultilanguageItemViewModel>().ToList().ForEach(i => i.PropertyChanged -= OnItemsPropertyChanged);
                break;
            }
            SetValue();
        }

        private void OnItemsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SetValue();
        }

        public string Value { get; private set; }

        public ObservableCollection<MultilanguageItemViewModel> Items { get; }

        public string[] Languages { get { return CultureInfo.GetCultures(CultureTypes.AllCultures).Select(c => c.Name).ToArray(); } }

        private void SetValue()
        {
            string lang = CultureInfo.CurrentUICulture?.Name ?? defaultKey;
            MultilanguageItemViewModel item;
            if ((item = this.Items.FirstOrDefault(i => i.Language == lang)) != null)
            {
                this.Value = item.Name;
            }
            else if ((item = this.Items.FirstOrDefault(i => i.Language == defaultKey)) != null)
            {
                this.Value = item.Name; 
            }
            else if ((item = this.Items.FirstOrDefault(i => i.Language == defaultLanguage)) != null)
            {
                this.Value = item.Name;
            }
            else
            {
                this.Value = String.Empty;
            }
            NotifyPropertyChanged(nameof(Value));
        }

        [DebuggerDisplay("{Language} : {Name}")]
        public class MultilanguageItemViewModel : BaseViewModel
        {
            private string language;
            private string name;

            public MultilanguageItemViewModel()
            { }

            public MultilanguageItemViewModel(KeyValuePair<string, string> keyValuePair)
            {
                this.Language = keyValuePair.Key;
                this.Name = keyValuePair.Value;
            }

            public string Language
            {
                get { return this.language; }
                set { this.language = value.Trim(); NotifyPropertyChanged(nameof(Language)); }
            }

            public string Name
            {
                get { return this.name; }
                set { this.name = value.Trim(); NotifyPropertyChanged(nameof(Name)); }
            }
        }

    }
}
