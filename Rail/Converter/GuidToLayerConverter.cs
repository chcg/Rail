using Rail.Model;
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Rail.Converter
{
    [ValueConversion(typeof(Guid), typeof(RailLayer))]
    public class GuidToLayerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Guid guid = (Guid)value;
            RailPlan railPlan = (RailPlan)parameter;
            RailLayer railLayer = railPlan.Layers.FirstOrDefault(l => l.Id == guid);
            return railLayer;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            RailLayer railLayer = (RailLayer)value;
            return railLayer.Id;
        }
    }
}
