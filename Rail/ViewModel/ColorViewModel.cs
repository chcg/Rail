using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Media;

namespace Rail.ViewModel
{
    public class ColorViewModel
    {
        public static ColorViewModel[] colors;
            
        static ColorViewModel()
        {
            colors = typeof(Colors).GetProperties().OrderBy(p => p.Name).Select(p => new ColorViewModel { Name = p.Name, Color = (Color)p.GetValue(null, null) }).ToArray();

        }

        public Color Color { get; set; }
        public string Name { get; set; }

        public Brush Brush { get { return new SolidColorBrush(this.Color); } }

        public IEnumerable<ColorViewModel> Colors { get { return colors; } }
    }
}
