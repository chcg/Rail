using Rail.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Rail.ViewModel
{
    public sealed partial class MainViewModel
    {
        private List<RailBase> copy = null;
        private int copyFactor;

        public void Clone()
        {
            // clone tree
            this.railPlan = this.railPlan.Clone();
            // clone dock point links
            RailDockPoint.CloneDockPointLinks();
        }

        private void OnCopy()
        {
            if (OnCanCopy())
            {
                this.copy = SelectedRails.ToList();
                this.copyFactor = 1;
            }
        }

        private bool OnCanCopy()
        {
            return this.railPlan.Rails.Any(r => r.IsSelected);
        }

        private void OnCut()
        {
            if (OnCanCut())
            {
                this.copy = SelectedRails.ToList();
                this.copy.ForEach(r => DeleteRailItem(r));
                this.copyFactor = 1;
                StoreToHistory();
                this.Invalidate();
            }
        }
        private bool OnCanCut()
        {
            return this.railPlan.Rails.Any(r => r.IsSelected);
        }

        private void OnPaste()
        {
            if (OnCanPaste())
            {
                this.railPlan.Rails.AddRange(copy.Select(r => r.Clone().Move(new Vector(copyPositionDrift * this.copyFactor, copyPositionDrift * this.copyFactor))));
                this.copyFactor++;
                // clone dock point links
                RailDockPoint.CloneDockPointLinks();

                StoreToHistory();
                this.Invalidate();
            }
        }

        private bool OnCanPaste()
        {
            return copy != null;
        }

        private void OnDelete()
        {
            if (OnCanDelete())
            {
                var list = SelectedRails.ToList();
                list.ForEach(r => this.DeleteRailItem(r));
                StoreToHistory();
                this.Invalidate();
            }
        }

        private bool OnCanDelete()
        {
            return this.railPlan.Rails.Any(r => r.IsSelected);
        }

        private void OnDuplicate()
        {
            if (OnCanDuplicate())
            {
                var selectedRails = SelectedRails;
                this.railPlan.Rails.AddRange(selectedRails.Select(r => r.Clone().Move(new Vector(copyPositionDrift * this.copyFactor, copyPositionDrift * this.copyFactor))));
                this.copyFactor = 1;
                // clone dock point links
                RailDockPoint.CloneDockPointLinks();

                StoreToHistory();
                this.Invalidate();
            }
        }

        private bool OnCanDuplicate()
        {
            return this.railPlan.Rails.Any(r => r.IsSelected);
        }

        private void OnSelectAll()
        {
            this.railPlan.Rails.ForEach(r => r.IsSelected = true);
            this.Invalidate();
        }

        private bool OnCanSelectAll()
        {
            return this.railPlan.Rails.Count() > 0;
        }

        //private void OnUnselectAll()
        //{
        //    this.RailPlan.Rails.ForEach(r => r.IsSelected = false);
        //    this.Invalidate();
        //}
    }
}
