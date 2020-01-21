using Rail.Mvvm;
using Rail.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Windows;

namespace Rail.ViewModel
{
    public class OptionsViewModel : DialogViewModel
    {
        private readonly Settings settings;
        public OptionsViewModel()
        {
            this.settings = Settings.Default;
            this.settings.Upgrade();

            ResourceManager resourceManager = new ResourceManager("Rail.Properties.Resources", typeof(Resources).Assembly);

            this.Languages = CultureInfo.GetCultures(CultureTypes.AllCultures).
                Where(c => resourceManager.GetResourceSet(c, true, false) != null).
                Select(c => new Language { Name = string.IsNullOrEmpty(c.Name) ? "Windows" : c.NativeName, Id = c.Name }).ToList();
            this.SelectedLanguage = this.Languages.FirstOrDefault(l => l.Id == this.settings.Language) ?? this.Languages.FirstOrDefault();
        }

        protected override void OnOK()
        {
            this.settings.Language = this.SelectedLanguage?.Id;
            this.settings.Save();

            base.OnOK();
        }

        public class Language
        {
            public string Name { get; set; }
            public string Id { get; set; }
        }

        public List<Language> Languages { get; private set; }

        private Language selectedLanguage = null;

        public Language SelectedLanguage
        {
            get
            {
                return this.selectedLanguage;
            }
            set
            {
                this.selectedLanguage = value;
                NotifyPropertyChanged(nameof(SelectedLanguage));
            }
        }
    }
}
