using Rail.Tracks.Properties;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Tracks
{
    public class TrackFlex : TrackBaseSingle
    {
        #region store

        [XmlElement("FlexType")]
        public TrackFlexType FlexType { get; set; }

        [XmlElement("MinLength")]
        public Guid MinLengthId { get; set; }

        [XmlElement("MaxLength")]
        public Guid MaxLengthId { get; set; }

        #endregion

        #region internal

        [XmlIgnore, JsonIgnore]
        public double MinLength { get; set; }

        [XmlIgnore, JsonIgnore]
        public double MaxLength { get; set; }

        #endregion

        #region override

        [XmlIgnore, JsonIgnore]
        public override TrackTypes TrackType { get { return TrackTypes.Flex; } }

        [XmlIgnore, JsonIgnore]
        public override double RampLength { get { return this.MinLength; } }

        public override TrackBase Clone()
        {
            TrackFlex track = new TrackFlex
            {
                Article = this.Article,
                FlexType = this.FlexType,
                MinLengthId = this.MinLengthId,
                MaxLengthId = this.MaxLengthId
            };
            track.Update(this.trackType);
            return track;
        }

        public override void Update(TrackType trackType)
        {
            this.MinLength = GetValueOrNull(trackType.Lengths, this.MinLengthId);
            this.MaxLength = GetValueOrNull(trackType.Lengths, this.MaxLengthId);

            this.Name = this.FlexType switch
            {
                TrackFlexType.Adjustment => $"{Resources.TrackAdjustment}",
                TrackFlexType.Flex => $"{Resources.TrackFlex}",
                _ => null
            };
            this.Description = $"{this.Article} {this.Name}";

            base.Update(trackType);
        }
   
        protected override Geometry CreateGeometry()
        {
            return StraitGeometry(this.MinLength, StraitOrientation.Center);
        }

        protected override Drawing CreateRailDrawing()
        {
            DrawingGroup drawingRail = new DrawingGroup();
            if (this.HasBallast)
            {
                drawingRail.Children.Add(StraitBallast(this.MinLength, StraitOrientation.Center, 0, null));
            }
            drawingRail.Children.Add(StraitSleepers(this.MinLength));
            drawingRail.Children.Add(StraitRail(this.MinLength));
            return drawingRail;
        }

        protected override List<TrackDockPoint> CreateDockPoints()
        {
            return new List<TrackDockPoint>
            {
                new TrackDockPoint(0, new Point(-this.MinLength / 2.0, 0.0), 135, this.dockType),
                new TrackDockPoint(1, new Point(+this.MinLength / 2.0, 0.0), 315, this.dockType)
            };
        }

        #endregion
    }
}
