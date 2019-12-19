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
    public class TrackRailViewModeToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            RailViewMode mode = (RailViewMode)value;
            return mode == RailViewMode.Terrain ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
