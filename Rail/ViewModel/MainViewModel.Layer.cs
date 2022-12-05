using Rail.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.ViewModel
{
    public sealed partial class MainViewModel
    {
        private RailLayer insertLayer;
        public RailLayer InsertLayer
        {
            get
            {
                return this.insertLayer;
            }
            set
            {
                this.insertLayer = value;
                NotifyPropertyChanged(nameof(InsertLayer));
                Invalidate();
            }
        }

        //Layers
        //public IEnumerable<RailLayer> Layers
        //{
        //    get { return this.RailPlan?.Layers.Reverse<RailLayer>(); }
        //}
    }
}
