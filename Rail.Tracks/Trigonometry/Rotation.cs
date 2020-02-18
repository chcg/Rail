using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;

namespace Rail.Tracks.Trigonometry
{
    /// <summary>
    /// Normalized turn angel between -360° and 360°
    /// </summary>
    [DebuggerDisplay("Rotation {Value}")]
    public class Rotation
    {
        private const int MAX = 3600;
        private const double FAC = 10.0;
        private int angle = 0;

        public Rotation()
        { }

        // Winkel in Prozent = tan(Winkel in Grad) * 100%
        // Winkel in Grad = atan(Winkel in Prozent / 100%)

        public Rotation(double value)
        {
            int val = (int)Math.Round(value * FAC);
            this.angle = Normalize(val);
        }

        public Rotation(int value)
        {
            this.angle = Normalize(value);
        }

        private static int Normalize(int value)
        {
            return (int)(value % MAX);
        }
        
        public double Value
        {
            get
            {
                return this.angle / FAC;
            }
            set
            {
                int val = (int)Math.Round(value * FAC);
                this.angle = (short)((val % MAX + MAX) % MAX);
            }
        }

        public static implicit operator Rotation(double value)
        {
            return new Rotation(value);
        }

        public static implicit operator double(Rotation a)
        {
            return a.angle / FAC;
        }

        public static Rotation operator +(Rotation a, Rotation b)
        {
            return new Rotation((int)(a.angle + b.angle));
        }

        public static Rotation operator +(Rotation a, Angle b)
        {
            return new Rotation((int)(a.angle + b.IntAngle));
        }

        

        public static Rotation operator -(Rotation a, Rotation b)
        {
            return new Rotation((int)(a.angle - b.angle));
        }

        public static Rotation operator -(Rotation a, Angle b)
        {
            return new Rotation((int)(a.angle - b.IntAngle));
        }

        

        /// <summary>
        /// for internal use only
        /// </summary>
        internal int IntAngle {  get { return angle; } }

        public static Rotation Calculate(Point center, Point from, Point to)
        {
            Vector fromVec = from - center;
            Vector toVec = to - center;
            double ang = Vector.AngleBetween(fromVec, toVec);
            return new Rotation(ang);
        }
    }
}
