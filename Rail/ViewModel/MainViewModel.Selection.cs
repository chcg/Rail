using Rail.Controls;
using Rail.Misc;
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

        private RailSelectedMode selectedMode;
        public RailSelectedMode SelectedMode
        {
            get
            {
                return this.selectedMode;
            }
            set
            {
                this.selectedMode = value;
                NotifyPropertyChanged();
            }
        }

        private RailBase selectedRail;
        public RailBase SelectedRail
        {
            get
            {
                return this.selectedRail;
            }
            set
            {
                this.selectedRail = value;
                NotifyPropertyChanged();
            }
        }

        private List<RailBase> selectedRails;
        public List<RailBase> SelectedRails
        {
            get
            {
                return this.selectedRails; // this.railPlan.Rails.Where(r => r.IsSelected).ToList();
            }
            set
            {
                this.selectedRails = value;
                NotifyPropertyChanged();
            }
        }


        //public List<RailBase> SelectedRails
        //{
        //    get
        //    {
        //        return this.railPlan.Rails.Where(r => r.IsSelected).ToList();
        //    }
        //}

        public void SelectRailItem(RailBase railItem, bool addSelect)
        {
            if (addSelect)
            {
                railItem.IsSelected = !railItem.IsSelected;
            }
            else
            {
                this.railPlan.Rails.ForEach(r => r.IsSelected = false);
                railItem.IsSelected = true;
            }
            UpdateSelectedRails();
        }

        public void SelectRailItem(Rect rec, bool addSelect)
        {
            if (!addSelect)
            {
                this.railPlan.Rails.ForEach(r => r.IsSelected = false);
            }
            this.railPlan.Rails.Where(r => r.IsInside(rec)).ForEach(r => r.IsSelected = true);
            UpdateSelectedRails();
        }

        public void UnselectAllRailItems()
        {
            this.railPlan.Rails.ForEach(r => r.IsSelected = false);
            UpdateSelectedRails();
        }

        private void UpdateSelectedRails()
        {
            this.SelectedRails = this.railPlan.Rails.Where(r => r.IsSelected).ToList();
            switch (this.SelectedRails.Count())
            {
            case 0:
                this.SelectedMode = RailSelectedMode.None;
                this.SelectedRail = null;
                this.SelectedRailsX = null;
                this.SelectedRailsY = null;
                this.SelectedRailsAngle = null;
                this.SelectedRailsLayer = Guid.Empty;
                //this.SelectedRailsGradient = null;
                //this.SelectedRailsHeight = null;
                //this.SelectedRamp = null;
                break;
            case 1:
                this.SelectedMode = RailSelectedMode.Single;
                this.SelectedRail = this.SelectedRails.Single();

                this.SelectedRailsX = this.SelectedRail.Position.X;
                this.SelectedRailsY = this.SelectedRail.Position.Y;
                this.SelectedRailsAngle = this.SelectedRail.Angle;
                this.SelectedRailsLayer = this.SelectedRail.Layer;
                //this.SelectedRailsGradient = this.selectedRail.Gradient;
                //this.SelectedRailsHeight = this.selectedRail.Height;
                //this.SelectedRamp = this.selectedRail as RailRamp;
                break;
            default:
                this.SelectedMode = RailSelectedMode.Multi;
                this.SelectedRail = null; 
                this.SelectedRailsX = null;
                this.SelectedRailsY = null;
                this.SelectedRailsAngle = null;
                this.SelectedRailsLayer = selectedRails.Select(r => r.Layer).IdenticalOrDefault();
                //this.SelectedRailsGradient = selectedRails.Select(r => r.Gradient).IdenticalOrDefault();
                //this.SelectedRailsHeight = selectedRails.Select(r => r.Height).IdenticalOrDefault();
                //this.SelectedRamp = null;
                break;
            }
        }
    }
}
