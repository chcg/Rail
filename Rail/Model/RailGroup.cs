using Rail.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Rail.Model
{
    public class RailGroup : RailBase
    {
        public override void DrawRailItem(DrawingContext drawingContext, RailViewMode viewMode, RailLayer layer)
        { }

        public override void DrawDockPoints(DrawingContext drawingContext)
        { }

        public override bool IsInside(Point point, RailViewMode viewMode)
        {
            // TODO
            return false;
        }

        public override bool IsInside(Rect rec, RailViewMode viewMode)
        {
            // TODO
            return false;
        }
}
}
