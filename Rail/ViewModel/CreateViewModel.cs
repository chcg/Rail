using Rail.Model;
using Rail.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rail.ViewModel
{
    public class CreateViewModel : DialogViewModel
    {
        private Tracks tracks;

        public CreateViewModel(Tracks tracks)
        {
            this.tracks = tracks;
            this.Width = 2000;
            this.Height = 1000;
    }

    public List<TrackType> TrackTypes { get { return this.tracks.TrackTypes; } }

        public TrackType SelectedTrackType { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }
    }
}
