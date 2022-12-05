using Rail.Controls;
using Rail.Misc;
using Rail.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rail.ViewModel
{
    public sealed partial class MainViewModel
    {
        private bool OnCanCreateGroup()
        {
            return
                this.SelectedMode == RailSelectedMode.Multi &&
                // cannot group other group
                this.SelectedRails.All(r => r is RailItem) &&
                // all must have the same layer
                this.SelectedRails.Select(r => r.Layer).Distinct().Count() == 1;
        }

        private void OnCreateGroup()
        {
            if (OnCanCreateGroup())
            {
                // take all selected rails
                var selectedRails = this.SelectedRails.ToArray();

                // create rail group
                this.railPlan.Rails.Add(new RailGroup(selectedRails));

                // remove from Rails
                selectedRails.ForEach(r => this.railPlan.Rails.Remove(r));

                Invalidate();
            }
        }

        private void OnResolveGroup()
        {
            if (OnCanResolveGroup() && this.SelectedRail is RailGroup railGroup)
            {
                this.railPlan.Rails.AddRange(railGroup.Resolve());
                this.railPlan.Rails.Remove(railGroup);
                Invalidate();
            }
        }

        private bool OnCanResolveGroup()
        {
            return this.SelectedMode == RailSelectedMode.Single && this.selectedRail is RailGroup;
        }

        private void OnSaveAsGroup()
        {
        }

        private bool OnCanSaveAsGroup()
        {
            return this.SelectedMode == RailSelectedMode.Single && this.selectedRail is RailGroup;
        }
    }
}
