using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Rail.Tracks.Trigonometry
{
    public static class PointExtentions
    {
        private static readonly double PIFactor = Math.PI / 180.0;

        public static Point Move(this Point point, double dx, double dy)
        {
            return point + new Vector(dx, dy);
        }

        public static Point Move(this Point point, Point? move)
        {
            return move == null ? point : point + (Vector)move;
        }

        public static Point Move(this Point point, Vector vec)
        {
            return point + vec;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="angle"></param>
        ///// <param name="radius"></param>
        ///// <returns></returns>
        //public static Vector Circle(double angle, double radius)
        //{
        //    angle *= PIFactor;
        //    double sin = Math.Sin(angle);
        //    double cos = Math.Cos(angle);

        //    return new Vector(sin * radius, cos * radius);
        //}


        //circleCenter - PointExtentions.Circle(startAngle, innerTrackRadius)

        public static Point CircleCenter(this Point pos, double angle, double radius)
        {
            angle *= PIFactor;
            double sin = Math.Sin(angle);
            double cos = Math.Cos(angle);

            return new Point(pos.X - sin * radius, pos.Y - cos * radius);
        }


        public static Point Circle(this Point pos, double angle, double radius)
        {
            angle *= PIFactor;
            double sin = Math.Sin(angle);
            double cos = Math.Cos(angle);

            return new Point(pos.X + cos * radius - sin * radius,
                             pos.Y + sin * radius + cos * radius);
        }

        /// <summary>
        /// Rotate a position around the zero point
        /// </summary>
        /// <param name="pos">Position to ratate</param>
        /// <param name="angle">Angle to rotate</param>
        /// <returns>Rotated position</returns>
        public static Point Rotate(this Point pos, double angle)
        {
            if (angle == 0.0)
            {
                return pos;
            }

            angle *= PIFactor;
            double sin = Math.Sin(angle);
            double cos = Math.Cos(angle);

            return new Point(cos * pos.X - sin * pos.Y,
                             sin * pos.X + cos * pos.Y);
        }

        /// <summary>
        /// Rotate a position around the center point
        /// </summary>
        /// <param name="pos">Position to ratate</param>
        /// <param name="angle">Angle to rotate</param>
        /// <param name="center">Center pointer around which is turned.</param>
        /// <returns>Rotated position</returns>
        public static Point Rotate(this Point pos, double angle, Point center)
        {
            angle *= PIFactor;
            double sin = Math.Sin(angle);
            double cos = Math.Cos(angle);

            return new Point(center.X + cos * (pos.X - center.X) - sin * (pos.Y - center.Y),
                             center.Y + sin * (pos.X - center.X) + cos * (pos.Y - center.Y));
        }
        
        public static Point Scale(this Point point, double factor)
        {
            return new Point(point.X * factor, point.Y * factor);
        }

        public static double Distance(this Point a, Point b)
        {
            return Math.Sqrt(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2));
        }

        public static Point Round(this Point point, int digits)
        {
            return new Point(Math.Round(point.X, digits), Math.Round(point.Y, digits));
        }

        //public static Point CircleCenter(this Point point, double angle, double radius)
        //{
        //    angle *= (Math.PI / 180.0);
        //    double sin = Math.Sin(angle);
        //    double cos = Math.Cos(angle);

        //    return new Point(
        //        point.X - cos * radius,
        //        point.Y - sin * radius);
        //}

        //public static Point CircleCenter(double angle, double radius)
        //{
        //    angle *= (Math.PI / 180.0);
        //    double sin = Math.Sin(angle);
        //    double cos = Math.Cos(angle);

        //    return new Point(
        //        -cos * radius,
        //        -sin * radius);
        //}


    }
}
