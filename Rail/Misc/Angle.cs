using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Xml.Serialization;

namespace Rail.Misc
{
    public class Angle
    {
        private const ushort MAX = 3600;
        private const ushort REV = 1800;
        private ushort angle = 0;

        public Angle()
        { }

        public Angle(double value)
        {
            int val = (int)Math.Round(value * 10.0);
            this.angle = (ushort)((val % MAX + MAX) % MAX);
        }

        public Angle(ushort value)
        {
            this.angle = (ushort)(value % MAX);
        }

        [XmlText]
        public double Value
        {
            get 
            { 
                return this.angle / 10; 
            }
            set
            {
                int val = (int)Math.Round(value * 10.0);
                this.angle = (ushort)((val % MAX + MAX) % MAX);
            }
        }

        public static implicit operator Angle(double value)
        {
            return new Angle(value);
        }

        public static implicit operator double(Angle a)
        {
            return a.angle / 10.0;
        }        

        public static Angle operator +(Angle a, Angle b)
        {
            return new Angle((ushort)(a.angle + b.angle));
        }
        
        public static Angle operator -(Angle a, Angle b)
        {
            return new Angle((ushort)(a.angle - b.angle));
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
            return (this.angle / 10).ToString("F1", CultureInfo.InvariantCulture);
        }

        public Angle Revert()
        {
            return new Angle((ushort)(this.angle + REV));
        }

        public static Angle Calculate(Point center, Point p1, Point p2)
        {
            return (Angle)Vector.AngleBetween(p1 - center, p2 - center);
        }

        public static Angle Calculate(Point center, Point p)
        {
            return (Angle)Vector.AngleBetween(p - center, new Vector(100, 0));
        }

        public Vector Circle(double radius)
        {
            double val = this.angle * (Math.PI / 180.0);
            double sin = Math.Sin(val);
            double cos = Math.Cos(val);

            return new Vector(sin * radius, cos * radius);
        }
    }
}
