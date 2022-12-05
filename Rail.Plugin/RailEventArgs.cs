using System;
using System.Drawing;

namespace Rail.Plugin
{
    public abstract class RailEventArgs : EventArgs
    {
        public Point Position { get; }
        public RailState State { get; set; }
    }
}
