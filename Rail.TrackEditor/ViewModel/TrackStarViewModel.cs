using Rail.Tracks;

namespace Rail.TrackEditor.ViewModel
{
    public class TrackStarViewModel : TrackViewModel
    {
        private readonly TrackStar track;

        public TrackStarViewModel(TrackTypeViewModel trackTypeViewModel) : this(trackTypeViewModel, new TrackStar())
        { }

        public TrackStarViewModel(TrackTypeViewModel trackTypeViewModel, TrackStar track) : base(trackTypeViewModel, track)
        {
            this.track = track;
        }

        public string Article
        {
            get { return this.track.Article; }
            set { this.track.Article = value.Trim(); NotifyPropertyChanged(nameof(Article)); }
        }

        public TrackNamedValueViewModel Length
        {
            get { return GetLength(this.track.LengthId); }
            set { this.track.LengthId = value.Id; NotifyPropertyChanged(nameof(Length)); }
        }

        public int Number
        {
            get { return this.track.Number; }
            set { this.track.Number = value; NotifyPropertyChanged(nameof(Number)); }
        }
    }
}
