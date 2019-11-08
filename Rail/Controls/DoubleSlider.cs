using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Rail.Controls
{
    public class DoubleSlider : NumericSlider<double>
    {
        static DoubleSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DoubleSlider), new FrameworkPropertyMetadata(typeof(DoubleSlider)));
        }

        public DoubleSlider()
        {
            
        }

        protected override string ValueToString()
        {
            return this.Value.ToString(this.FormatString, this.CultureInfo);
        }

        protected override void IncrementValue()
        {
            this.Value += this.Increment;
        }

        protected override void DecrementValue()
        {
            this.Value -= this.Increment;
        }

        protected override object OnCoerceValue(object newValue)
        {
            double value = (double) newValue;
            value = Math.Max(value, this.Minimum);
            value = Math.Min(value, this.Maximum);
            value = Math.Round(value / this.Increment) * this.Increment;
            return value;
        }
    }
}
