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
        private double angle;

        public TrackDockPoint(TrackBase track)
        {
            this.Track = track;
        }

        public TrackDockPoint(TrackBase track, double x, double y, double angle)
        {
            this.Track = track;
            this.X = x;
            this.Y = y;
            this.angle = angle % 360.0;
        }

        public TrackDockPoint(TrackBase track, Point point, double angle)
        {
            this.Track = track;
            this.X = point.X;
            this.Y = point.Y;
            this.angle = angle % 360.0;
        }

        public static implicit operator Point(TrackDockPoint dockPoint)
        {
            return new Point(dockPoint.X, dockPoint.Y);
        }

        public static implicit operator RailItem(TrackDockPoint dockPoint)
        {
            return null; // dockPoint.Track;
        }

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

        public double Angle
        {
            get
            {
                return this.angle;
            }
            set
            {
                this.angle = value % 360.0;
            }
        }

        public TrackDockPoint Dock { get; set; }

        public bool IsDocked { get { return this.Dock != null; } }

        public TrackBase Track { get; private set; }

    } 
}
