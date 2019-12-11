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

        protected override void Create()
        {
            this.Geometry = CreateStraitTrackGeometry(this.Length);

            DrawingGroup drawing = new DrawingGroup();
            if (this.Ballast)
            {
                drawing.Children.Add(StraitBallast(this.Length, StraitOrientation.Center, 0, null));
            }
            drawing.Children.Add(StraitRail(this.Length));
            this.RailDrawing = drawing;  

            this.DockPoints = new List<TrackDockPoint> 
            { 
                new TrackDockPoint(-this.Length / 2.0, 0.0, 135), 
                new TrackDockPoint( this.Length / 2.0, 0.0, 315) 
            };
        }
    }
}
