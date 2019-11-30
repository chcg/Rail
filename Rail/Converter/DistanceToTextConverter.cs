using System;
using System.Globalization;
using System.Windows.Data;

namespace Rail.Converter
{
    [ValueConversion(typeof(double), typeof(int))]
    public class DistanceToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double val = (double)value;
            return val == 0 ? "No" : $"{val} mm";

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
