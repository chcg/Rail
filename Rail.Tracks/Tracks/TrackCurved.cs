using Rail.Tracks.Properties;
using Rail.Tracks.Trigonometry;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Tracks
{
    public class TrackCurved : TrackBaseSingle
    {
        #region store 
        
        [XmlElement("Radius")]
        public Guid RadiusId { get; set; }

        [XmlElement("Angle")]
        public Guid AngleId { get; set; }

        [XmlElement("Extra")]
        public TrackExtras Extra { get; set; }

        #endregion

        #region internal

        [XmlIgnore, JsonIgnore]
        public double Radius { get; set; }
        
        [XmlIgnore, JsonIgnore]
        public double Angle { get; set; }

        #endregion

        #region override

        [XmlIgnore, JsonIgnore]
        public override double RampLength { get { return 0; /* TODO calc length */  } }

        public override void Update(TrackType trackType)
        {
            this.Radius = GetValue(trackType.Radii, this.RadiusId);
            this.Angle = GetValue(trackType.Angles, this.AngleId);

            string radiusName = GetName(trackType.Radii, this.RadiusId);
            
            this.Name = this.Extra switch
                {
                    TrackExtras.No => $"{Resources.TrackCurved} {radiusName} {Radius} mm {Angle}°",
                    TrackExtras.Circuit => $"{Resources.TrackCurvedCircuit} {radiusName} {Radius} mm {Angle}°",
                    TrackExtras.Contact => $"{Resources.TrackCurvedContact} {radiusName} {Radius} mm {Angle}°",
                    TrackExtras.Uncoupler => $"{Resources.TrackCurvedUncoupler} {radiusName} {Radius} mm {Angle}°",
                    TrackExtras.Isolating => $"{Resources.TrackCurvedIsolating} {radiusName} {Radius} mm {Angle}°",
                    TrackExtras.Feeder => $"{Resources.TrackCurvedFeeder} {radiusName} {Radius} mm {Angle}°",
                    _ => null
                };

            this.Description = this.Extra switch
                {
                    TrackExtras.No => $"{this.Article} {Resources.TrackCurved} {radiusName} {Radius} mm {Angle}°",
                    TrackExtras.Circuit => $"{this.Article} {Resources.TrackCurvedCircuit} {radiusName} {Radius} mm {Angle}°",
                    TrackExtras.Contact => $"{this.Article} {Resources.TrackCurvedContact} {radiusName} {Radius} mm {Angle}°",
                    TrackExtras.Uncoupler => $"{this.Article} {Resources.TrackCurvedUncoupler} {radiusName} {Radius} mm {Angle}°",
                    TrackExtras.Isolating => $"{this.Article} {Resources.TrackCurvedIsolating} {radiusName} {Radius} mm {Angle}°",
                    TrackExtras.Feeder => $"{this.Article} {Resources.TrackCurvedFeeder} {radiusName} {Radius} mm {Angle}°",
                    _ => null
                };

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

        #endregion
    }
}
