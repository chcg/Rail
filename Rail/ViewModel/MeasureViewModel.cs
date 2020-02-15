using Rail.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.ViewModel
{
    public class MeasureViewModel : DialogViewModel
    {
        public MeasureViewModel()
        { }

        public double DistanceX { get; set; }

        public double DistanceY { get; set; }

        public double Distance { get; set; }
    }
}
