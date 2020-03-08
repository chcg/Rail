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
    public class TrackCrossing : TrackBaseSingle
    {
        #region store 

        [XmlElement("LengthA")]
        public Guid LengthAId { get; set; }

        [XmlElement("LengthB")]
        public Guid LengthBId { get; set; }

        [XmlElement("CrossingAngle")]
        public Guid CrossingAngleId { get; set; }

        #endregion

        #region internal

        [XmlIgnore, JsonIgnore]
        public double LengthA { get; set; }

        [XmlIgnore, JsonIgnore]
        public double LengthB { get; set; }

        [XmlIgnore, JsonIgnore]
        public double CrossingAngle { get; set; }

        #endregion

        #region override

        [XmlIgnore, JsonIgnore]
        public override double RampLength { get { return Math.Max(this.LengthA, this.LengthB); } }
        
        public override void Update(TrackType trackType)
        {
            this.LengthA = GetValue(trackType.Lengths, this.LengthAId);
            this.LengthB = GetValue(trackType.Lengths, this.LengthBId);
            this.CrossingAngle = GetValue(trackType.Angles, this.CrossingAngleId);

            this.Name = $"{Resources.TrackCrossing}";
            this.Description = $"{this.Article} {Resources.TrackCrossing}";
            
            base.Update(trackType);
        }

        protected override Geometry CreateGeometry()
        {
            return new CombinedGeometry(
                StraitGeometry(this.LengthA, StraitOrientation.Center, -this.CrossingAngle / 2),
                StraitGeometry(this.LengthB, StraitOrientation.Center, +this.CrossingAngle / 2));
        }

        protected override Drawing CreateRailDrawing()
        {
            DrawingGroup drawingRail = new DrawingGroup();
            if (this.HasBallast)
            {
                drawingRail.Children.Add(StraitBallast(this.LengthA, StraitOrientation.Center, -this.CrossingAngle / 2));
                drawingRail.Children.Add(StraitBallast(this.LengthB, StraitOrientation.Center, +this.CrossingAngle / 2));
            }
            drawingRail.Children.Add(StraitSleepers(this.LengthA, StraitOrientation.Center, -this.CrossingAngle / 2));
            drawingRail.Children.Add(StraitSleepers(this.LengthB, StraitOrientation.Center, +this.CrossingAngle / 2));
            drawingRail.Children.Add(StraitRail(this.LengthA, StraitOrientation.Center, -this.CrossingAngle / 2));
            drawingRail.Children.Add(StraitRail(this.LengthB, StraitOrientation.Center, +this.CrossingAngle / 2));
            return drawingRail;
        }

        protected override List<TrackDockPoint> CreateDockPoints()
        {
            return new List<TrackDockPoint>
            {
                new TrackDockPoint(0, new Point(-this.LengthA / 2.0, 0.0).Rotate( this.CrossingAngle /2),  this.CrossingAngle /2 + 135, this.dockType),
                new TrackDockPoint(1, new Point(-this.LengthA / 2.0, 0.0).Rotate(-this.CrossingAngle /2), -this.CrossingAngle /2 + 135, this.dockType),
                new TrackDockPoint(2, new Point( this.LengthA / 2.0, 0.0).Rotate( this.CrossingAngle /2),  this.CrossingAngle /2 + 45-90, this.dockType),
                new TrackDockPoint(3, new Point( this.LengthA / 2.0, 0.0).Rotate(-this.CrossingAngle /2), -this.CrossingAngle /2 + 45-90, this.dockType),
            };
        }

        #endregion
    }
}
