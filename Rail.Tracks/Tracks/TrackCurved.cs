using Rail.Tracks.Properties;
using Rail.Tracks.Trigonometry;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Tracks
{
    public class TrackCurved : TrackBaseSingle
    {
        [XmlAttribute("Radius")]
        public string RadiusNameOrValue { get; set; }

        [XmlIgnore, JsonIgnore]
        public double Radius { get; set; }

        [XmlIgnore, JsonIgnore]
        public string RadiusName { get; set; }

        [XmlAttribute("Angle")]
        public double Angle { get; set; }

        [XmlAttribute("Extra")]
        public TrackExtras Extra { get; set; }

        [XmlIgnore, JsonIgnore]
        public override double RampLength { get { return 0; /* TODO calc length */  } }

        [XmlIgnore, JsonIgnore]
        public override string Name
        {
            get
            {
                return this.Extra switch
                {
                    TrackExtras.No => $"{Resources.TrackCurved} {RadiusName} {Radius} mm {Angle}°",
                    TrackExtras.Circuit => $"{Resources.TrackCurvedCircuit} {RadiusName} {Radius} mm {Angle}°",
                    TrackExtras.Contact => $"{Resources.TrackCurvedContact} {RadiusName} {Radius} mm {Angle}°",
                    TrackExtras.Uncoupler => $"{Resources.TrackCurvedUncoupler} {RadiusName} {Radius} mm {Angle}°",
                    TrackExtras.Isolating => $"{Resources.TrackCurvedIsolating} {RadiusName} {Radius} mm {Angle}°",
                    TrackExtras.Feeder => $"{Resources.TrackCurvedFeeder} {RadiusName} {Radius} mm {Angle}°",
                    _ => null

                };
            }
        }

        [XmlIgnore, JsonIgnore]
        public override string Description
        {
            get
            {
                return this.Extra switch
                {
                    TrackExtras.No => $"{this.Article} {Resources.TrackCurved} {Radius} {Radius} mm {Angle}°",
                    TrackExtras.Circuit => $"{this.Article} {Resources.TrackCurvedCircuit} {RadiusName} {Radius} mm {Angle}°",
                    TrackExtras.Contact => $"{this.Article} {Resources.TrackCurvedContact} {RadiusName} {Radius} mm {Angle}°",
                    TrackExtras.Uncoupler => $"{this.Article} {Resources.TrackCurvedUncoupler} {RadiusName} {Radius} mm {Angle}°",
                    TrackExtras.Isolating => $"{this.Article} {Resources.TrackCurvedIsolating} {RadiusName} {Radius} mm {Angle}°",
                    TrackExtras.Feeder => $"{this.Article} {Resources.TrackCurvedFeeder} {RadiusName} {Radius} mm {Angle}°",
                    _ => null
                };
            }
        }

        public override void Update(TrackType trackType)
        {
            this.Radius = GetValue(trackType.Radii, this.RadiusNameOrValue);
            this.RadiusName = GetName(this.RadiusNameOrValue);
            base.Update(trackType);
        }

        protected override Geometry CreateGeometry()
        {
            return CurvedGeometry(this.Angle, this.Radius, CurvedOrientation.Center, new Point(0, 0));
        }

        protected override Drawing CreateRailDrawing()
        {
            DrawingGroup drawingRail = new DrawingGroup();
            if (this.HasBallast)
            {
                drawingRail.Children.Add(CurvedBallast(this.Angle, this.Radius, CurvedOrientation.Center, new Point(0, 0)));
            }
            drawingRail.Children.Add(CurvedSleepers(this.Angle, this.Radius, CurvedOrientation.Center, new Point(0, 0)));
            drawingRail.Children.Add(CurvedRail(this.Angle, this.Radius, CurvedOrientation.Center, new Point(0, 0)));
            return drawingRail;
        }

        protected override List<TrackDockPoint> CreateDockPoints()
        {
            Point circleCenter = new Point(0, this.Radius);
            return new List<TrackDockPoint>
            {
                //new TrackDockPoint(0, circleCenter - PointExtentions.Circle(-this.Angle / 2, this.Radius),  this.Angle / 2 -  45, this.dockType),
                //new TrackDockPoint(1, circleCenter - PointExtentions.Circle( this.Angle / 2, this.Radius), -this.Angle / 2 + 135, this.dockType)
                new TrackDockPoint(0, circleCenter.CircleCenter(-this.Angle / 2, this.Radius),  this.Angle / 2 -  45, this.dockType),
                new TrackDockPoint(1, circleCenter.CircleCenter( this.Angle / 2, this.Radius), -this.Angle / 2 + 135, this.dockType)
            };
        }
    }
}
