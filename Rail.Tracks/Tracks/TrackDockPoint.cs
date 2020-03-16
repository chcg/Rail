using System;
using System.Windows;

namespace Rail.Tracks
{
    public class TrackDockPoint
    {
        public TrackDockPoint(int debugIndex, Point position, double angle, Guid dockType)
        {
            this.DebugIndex = debugIndex;
            this.Position = position;
            this.Angle = angle;
            this.DockType = dockType;
        }

        public int DebugIndex { get; }

        public Point Position { get; set; }
        
        public double Angle { get; set; }        

        public Guid DockType { get; }
    } 
}
