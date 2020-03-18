using Rail.Tracks.Properties;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Rail.Tracks.Converter
{
    public sealed class EnumToLocalizedStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }
            string name = $"Enum{value}";
            return Resources.ResourceManager.GetString(name);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
