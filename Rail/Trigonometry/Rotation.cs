using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Rail.Trigonometry
{
    /// <summary>
    /// Normalized turn angel between -360° and 360°
    /// </summary>
    public class Rotation
    {
        private const int MAX = 3600;
        private const double FAC = 10.0;
        private int angle = 0;

        public Rotation()
        { }

        public Rotation(double value)
        {
            int val = (int)Math.Round(value * FAC);
            this.angle = (short)((val % MAX + MAX) % MAX);
        }

        private Rotation(short value)
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
