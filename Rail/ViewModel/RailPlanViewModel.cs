using Rail.Model;
using Rail.Mvvm;
using System.Collections.ObjectModel;
using System.Linq;
using Rail.Misc;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Media;
using System.Reflection;

namespace Rail.ViewModel
{
    public class RailPlanViewModel : DialogViewModel
    {
        private readonly RailPlan railPlan;

        public RailPlanViewModel(RailPlan railPlan)
        {
            this.railPlan = railPlan;
            this.PlatePoints = new ObservableCollection<PointViewModel>(railPlan.PlatePoints.Select(p => new PointViewModel(p)));
            this.Layers = new ObservableCollection<LayerViewModel>(railPlan.Layers.Select(l => new LayerViewModel(l)));
        }

        protected override void OnOK()
        {
            this.railPlan.PlatePoints.Clear();
            this.railPlan.PlatePoints.AddRange(this.PlatePoints.Select(p => (Point)p));
            this.railPlan.Layers.Clear();
            this.railPlan.Layers.AddRange(this.Layers.Select(l => (RailLayer)l));
            base.OnOK();
        }

        
        public ObservableCollection<PointViewModel> PlatePoints { get; private set; }

        public ObservableCollection<LayerViewModel> Layers { get; private set; }
    }
}
