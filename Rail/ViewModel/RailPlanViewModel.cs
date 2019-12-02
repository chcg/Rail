using Rail.Model;
using Rail.Mvvm;

namespace Rail.ViewModel
{
    public class RailPlanViewModel : DialogViewModel
    {
        private readonly RailPlan railPlan;

        public RailPlanViewModel(RailPlan railPlan)
        {
            this.railPlan = railPlan;
            this.Width1 = railPlan.Width1;
            this.Width2 = railPlan.Width2;
            this.Width3 = railPlan.Width3;
            this.Height1 = railPlan.Height1;
            this.Height2 = railPlan.Height2;
            this.Height3 = railPlan.Height3;
        }

        protected override void OnOK()
        {
            this.railPlan.Width1 = this.Width1;
            this.railPlan.Width2 = this.Width2;
            this.railPlan.Width3 = this.Width3;
            this.railPlan.Height1 = this.Height1;
            this.railPlan.Height2 = this.Height2;
            this.railPlan.Height3 = this.Height3;
            base.OnOK();
        }

        private int width1 = 0;
        public int Width1
        {
            get
            {
                return this.width1;
            }
            set
            {
                this.width1 = value;
                NotifyPropertyChanged(nameof(Width1));
            }
        }


        private int width2 = 0;
        public int Width2
        {
            get
            {
                return this.width2;
            }
            set
            {
                this.width2 = value;
                NotifyPropertyChanged(nameof(Width2));
            }
        }


        private int width3 = 0;
        public int Width3
        {
            get
            {
                return this.width3;
            }
            set
            {
                this.width3 = value;
                NotifyPropertyChanged(nameof(Width3));
            }
        }


        private int height1 = 0;
        public int Height1
        {
            get
            {
                return this.height1;
            }
            set
            {
                this.height1 = value;
                NotifyPropertyChanged(nameof(Height1));
            }
        }

        private int height2 = 0;
        public int Height2
        {
            get
            {
                return this.height2;
            }
            set
            {
                this.height2 = value;
                NotifyPropertyChanged(nameof(Height2));
            }
        }

        private int height3 = 0;
        public int Height3
        {
            get
            {
                return this.height3;
            }
            set
            {
                this.height3 = value;
                NotifyPropertyChanged(nameof(Height3));
            }
        }
    }
}
