using Rail.Properties;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Model
{
    public class TrackEndPiece : TrackBaseSingle
    {
        [XmlIgnore]
        public override string Name
        {
            get
            {
                return $"{Resources.TrackEndPiece}";
            }
        }

        [XmlIgnore]
        public override string Description
        {
            get
            {
                return $"{this.Article} {Resources.TrackEndPiece}";
            }
        }

        protected override Geometry CreateGeometry(double spacing)
        {
            double length = this.RailSpacing;

            return StraitGeometry(length, StraitOrientation.Left, spacing);
        }

        protected override Drawing CreateRailDrawing()
        {
            double length = this.RailSpacing;

            DrawingGroup drawingRail = new DrawingGroup();
            if (this.ViewType.HasFlag(TrackViewType.Ballast))
            {
                drawingRail.Children.Add(StraitBallast(length, StraitOrientation.Left, 0, null));
            }
            drawingRail.Children.Add(StraitSleepers(length / 2, StraitOrientation.Left, 0, null));
            drawingRail.Children.Add(StraitRail(length / 2, StraitOrientation.Left, 0, null));
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
