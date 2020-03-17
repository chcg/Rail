using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Rail.Mvvm.Converter
{
    public class NumToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null) return DependencyProperty.UnsetValue;

            if (parameter.GetType().IsArray)
            {
                return ((Array)parameter).Cast<int>().Any(p => p == (int)value) ? Visibility.Visible : Visibility.Collapsed;
            }
            return (int)parameter == (int)value ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
