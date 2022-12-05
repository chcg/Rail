using Rail.Controls;
using Rail.Model;
using Rail.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rail.ViewModel
{
    public sealed partial class MainViewModel
    {
        private bool OnCanCreateRamp()
        {
            return
                this.SelectedMode == RailSelectedMode.Multi &&
                SelectedRails.All(r => r is RailItem) &&
                SelectedRails.Count() >= 8;
        }

        private void OnCreateRamp()
        {
            // take all selected rails
            var selectedRails = SelectedRails;

            RailRamp railRamp = new RailRamp(selectedRails);

            RampView rampView = new RampView { DataContext = new RampViewModel { RailRamp = railRamp, LayerHight = railRamp.LayerHeigh } };
            if (rampView.ShowDialog().Value)
            {
                // remove from Rails
                selectedRails.ForEach(r => this.railPlan.Rails.Remove(r));
                // add rail group
                this.railPlan.Rails.Add(railRamp);
                Invalidate();
            }
        }

        private void OnDeleteRamp()
        {
            if (OnCanDeleteRamp() && this.selectedRail is RailRamp railRamp)
            {
                this.railPlan.Rails.AddRange(railRamp.Resolve());
                this.railPlan.Rails.Remove(railRamp);
                Invalidate();
            }
        }

        private bool OnCanDeleteRamp()
        {
            return this.SelectedMode == RailSelectedMode.Single && this.selectedRail is RailRamp;
        }

        public void OnEditRamp()
        {
            if (OnCanEditRamp())
            {
                RampView rampView = new RampView { DataContext = new RampViewModel { RailRamp = (RailRamp)this.selectedRail } };
                if (rampView.ShowDialog().Value)
                {
                }
            }
        }

        private bool OnCanEditRamp()
        {
            return this.SelectedMode == RailSelectedMode.Single && this.selectedRail is RailRamp;
        }
    }
}
