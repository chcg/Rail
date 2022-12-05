using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Rail.Plugin
{
    public class RailMouseEventArgs : RailEventArgs
    {
        public MouseButtonState ButtonState { get; }
        public MouseButton ChangedButton { get; }
        public int ClickCount { get; }
        public ModifierKeys Modifiers { get; }
        
    }
}
