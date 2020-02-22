﻿using System.Windows;

namespace Rail.Tracks
{
    public class TrackDockPoint
    {
        public TrackDockPoint(int debugIndex, Point position, double angle, string dockType)
        {
            this.DebugIndex = debugIndex;
            this.Position = position;
            this.Angle = angle;
            this.DockType = dockType;
        }

        public int DebugIndex { get; private set;}

        public Point Position { get; set; }
        
        public double Angle { get; set; }        

        public string DockType { get; private set; }
    } 
}