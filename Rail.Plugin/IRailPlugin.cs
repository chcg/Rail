using System;
using System.Windows;
using System.Windows.Controls.Ribbon;

namespace Rail.Plugin
{
    public interface IRailPlugin
    {
        void Register(Window mainWindow, IRailPlan railPlan, RibbonTab pluginRibbonTab);

        void MouseEvent(RailMouseEventArgs args);
        void KeyEvent(RailKeyEventArgs args);
    }
}
