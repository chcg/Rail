using Rail.Controls;
using Rail.Misc;
using Rail.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Model
{

    public class TrackStraight : TrackBase
    {

        [XmlAttribute("Length")]
        public string LengthNameOrValue { get; set; }

        [XmlIgnore]
        public double Length { get; set; }

        [XmlIgnore]
        public string LengthName { get; set; }

        [XmlIgnore]
        public override string Name 
        { 
            get 
            { 
                return $"{Resources.TrackStraight} {LengthName} {Length} mm"; 
            } 
        }

        [XmlIgnore]
        public override string Description
        {
            get
            {
                return $"{this.Article} {Resources.TrackStraight} {LengthName} {Length} mm";
            }
        }

        public override void Update(TrackType trackType)
        {
            this.Length = GetValue(trackType.Lengths, this.LengthNameOrValue);
            this.LengthName = GetName(this.LengthNameOrValue);
            base.Update(trackType);
        }

        protected override Geometry CreateGeometry(double spacing)
        {
            return StraitGeometry(this.Length, StraitOrientation.Center, spacing); 
        }

        protected override Drawing CreateRailDrawing(bool isSelected)
        {
            DrawingGroup drawingRail = new DrawingGroup();
            if (this.ViewType.HasFlag(TrackViewType.Ballast))
            {
                drawingRail.Children.Add(StraitBallast(this.Length, StraitOrientation.Center, 0, null));
            }
            drawingRail.Children.Add(StraitSleepers(isSelected, this.Length));
            drawingRail.Children.Add(StraitRail(isSelected, this.Length));
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
