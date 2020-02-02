using Rail.Model;
using Rail.Mvvm;
using Rail.Properties;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.ViewModel
{
    public class RampViewModel : DialogViewModel
    {
        public RampViewModel()
        {
            this.MaxPitch = Settings.Default.RampMaxPitch;
            this.KinkAngle = Settings.Default.RampKinkAngle;
        }

        protected override void OnOK()
        {
            base.OnOK();
        }

        private double maxPitch = 0.0;

        public double MaxPitch
        {
            get
            {
                return this.maxPitch;
            }
            set
            {
                this.maxPitch = value;
                NotifyPropertyChanged(nameof(MaxPitch));
            }
        }

        private double kinkAngle = 0.0;

        public double KinkAngle
        {
            get
            {
                return this.kinkAngle;
            }
            set
            {
                this.kinkAngle = value;
                NotifyPropertyChanged(nameof(KinkAngle));
            }
        }

        private double layerHight = 0.0;

        public double LayerHight
        {
            get
            {
                return this.layerHight;
            }
            set
            {
                this.layerHight = value;
                NotifyPropertyChanged(nameof(LayerHight));
            }
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
