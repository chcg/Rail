using Rail.Mvvm;
using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Rail.TrackEditor.ViewModel
{
   
    [DebuggerDisplay("{Id} {Name}")]
    public class TrackNameViewModel : BaseViewModel
    {
        private readonly TrackName trackName;

        public TrackNameViewModel() : this(new TrackName())
        { }

        public TrackNameViewModel(TrackName trackName)
        {
            this.trackName = trackName;
        }

        public TrackName TrackName { get { return this.trackName; } }

        public Guid Id
        {
            get { return this.trackName.Id; }
        }

        public string Name
        {
            get { return this.trackName.Name; }
            set { this.trackName.Name = value.Trim(); NotifyPropertyChanged(nameof(Name)); }
        }

        public static TrackNameViewModel Null
        {
            get { return new TrackNameViewModel(new TrackName() { Id = Guid.Empty, Name = "" }); }
        }
    }
}
