using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Rail.Controls
{
    public class DockPoint 
    {
        public double X;
        public double Y;
        private double angle;

        public DockPoint(RailTrack track)
        {
            this.Track = track;
        }

        public DockPoint(RailTrack track, double x, double y, double angle)
        {
            this.Track = track;
            this.X = x;
            this.Y = y;
            this.angle = angle % 360.0;
        }

        public DockPoint(RailTrack track, Point point, double angle)
        {
            this.Track = track;
            this.X = point.X;
            this.Y = point.Y;
            this.angle = angle % 360.0;
        }

        public static implicit operator Point(DockPoint dockPoint)
        {
            return new Point(dockPoint.X, dockPoint.Y);
        }

        public static implicit operator RailTrack(DockPoint dockPoint)
        {
            return dockPoint.Track;
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

        public DockPoint Dock { get; set; }

        public bool IsDocked { get { return this.Dock != null; } }

        public RailTrack Track { get; private set; }

    } 
}
