using Rail.Controls;
using Rail.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rail.ViewModel
{
    public sealed partial class MainViewModel
    {
        private bool OnCanAnchor()
        {
            return this.SelectedMode == RailSelectedMode.Single && !this.selectedRail.IsAnchored;
        }

        private void OnAnchor()
        {
            if (OnCanAnchor())
            {
                this.SelectedRail.IsAnchored = true;
                this.Invalidate();
            }
        }

        private bool OnCanUnanchor()
        {
            return this.SelectedMode == RailSelectedMode.Single && this.selectedRail.IsAnchored;
        }

        private void OnUnanchor()
        {
            if (OnCanUnanchor())
            {
                this.selectedRail.IsAnchored = false;
                this.Invalidate();
            }
        }

        public bool IsAnchored(List<RailBase> rails)
        {
            return rails.Any(r => r.IsAnchored);
        }
    }
}
