using Rail.Controls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Model
{

    public class TrackStraight : TrackBase
    {

        [XmlAttribute("Length")]
        public double Length { get; set; }

        public override void Update(double spacing)
        {
            base.Update(spacing);

            this.Geometry = CreateStraitTrackGeometry(this.Length);
            this.RailDrawing = CreateStraitTrackDrawing(this.Length);

            this.DockPoints = new List<TrackDockPoint> 
            { 
                new TrackDockPoint(-this.Length / 2.0, 0.0, 135), 
                new TrackDockPoint( this.Length / 2.0, 0.0, 315) 
            };
        }
    }
}
