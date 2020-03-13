using Rail.Mvvm;
using Rail.Tracks;
using System;
using System.Diagnostics;

namespace Rail.TrackEditor.ViewModel
{
    [DebuggerDisplay("{Id} {Name} {Value}")]
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

        public Guid Id
        {
            get { return this.namedValue.Id; }
        }

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

        public static TrackNamedValueViewModel Null
        {
            get { return new TrackNamedValueViewModel(new TrackNamedValue() { Id = Guid.Empty, Name = "0", Value = 0 }); }
        }

    }
}
