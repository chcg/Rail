using Rail.Model;
using Rail.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Rail.ViewModel
{
    public class LayerViewModel : BaseViewModel
    {
        private Guid Id;
        private string name;
        private int height;
        private ColorViewModel trackColor;
        private bool show;
        private ColorViewModel plateColor;

        public LayerViewModel()
        {
            this.Id = Guid.NewGuid();
            this.Name = "New Layer";
            this.Show = true;
            this.Height = 100;
            this.TrackColor = ColorViewModel.colors.FirstOrDefault(c => c.Color == System.Windows.Media.Colors.White);
            this.PlateColor = ColorViewModel.colors.FirstOrDefault(c => c.Color == System.Windows.Media.Colors.Green);
        }
        public LayerViewModel(RailLayer layer)
        {
            this.Id = layer.Id;
            this.Name = layer.Name;
            this.Show = layer.Show;
            this.Height = layer.Height;
            this.TrackColor = ColorViewModel.colors.FirstOrDefault(c => c.Color == layer.TrackColor);
            this.PlateColor = ColorViewModel.colors.FirstOrDefault(c => c.Color == layer.PlateColor);
        }

        public static implicit operator RailLayer(LayerViewModel vm)
        {
            return new RailLayer()
            {
                Id = vm.Id,
                Name = vm.Name,
                Show = vm.Show,
                Height = vm.Height,
                TrackColor = vm.TrackColor.Color,
                PlateColor = vm.PlateColor.Color
            };
        }

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

        public bool Show
        {
            get
            {
                return this.show;
            }
            set
            {
                this.show = value;
                NotifyPropertyChanged(nameof(Show));
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

        public ColorViewModel TrackColor
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

        public ColorViewModel PlateColor
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

        public IEnumerable<ColorViewModel> Colors { get { return ColorViewModel.colors; } }

    }
}
