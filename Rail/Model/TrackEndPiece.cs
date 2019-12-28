using Rail.Properties;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Model
{
    public class TrackEndPiece : TrackBase
    {
        [XmlIgnore]
        public override string Name
        {
            get
            {
                return $"{Resources.TrackEndPiece}";
            }
        }

        protected override Geometry CreateGeometry(double spacing)
        {
            double length = this.RailSpacing;

            return StraitGeometry(length, StraitOrientation.Left, spacing);
        }

        protected override Drawing CreateRailDrawing(bool isSelected)
        {
            double length = this.RailSpacing;

            DrawingGroup drawingRail = new DrawingGroup();
            if (this.Ballast)
            {
                drawingRail.Children.Add(StraitBallast(length, StraitOrientation.Left, 0, null));
            }
            drawingRail.Children.Add(StraitRail(isSelected, length / 2, StraitOrientation.Left, 0, null));
            return drawingRail;
        }

        protected override List<TrackDockPoint> CreateDockPoints()
        {
            return new List<TrackDockPoint>
            {
                new TrackDockPoint(0, new Point(this.RailSpacing / 2.0, 0.0), 315, this.dockType)
            };
        }
    }
}
