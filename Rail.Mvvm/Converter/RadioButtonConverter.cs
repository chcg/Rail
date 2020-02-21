using System;
using System.Globalization;
using System.Windows.Data;

namespace Rail.Mvvm.Converter
{
    [ValueConversion(typeof(object), typeof(bool))]
    public class RadioButtonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return parameter.Equals(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return parameter;
        }
    }
}
