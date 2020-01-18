using Rail.Controls;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Rail.Converter
{
    [ValueConversion(typeof(RailSelectedMode), typeof(bool))]
    public class SelectedModeNotNoneToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            RailSelectedMode railSelectedMode = (RailSelectedMode)value;
            return railSelectedMode != RailSelectedMode.None;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
