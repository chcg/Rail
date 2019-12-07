using Rail.Misc;
using Rail.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Rail.Model
{
    public class TrackDockPoint
    {
        public double X;
        public double Y;
        private Angle angle;


        public TrackDockPoint(double x, double y, double angle)
        {
            this.X = x;
            this.Y = y;
            this.angle = angle;
        }

        public TrackDockPoint(Point point, double angle)
        {
            this.X = point.X;
            this.Y = point.Y;
            this.angle = angle;
        }

        //public static implicit operator Point(TrackDockPoint dockPoint)
        //{
        //    return new Point(dockPoint.X, dockPoint.Y);
        //}

        //public static implicit operator RailItem(TrackDockPoint dockPoint)
        //{
        //    return null; // dockPoint.Track;
        //}

        public Point Position
        {
            get
            {
                return new Point(this.X, this.Y);
            }
            set
            {
                this.X = value.X;
                this.Y = value.Y;
            }
        }

        public Angle Angle
        {
            get
            {
                return this.angle;
            }
            set
            {
                this.angle = value;
            }
        }
    } 
}
