using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rail.TrackEditor.ViewModel
{
    public class TrackDoubleCrossoverViewModel : TrackViewModel
    {
        private readonly TrackDoubleCrossover track;
        
        public TrackDoubleCrossoverViewModel(TrackTypeViewModel trackTypeViewModel) : this(trackTypeViewModel, new TrackDoubleCrossover())
        { }

        public TrackDoubleCrossoverViewModel(TrackTypeViewModel trackTypeViewModel, TrackDoubleCrossover track) : base(trackTypeViewModel, track)
        {
            this.track = track;
        }

        public static TrackViewModel CreateNew(TrackTypeViewModel trackTypeViewModel)
        {
            TrackDoubleCrossover trackDoubleCrossover = new TrackDoubleCrossover
            {
                Article = string.Empty,
                LengthId = trackTypeViewModel.Lengths.First().Id
            };
            return new TrackDoubleCrossoverViewModel(trackTypeViewModel, trackDoubleCrossover);
        }

        public string Article
        {
            get { return this.track.Article; }
            set { this.track.Article = value.Trim(); NotifyPropertyChanged(nameof(Article)); }
        }
    }
}
