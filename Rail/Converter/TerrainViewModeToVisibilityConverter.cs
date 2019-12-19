using Rail.Controls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Rail.Converter
{
    [ValueConversion(typeof(RailViewMode), typeof(Visibility))]
    public class TerrainViewModeToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            RailViewMode mode = (RailViewMode)value;
            return mode == RailViewMode.Terrain ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
