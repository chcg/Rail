using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace Rail.Model
{
    public static class TrackBrushes
    {
        public static Brush Text { get { return Brushes.Black; } }

        public static Brush TrackFrame { get { return Brushes.Black; } }
        public static Brush TrackBackground { get { return Brushes.White; } }
        public static Brush TrackSelectedBackground { get { return Brushes.Yellow; } }

        public static Brush WoodenRail { get { return Brushes.Silver; } }
        public static Brush ConcreteRail { get { return Brushes.Black; } }
        public static Brush SelectedRail { get { return Brushes.Blue; } }

        public static Brush WoodenSleepers { get { return Brushes.Brown; } }
        public static Brush ConcreteSleepers { get { return Brushes.LightGray; } }
        public static Brush SelectedSleepers { get { return Brushes.Blue; } }

        public static Brush Ballast { get { return new SolidColorBrush(Color.FromRgb(0x51, 0x56, 0x5c)); } }

        public static Brush Dock { get { return Brushes.Blue; } }

        public static Brush Plate { get { return Brushes.Green; } } // new SolidColorBrush(Colors.Green) } }
        public static Brush PlateFrame { get { return Brushes.Black; } } // new SolidColorBrush(Colors.Green) } }
    }
}
