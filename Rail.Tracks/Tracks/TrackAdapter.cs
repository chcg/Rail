using Rail.Tracks.Properties;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Tracks
{
    public class TrackAdapter : TrackBaseSingle
    {
        #region store

        [XmlAttribute("Length")]
        public string LengthName { get; set; }

        [XmlAttribute("DockType")]
        public string DockType { get; set; }

        #endregion

        #region internal

        [XmlIgnore, JsonIgnore]
        public double Length { get; set; }

        #endregion

        [XmlIgnore, JsonIgnore]
        public override double RampLength { get { return this.Length; } }
        
        public override void Update(TrackType trackType)
        {
            this.Length = GetValue(trackType.Lengths, this.LengthName);

            this.Name = $"{Resources.TrackAdapter}";
            this.Description = $"{this.Article} {Resources.TrackAdapter}";

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
