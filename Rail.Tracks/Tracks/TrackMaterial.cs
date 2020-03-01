using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.Tracks
{
    public class TrackMaterial
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public string Manufacturer { get; set; }
        public string Article { get; set; }
        public string Name { get; set; }
    }
}
