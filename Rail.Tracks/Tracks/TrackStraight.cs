using Rail.Tracks.Properties;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Tracks
{

    public class TrackStraight : TrackBaseSingle
    {
        #region store 

        [XmlElement("Length")]
        public Guid LengthId { get; set; }

        [XmlElement("Extra")]
        public TrackExtras Extra { get; set; }

        [XmlElement("DockType")]
        public string DockType { get; set; }

        #endregion

        #region internal

        [XmlIgnore, JsonIgnore]
        public double Length { get; set; }

        #endregion

        #region override

        [XmlIgnore, JsonIgnore]
        public override double RampLength { get { return this.Length; } }

        public override void Update(TrackType trackType)
        {
            this.Length = GetValue(trackType.Lengths, this.LengthId);

            string lengthName = GetName(trackType.Lengths, this.LengthId);
            this.Name = this.Extra switch
            {
                TrackExtras.No => $"{Resources.TrackStraight} {lengthName} {Length} mm",
                TrackExtras.Circuit => $"{Resources.TrackStraightCircuit}",
                TrackExtras.Contact => $"{Resources.TrackStraightContact}",
                TrackExtras.Uncoupler => $"{Resources.TrackStraightUncoupler} {Length} mm",
                TrackExtras.Isolating => $"{Resources.TrackStraightIsolating} {Length} mm",
                TrackExtras.Feeder => $"{Resources.TrackStraightFeeder} {Length} mm",
                TrackExtras.Adapter => $"{Resources.TrackAdapter} {this.DockType}",
                _ => null
            };

            this.Description = $"{this.Article} {this.Name}";
            
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
                new TrackDockPoint(1, new Point(+this.Length / 2.0, 0.0), 315,
                this.Extra == TrackExtras.Adapter ? this.DockType : this.dockType)
            };
        }

        #endregion
    }
}
