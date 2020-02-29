using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace Rail.Tracks
{
    public static class TrackBrushes
    {
        private static readonly Brush darkBallast = new SolidColorBrush(Color.FromRgb(0x51, 0x56, 0x5c));
        private static readonly Brush copper = new SolidColorBrush(Color.FromRgb(0xb8, 0x73, 0x33));

        public static Brush Text { get { return Brushes.Black; } }

        public static Brush TrackFrame { get { return Brushes.Black; } }
        //public static Brush TrackBackground { get { return Brushes.White; } }

        public static Brush SilverRail { get { return Brushes.Silver; } }
        public static Brush CopperRail { get { return copper /*Brushes.Gold*/; } }
        public static Brush BlackRail { get { return Brushes.Black; } }

        public static Brush WoodenSleepers { get { return Brushes.Brown; } }
        public static Brush ConcreteSleepers { get { return Brushes.LightGray; } }

        public static Brush LightBallast { get { return Brushes.LightGray; } }
        public static Brush MediumBallast { get { return Brushes.Gray; } }
        public static Brush DarkBallast { get { return darkBallast; } }

        public static Brush Dock { get { return Brushes.Blue; } }

        public static Brush Plate { get { return Brushes.Green; } } // new SolidColorBrush(Colors.Green) } }
        public static Brush PlateFrame { get { return Brushes.Black; } } // new SolidColorBrush(Colors.Green) } }
    }
}
