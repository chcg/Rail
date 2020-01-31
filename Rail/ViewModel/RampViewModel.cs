using Rail.Model;
using Rail.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.ViewModel
{
    public class RampViewModel : DialogViewModel
    {
        public RampViewModel()
        {
        }

        protected override void OnOK()
        {
            base.OnOK();
        }

        private RailRamp railRamp = null;

        public RailRamp RailRamp
        {
            get
            {
                return this.railRamp;
            }
            set
            {
                this.railRamp = value;
                NotifyPropertyChanged(nameof(RailRamp));
            }
        }
    }
}
