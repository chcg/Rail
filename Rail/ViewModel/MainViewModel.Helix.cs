using Rail.Controls;
using Rail.Model;
using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rail.ViewModel
{
    public sealed partial class MainViewModel
    {
        private void OnCreateHelix()
        { }

        private bool OnCanCreateHelix()
        {
            return this.SelectedMode == RailSelectedMode.Multi &&
                    SelectedRails.All(r => r is RailItem ri && ri.Track is TrackCurved) &&
                    SelectedRails.Count() >= 16;
        }

        private void OnDeleteHelix()
        { }

        private bool OnCanDeleteHelix()
        {
            return this.SelectedMode == RailSelectedMode.Single && this.selectedRail is RailHelix;
        }

        private void OnEditHelix()
        { }

        private bool OnCanEditHelix()
        {
            return this.SelectedMode == RailSelectedMode.Single && this.selectedRail is RailHelix;
        }
    }
}
