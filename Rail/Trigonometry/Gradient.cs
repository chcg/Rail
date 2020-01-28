using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.Trigonometry
{
    public static class Gradient
    {
        private static readonly double PIFactor = Math.PI / 180.0;

        public static double AngleToPercent(double value)
        {
            return Math.Tan(value * PIFactor) * 100.0;
        }

        public static double PercentToAngle(double value)
        {
            return Math.Atan(value / 100.0) / PIFactor;
        }
    }
}
