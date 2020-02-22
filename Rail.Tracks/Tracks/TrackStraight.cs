using Rail.Tracks.Properties;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Tracks
{

    public class TrackStraight : TrackBaseSingle
    {

        [XmlAttribute("Length")]
        public string LengthNameOrValue { get; set; }

        [XmlAttribute("Extra")]
        public TrackExtras Extra { get; set; }

        [XmlIgnore, JsonIgnore]
        public double Length { get; set; }

        [XmlIgnore, JsonIgnore]
        public string LengthName { get; set; }

        [XmlIgnore, JsonIgnore]
        public override double RampLength { get { return this.Length; } }

        [XmlIgnore, JsonIgnore]
        public override string Name 
        { 
            get 
            {
                return this.Extra switch
                {
                    TrackExtras.No => $"{Resources.TrackStraight} {LengthName} {Length} mm",
                    TrackExtras.Circuit => $"{Resources.TrackStraightCircuit}",
                    TrackExtras.Contact => $"{Resources.TrackStraightContact}",
                    TrackExtras.Uncoupler => $"{Resources.TrackStraightUncoupler} {Length} mm",
                    TrackExtras.Isolating => $"{Resources.TrackStraightIsolating} {Length} mm",
                    TrackExtras.Feeder => $"{Resources.TrackStraightFeeder} {Length} mm",
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
                    TrackExtras.No => $"{this.Article} {Resources.TrackStraight} {LengthName} {Length} mm",
                    TrackExtras.Circuit => $"{this.Article} {Resources.TrackStraightCircuit}",
                    TrackExtras.Contact => $"{this.Article} {Resources.TrackStraightContact}",
                    TrackExtras.Uncoupler => $"{this.Article} {Resources.TrackStraightUncoupler} {Length} mm",
                    TrackExtras.Isolating => $"{this.Article} {Resources.TrackStraightIsolating} {Length} mm",
                    TrackExtras.Feeder => $"{this.Article} {Resources.TrackStraightFeeder} {Length} mm",
                    _ => null
                };
            }
        }

        public override void Update(TrackType trackType)
        {
            this.Length = GetValue(trackType.Lengths, this.LengthNameOrValue);
            this.LengthName = GetName(this.LengthNameOrValue);
            base.Update(trackType);
        }

        protected override Geometry CreateGeometry()
        {
            return StraitGeometry(this.Length, StraitOrientation.Center); 
        }

        protected override Drawing CreateRailDrawing()
        {
            DrawingGroup drawingRail = new DrawingGroup();
            if (this.HasBallast)
            {
                drawingRail.Children.Add(StraitBallast(this.Length, StraitOrientation.Center, 0, null));
            }
            drawingRail.Children.Add(StraitSleepers(this.Length));
            drawingRail.Children.Add(StraitRail(this.Length));
            return drawingRail;
        }

        protected override List<TrackDockPoint> CreateDockPoints()
        {
            return new List<TrackDockPoint>
            {
                new TrackDockPoint(0, new Point(-this.Length / 2.0, 0.0), 135, this.dockType),
                new TrackDockPoint(1, new Point(+this.Length / 2.0, 0.0), 315, this.dockType)
            };
        }
    }
}
