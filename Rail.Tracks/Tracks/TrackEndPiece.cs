using Rail.Tracks.Properties;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Tracks
{
    public class TrackEndPiece : TrackBaseSingle
    {
        #region store 

        [XmlElement("Length")]
        public Guid LengthId { get; set; }

        [XmlElement("Extra")]
        public TrackEndType EndType { get; set; }

        #endregion

        #region internal

        [XmlIgnore, JsonIgnore]
        public double Length { get; set; }

        #endregion

        #region override

        [XmlIgnore, JsonIgnore]
        public override double RampLength { get { return 0; } }

        public override void Update(TrackType trackType)
        {
            this.Length = GetValueOrNull(trackType.Lengths, this.LengthId);

            this.Name = this.EndType switch
            {
                TrackEndType.End => Resources.TrackEndPiece,
                TrackEndType.Bumper => Resources.TrackBumper,
                TrackEndType.BumperWithLantern => Resources.TrackBumperWithLantern,
                _ => string.Empty
            };
            this.Description = $"{this.Article} {this.Name}";
        
            base.Update(trackType);
        }

        protected override Geometry CreateGeometry()
        {
            double length = Math.Max(Length, this.RailWidth);

            return StraitGeometry(length, StraitOrientation.Left);
        }

        protected override Drawing CreateRailDrawing()
        {
            double length = Math.Max(Length, this.RailWidth);

            DrawingGroup drawingRail = new DrawingGroup();
            if (this.HasBallast)
            {
                drawingRail.Children.Add(StraitBallast(length));
            }
            drawingRail.Children.Add(StraitSleepers(length));
            drawingRail.Children.Add(StraitRail(length));
            return drawingRail;
        }

        protected override List<TrackDockPoint> CreateDockPoints()
        {
            return new List<TrackDockPoint>
            {
                new TrackDockPoint(0, new Point(this.RailWidth / 2.0, 0.0), 315, this.dockType)
            };
        }

        #endregion
    }
}
