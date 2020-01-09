using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Xml.Serialization;

namespace Rail.Trigonometry
{
    /// <summary>
    /// Normalized angel between 0° and 360°
    /// </summary>
    public class Angle
    {
        private const int MAX = 3600;
        private const int REV = 1800;
        private const double FAC = 10.0;
        private int angle = 0;

        public Angle()
        { }

        public Angle(double value)
        {
            int val = (int)Math.Round(value * FAC);
            this.angle = Normalize(val);
        }

        private Angle(int value)
        {
            this.angle = (int)((value % MAX + MAX) % MAX);
        }

        private static short Normalize(int value)
        {
            return (short)((value % MAX + MAX) % MAX);
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
                this.angle = Normalize(val);
            }
        }

        public static implicit operator Angle(double value)
        {
            return new Angle(value);
        }

        public static implicit operator double(Angle a)
        {
            return a.angle / FAC;
        }        

        public static Angle operator +(Angle a, Angle b)
        {
            return new Angle((int)(a.angle + b.angle));
        }

        public static Angle operator +(Angle a, Rotation b)
        {
            return new Angle((int)(a.angle + b.IntAngle));
        }

        public static Angle operator -(Angle a, Angle b)
        {
            return new Angle((int)(a.angle - b.angle));
        }

        public static Angle operator -(Angle a, Rotation b)
        {
            return new Angle((int)(a.angle - b.IntAngle));
        }

        public static bool operator ==(Angle a, Angle b)
        {
            return a.angle == b.angle;
        }

        public static bool operator !=(Angle a, Angle b)
        {
            return a.angle != b.angle;
        }

        public override bool Equals(object obj)
        {
            return this.angle == ((Angle)obj).angle;
        }

        public override int GetHashCode()
        {
            return this.angle;
        }

        public override string ToString()
        {
            return this.Value.ToString("F1", CultureInfo.InvariantCulture);
        }

        public Angle Revert()
        {
            return new Angle(this.angle + REV);
        }

        public static Angle Calculate(Point center, Point p1, Point p2)
        {
            return (Angle)Vector.AngleBetween(p1 - center, p2 - center);
        }

        public static Angle Calculate(Point center, Point pos)
        {
            return (Angle)Vector.AngleBetween(new Vector(100, 0), pos - center);
        }

        public Vector Circle(double radius)
        {
            double val = this.Value * (Math.PI / 180.0);
            double sin = Math.Sin(val);
            double cos = Math.Cos(val);

            return new Vector(sin * radius, cos * radius);
        }

        
    }

    
}
