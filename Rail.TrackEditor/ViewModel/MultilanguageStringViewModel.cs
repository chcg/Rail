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
    public class MultilanguageStringViewModel : BaseViewModel
    {
        private const string defaultKey = "default";
        private readonly CultureInfo defaultLanguage = new CultureInfo("en-US");

        private XmlMultilanguageString multilanguageString;

        public MultilanguageStringViewModel(XmlMultilanguageString multilanguageString)
        {
            this.multilanguageString = multilanguageString;
            this.Items = new ObservableCollection<MultilanguageItemViewModel>(multilanguageString.LanguageDictionary.Select(n => new MultilanguageItemViewModel(n)));
            this.Items.ToList().ForEach(i => i.PropertyChanged += OnItemsPropertyChanged);
            this.Items.CollectionChanged += OnCollectionChanged;
            this.Value = this.Items.FirstOrDefault()?.Name;

            //var x = CultureInfo.GetCultures(CultureTypes.NeutralCultures);
            //var k = x.First(c => c.DisplayName == "Kölsch");
            //var d = x.First(c => c.DisplayName == "Deutsch");
            //var i = CultureInfo.InvariantCulture;
            //var y = x.Where(c => c.IsNeutralCulture).ToArray();
            //var z = y.Where(c => !c.CultureTypes.HasFlag(CultureTypes.UserCustomCulture)).ToArray();
            //var zz = z.Where(c => c.CultureTypes.HasFlag(CultureTypes.InstalledWin32Cultures)).ToArray();
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

        public CultureInfo[] Languages { get { return CultureInfo.GetCultures(CultureTypes.NeutralCultures).Where(c => c.IsNeutralCulture).OrderBy(c => c.DisplayName).ToArray(); } }

        private void SetValue()
        {
            this.multilanguageString.LanguageDictionary = this.Items.Where(i => i.Language != null && i.Name != null).ToDictionary(i => i.Language.Name, i=> i.Name);

            string lang = CultureInfo.CurrentUICulture?.Name ?? defaultKey;
            MultilanguageItemViewModel item =
                this.Items.FirstOrDefault(i => i.Language == CultureInfo.CurrentUICulture && !string.IsNullOrEmpty(i.Name)) ??
                //this.Items.FirstOrDefault(i => i.Language == defaultKey && !string.IsNullOrEmpty(i.Name)) ??
                this.Items.FirstOrDefault(i => i.Language == defaultLanguage && !string.IsNullOrEmpty(i.Name));
            this.Value = item?.Name ?? String.Empty;

            NotifyPropertyChanged(nameof(Value));
        }

        [DebuggerDisplay("{Language} : {Name}")]
        public class MultilanguageItemViewModel : BaseViewModel
        {
            private CultureInfo language;
            private string name;

            public MultilanguageItemViewModel()
            { }

            public MultilanguageItemViewModel(KeyValuePair<string, string> keyValuePair)
            {
                this.Language = keyValuePair.Key == defaultKey ? new CultureInfo("en") : new CultureInfo(keyValuePair.Key);
                this.Name = keyValuePair.Value;
            }

            public CultureInfo Language
            {
                get { return this.language; }
                set { this.language = value; NotifyPropertyChanged(nameof(Language)); }
            }

            public string Name
            {
                get { return this.name; }
                set { this.name = value.Trim(); NotifyPropertyChanged(nameof(Name)); }
            }
        }

    }
}
