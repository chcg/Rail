using Rail.Model;
using Rail.Mvvm;
using System.Collections.ObjectModel;
using System.Linq;
using Rail.Misc;
using System.Windows;

namespace Rail.ViewModel
{
    public class RailPlanViewModel : DialogViewModel
    {
        private readonly RailPlan railPlan;

        public RailPlanViewModel(RailPlan railPlan)
        {
            this.railPlan = railPlan;
            this.PlatePoints = new ObservableCollection<PointViewModel>(railPlan.PlatePoints.Select(p => new PointViewModel(p)));
        }

        protected override void OnOK()
        {
            this.railPlan.PlatePoints.Clear();
            this.railPlan.PlatePoints.AddRange(this.PlatePoints.Select(p => (Point)p)); 
            base.OnOK();
        }

        
        public ObservableCollection<PointViewModel> PlatePoints { get; private set; }
    }
}
