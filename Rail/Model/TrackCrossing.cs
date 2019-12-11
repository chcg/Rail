using Rail.Misc;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Model
{
    public class TrackCrossing : TrackBase
    {
        [XmlAttribute("Length1")]
        public double Length1 { get; set; }

        [XmlAttribute("Length2")]
        public double Length2 { get; set; }

        [XmlAttribute("Angle")]
        public double Angle { get; set; }

        protected override void Create()
        {
            this.Geometry = CreateCrossingTrackGeometry(this.Length1, this.Length2, this.Angle);
            
            DrawingGroup drawing = new DrawingGroup();
            if (this.Ballast)
            {
                drawing.Children.Add(StraitBallast(this.Length1, StraitOrientation.Center, -this.Angle / 2));
                drawing.Children.Add(StraitBallast(this.Length2, StraitOrientation.Center, +this.Angle / 2));
            }
            drawing.Children.Add(StraitRail(this.Length1, StraitOrientation.Center, -this.Angle / 2));
            drawing.Children.Add(StraitRail(this.Length2, StraitOrientation.Center, +this.Angle / 2));
            this.RailDrawing = drawing;

            this.DockPoints = new List<TrackDockPoint>
            {
                new TrackDockPoint(new Point(-this.Length1 / 2.0, 0.0).Rotate( this.Angle /2),  this.Angle /2 + 135),
                new TrackDockPoint(new Point(-this.Length1 / 2.0, 0.0).Rotate(-this.Angle /2), -this.Angle /2 + 135),
                new TrackDockPoint(new Point( this.Length1 / 2.0, 0.0).Rotate( this.Angle /2),  this.Angle /2 + 45-90),
                new TrackDockPoint(new Point( this.Length1 / 2.0, 0.0).Rotate(-this.Angle /2), -this.Angle /2 + 45-90),
            };
        }
    }
}
