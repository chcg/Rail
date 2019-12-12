using Rail.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Rail.ViewModel
{
    public class PointViewModel : BaseViewModel
    {
        private uint x;
        private uint y;

        public event EventHandler OnChange;

        public PointViewModel()
        { }

        public PointViewModel(Point point)
        {
            this.X = (uint)Math.Round(point.X);
            this.Y = (uint)Math.Round(point.Y);
        }

        public uint X
        {
            get
            {
                return this.x;
            }
            set
            {
                if (this.x != value)
                {
                    this.x = value;
                    NotifyPropertyChanged(nameof(X));
                    this.OnChange?.Invoke(this, new EventArgs());
                }
            }
        }

        public uint Y
        {
            get
            {
                return this.y;
            }
            set
            {
                if (this.y != value)
                {
                    this.y = value;
                    NotifyPropertyChanged(nameof(Y));
                    this.OnChange?.Invoke(this, new EventArgs());
                }
            }
        }

        public static implicit operator PointViewModel(Point point)
        {
            return new PointViewModel(point);
        }

        public static implicit operator Point(PointViewModel point)
        {
            return new Point(point.X, point.Y);
        }

        public static implicit operator Vector(PointViewModel point)
        {
            return new Vector(point.X, point.Y);
        }
    }
}
