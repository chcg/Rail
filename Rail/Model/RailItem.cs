using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Model
{
    [DebuggerDisplay("RailItem Id={Id} X={Position.X} Y={Position.Y} A={Angle}")]

    public class RailItem
    {
        public RailItem()
        { }

        public RailItem(TrackBase track, Point pos)
        {
            this.Id = track.Id;
            this.Track = track;
            this.Position = pos;
            this.Angle = 0.0;
        }

        //public RailItem(TrackBase track, double x, double y, double angle, int[] docks)
        //{
        //    this.Id = track.Id;
        //    this.Position.X = x;
        //    this.Position.Y = y;
        //    this.Angle = angle;
        //}

        [XmlAttribute("Id")]
        public string Id { get; set; }

        [XmlIgnore]
        public TrackBase Track { get; set; }

        [XmlIgnore]
        public List<TrackDockPoint> DockPoints { get { return this.Track.DockPoints; } }

        [XmlIgnore]
        public Point Position;

        [XmlAttribute("X")]
        public double X 
        {
            get { return this.Position.X; } 
            set { this.Position.X = value; }
        }

        [XmlAttribute("Y")]
        public double Y 
        {
            get { return this.Position.Y; }
            set { this.Position.Y = value; }
        }

        [XmlAttribute("Angle")]
        public double Angle { get; set; }

        [XmlArray("Docks")]
        [XmlArrayItem("Dock")]
        public RailDock[] Docks { get; set; }

        public void Render(DrawingContext drawingContext)
        {
            this.Track.Render(drawingContext);
        }

        public bool IsInside(Point point)
        {
            TransformGroup grp = new TransformGroup();
            grp.Children.Add(new TranslateTransform(this.Position.X, this.Position.Y));
            grp.Children.Add(new RotateTransform(this.Angle, this.Position.X, this.Position.Y));
            this.Track.Geometry.Transform = grp;
            return this.Track.Geometry.FillContains(point);
        }
    }
}
