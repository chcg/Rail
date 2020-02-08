using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.Model
{

    #pragma warning disable CS0414  

    public class RailBinder
    {
        private RailPlan railPlan;
        private TrackType trackType;
        private RailDockPoint from;
        private RailDockPoint to;

        public RailBinder()
        { }

        public static void Bind(RailPlan railPlan, TrackType trackType, RailDockPoint from, RailDockPoint to)
        {
            new RailBinder() { railPlan = railPlan, trackType = trackType, from = from, to = to }.Bind();
        }

        private void Bind()
        {

        }
    }
}
