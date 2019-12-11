using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Rail.Misc
{
    public class Rectangle
    {
        private Rectangle()
        { }

        public Rectangle(double x, double y, double width, double height)
        {
            this.LeftTop = new Point(x, y);
            this.LeftBottom = new Point(x, y + height);
            this.RightTop = new Point(x + width, y);
            this.RightBottom = new Point(x + width, y + height);
        }

        public Point LeftTop { get; set; }
        public Point LeftBottom { get; set; }

        public Point RightTop { get; set; }
        public Point RightBottom { get; set; }

        public Rectangle Move(Vector move)
        {
            return new Rectangle() { LeftTop = this.LeftTop + move, LeftBottom = this.LeftBottom + move, RightTop = this.RightTop + move, RightBottom = this.RightBottom + move };
        }

        public Rectangle Rotate(double angle)
        {
            return new Rectangle() { LeftTop = this.LeftTop.Rotate(angle), LeftBottom = this.LeftBottom.Rotate(angle), RightTop = this.RightTop.Rotate(angle), RightBottom = this.RightBottom.Rotate(angle) };
        }

        public Rectangle Rotate(double angle, Point center)
        {
            return new Rectangle() { LeftTop = this.LeftTop.Rotate(angle, center), LeftBottom = this.LeftBottom.Rotate(angle, center), RightTop = this.RightTop.Rotate(angle, center), RightBottom = this.RightBottom.Rotate(angle, center) };
        }

        public Rectangle Move(Point? move)
        {
            return move.HasValue ? this.Move((Vector)move.Value) : this;
        }
    }
}
