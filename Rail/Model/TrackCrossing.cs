using Rail.Misc;
using Rail.Properties;
using Rail.Trigonometry;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Model
{
    public class TrackCrossing : TrackBaseSingle
    {
        [XmlAttribute("Length1")]
        public string Length1NameOrValue { get; set; }

        [XmlIgnore]
        public double Length1 { get; set; }

        [XmlIgnore]
        public string Length1Name { get; set; }

        [XmlAttribute("Length2")]
        public string Length2NameOrValue { get; set; }

        [XmlIgnore]
        public double Length2 { get; set; }

        [XmlIgnore]
        public string Length2Name { get; set; }

        [XmlAttribute("Angle")]
        public double Angle { get; set; }

        [XmlIgnore]
        public override string Name
        {
            get
            {
                return $"{Resources.TrackCrossing}";
            }
        }

        [XmlIgnore]
        public override string Description
        {
            get
            {
                return $"{this.Article} {Resources.TrackCrossing}";
            }
        }

        public override void Update(TrackType trackType)
        {
            this.Length1 = GetValue(trackType.Lengths, this.Length1NameOrValue);
            this.Length1Name = GetName(this.Length1NameOrValue);
            this.Length2 = GetValue(trackType.Lengths, this.Length1NameOrValue);
            this.Length2Name = GetName(this.Length1NameOrValue);
            base.Update(trackType);
        }

        protected override Geometry CreateGeometry(double spacing)
        {
            return new CombinedGeometry(
                StraitGeometry(this.Length1, StraitOrientation.Center, spacing, -this.Angle / 2),
                StraitGeometry(this.Length2, StraitOrientation.Center, spacing, +this.Angle / 2));
        }

        protected override Drawing CreateRailDrawing()
        {
            DrawingGroup drawingRail = new DrawingGroup();
            if (this.ViewType.HasFlag(TrackViewType.Ballast))
            {
                drawingRail.Children.Add(StraitBallast(this.Length1, StraitOrientation.Center, -this.Angle / 2));
                drawingRail.Children.Add(StraitBallast(this.Length2, StraitOrientation.Center, +this.Angle / 2));
            }
            drawingRail.Children.Add(StraitSleepers(this.Length1, StraitOrientation.Center, -this.Angle / 2));
            drawingRail.Children.Add(StraitSleepers(this.Length2, StraitOrientation.Center, +this.Angle / 2));
            drawingRail.Children.Add(StraitRail(this.Length1, StraitOrientation.Center, -this.Angle / 2));
            drawingRail.Children.Add(StraitRail(this.Length2, StraitOrientation.Center, +this.Angle / 2));
            return drawingRail;
        }

        protected override List<TrackDockPoint> CreateDockPoints()
        {
            return new List<TrackDockPoint>
            {
                new TrackDockPoint(0, new Point(-this.Length1 / 2.0, 0.0).Rotate( this.Angle /2),  this.Angle /2 + 135, this.dockType),
                new TrackDockPoint(1, new Point(-this.Length1 / 2.0, 0.0).Rotate(-this.Angle /2), -this.Angle /2 + 135, this.dockType),
                new TrackDockPoint(2, new Point( this.Length1 / 2.0, 0.0).Rotate( this.Angle /2),  this.Angle /2 + 45-90, this.dockType),
                new TrackDockPoint(3, new Point( this.Length1 / 2.0, 0.0).Rotate(-this.Angle /2), -this.Angle /2 + 45-90, this.dockType),
            };
        }
    }
}
