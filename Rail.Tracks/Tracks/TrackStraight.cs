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
        public TrackStraightType StraightType { get; set; }

        [XmlElement("DockType")]
        public Guid DockType { get; set; }

        public bool ShouldSerializeStraightType() { return this.StraightType != TrackStraightType.No; }

        public bool ShouldSerializeDockType() { return this.StraightType == TrackStraightType.Adapter; }

        #endregion

        #region internal

        [XmlIgnore, JsonIgnore]
        public double Length { get; set; }

        #endregion

        #region override

        [XmlIgnore, JsonIgnore]
        public override TrackTypes TrackType { get { return TrackTypes.Straight; } }

        [XmlIgnore, JsonIgnore]
        public override double RampLength { get { return this.Length; } }

        public override TrackBase Clone()
        {
            TrackStraight track = new TrackStraight
            {
                Article = this.Article,
                LengthId = this.LengthId,
                StraightType = this.StraightType,
                DockType = this.DockType
            };
            track.Update(this.trackType);
            return track;
        }

        public override void Update(TrackType trackType)
        {
            this.Length = GetValue(trackType.Lengths, this.LengthId);

            string lengthName = GetName(trackType.Lengths, this.LengthId);
            this.Name = this.StraightType switch
            {
                TrackStraightType.No => $"{Resources.TrackStraight} {lengthName} {Length} mm",
                TrackStraightType.Circuit => $"{Resources.TrackStraightCircuit} {lengthName} {Length} mm",
                TrackStraightType.Contact => $"{Resources.TrackStraightContact} {lengthName} {Length} mm",
                TrackStraightType.Uncoupler => $"{Resources.TrackStraightUncoupler} {lengthName} {Length} mm",
                TrackStraightType.Isolating => $"{Resources.TrackStraightIsolating} {lengthName} {Length} mm",
                TrackStraightType.Separation => $"{Resources.TrackStraightSeparation}  {lengthName} {Length} mm",
                TrackStraightType.Feeder => $"{Resources.TrackStraightFeeder} {lengthName} {Length} mm",
                TrackStraightType.Adapter => $"{Resources.TrackStraightAdapter} {lengthName} {Length} mm {this.DockType}",
                TrackStraightType.Rerailer => $"{Resources.TrackStraightRetailer} {lengthName} {Length} mm",
                TrackStraightType.InterferenceSuppressor => $"{Resources.TrackStraightInterferenceSuppressor} {lengthName} {Length} mm",
                TrackStraightType.Crossing => $"{Resources.TrackStraightCrossing} {lengthName} {Length} mm",
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
                this.StraightType == TrackStraightType.Adapter ? this.DockType : this.dockType)
            };
        }

        #endregion
    }
}
