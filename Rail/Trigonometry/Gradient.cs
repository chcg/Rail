using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.Trigonometry
{
    public static class Gradient
    {
        private static readonly double PIFactor = Math.PI / 180.0;

        public static double AngleToPercent(double angle)
        {
            return Math.Tan(angle * PIFactor) * 100.0;
        }

        public static double PercentToAngle(double perc)
        {
            return Math.Atan(perc / 100.0) / PIFactor;
        }

        public static double CalcHeight(double angle, double radius)
        {
            return Math.Tan(angle * PIFactor) * radius;
        }
    }
}
