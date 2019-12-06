using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.Misc
{
    public class Angle
    {
        private const ushort MAX = 3600;
        private const ushort REV = 1800;
        private ushort angle;

        public Angle(double value)
        {
            int val = (int)Math.Round(value * 10.0);
            this.angle = (ushort)((val % MAX + MAX) % MAX);
        }

        public Angle(ushort value)
        {
            this.angle = (ushort)(value % MAX);
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

        public Angle Revert()
        {
            return new Angle((ushort)(this.angle + REV));
        }
    }
}
