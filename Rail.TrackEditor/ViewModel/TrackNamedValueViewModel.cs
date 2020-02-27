using Rail.Mvvm;
using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.TrackEditor.ViewModel
{
    public class TrackNamedValueViewModel : BaseViewModel
    {
        private readonly TrackNamedValue namedValue;

        public TrackNamedValueViewModel() : this(new TrackNamedValue())
        { }

        public TrackNamedValueViewModel(TrackNamedValue namedValue)
        {
            this.namedValue = namedValue;
        }

        public TrackNamedValue NamedValue { get { return this.namedValue; } }

        public string Name
        {
            get { return this.namedValue.Name; }
            set { this.namedValue.Name = value.Trim(); NotifyPropertyChanged(nameof(Name)); }
        }

        public double Value
        {
            get { return this.namedValue.Value; }
            set { this.namedValue.Value = value; NotifyPropertyChanged(nameof(Value)); }
        }
    }
}
