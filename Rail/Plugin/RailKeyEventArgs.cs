using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Rail.Plugin
{
    public class RailKeyEventArgs : RailEventArgs
    {
        public bool IsDown { get; }
        public bool IsRepeat { get; }
        public bool IsToggled { get; }
        public bool IsUp { get; }
        public Key Key { get; }
        public KeyStates KeyStates { get; }
    }
}
