using Rail.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace Rail.ViewModel
{
    public class LayerViewModel : BaseViewModel
    {
        private string name;
        private int height;
        private Color trackColor;
        private Color plateColor;

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
                NotifyPropertyChanged(nameof(Name));
            }
        }

        public int Height
        {
            get
            {
                return this.height;
            }
            set
            {
                this.height = value;
                NotifyPropertyChanged(nameof(Height));
            }
        }

        public Color TrackColor
        {
            get
            {
                return this.trackColor;
            }
            set
            {
                this.trackColor = value;
                NotifyPropertyChanged(nameof(TrackColor));
            }
        }

        public Color PlateColor
        {
            get
            {
                return this.plateColor;
            }
            set
            {
                this.plateColor = value;
                NotifyPropertyChanged(nameof(PlateColor));
            }
        }
    }
}
