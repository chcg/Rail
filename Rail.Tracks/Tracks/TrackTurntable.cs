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
    public class TrackTurntable : TrackBaseSingle
    {
        #region store

        [XmlElement("RailNum")]
        public TrackTurntableRailNum RailNum { get; set; }

        [XmlElement("Diameter")]
        public Guid DiameterId { get; set; }

        [XmlElement("DeckLength")]
        public Guid DeckLengthId { get; set; }

        #endregion

        #region intern

        [XmlIgnore, JsonIgnore]
        public double Diameter { get; set; }

        [XmlIgnore, JsonIgnore]
        public double DeckLength { get; set; }

        #endregion

        #region override

        /// <summary>
        /// Ramp length
        /// </summary>
        /// <remarks>Turntable can not be used in ramps</remarks>
        [XmlIgnore, JsonIgnore]
        public override double RampLength { get { return 0; } }

        public override void Update(TrackType trackType)
        {
            this.Diameter = GetValue(trackType.Lengths, this.DiameterId);
            this.DeckLength = GetValue(trackType.Lengths, this.DeckLengthId);

            this.Name = $"{Resources.TrackTurntable}";
            this.Description = $"{this.Article} {Resources.TrackTurntable}";

            base.Update(trackType);
        }

        protected override Geometry CreateGeometry()
        {
            return new EllipseGeometry(new Point(0, 0), this.Diameter / 2, this.Diameter / 2);
        }

        protected override Drawing CreateRailDrawing()
        {
            int railNum = this.RailNum switch
            {
                TrackTurntableRailNum.TurntableNum_24 => 24,
                TrackTurntableRailNum.TurntableNum_30 => 30,
                TrackTurntableRailNum.TurntableNum_40 => 40,
                TrackTurntableRailNum.TurntableNum_48 => 48,
                _ => 0
            };
            double angle = 360 / railNum;

            DrawingGroup drawingRail = new DrawingGroup();
            // background
            drawingRail.Children.Add(new GeometryDrawing(new SolidColorBrush(Colors.DarkGray), linePen, new EllipseGeometry(new Point(0, 0), this.Diameter / 2 , this.Diameter / 2)));
            drawingRail.Children.Add(new GeometryDrawing(new SolidColorBrush(Colors.Gray), linePen, new EllipseGeometry(new Point(0, 0), this.DeckLength / 2, this.DeckLength / 2)));
            if (this.HasBallast)
            {
                for (int i = 0; i < railNum; i++)
                {
                    drawingRail.Children.Add(new GeometryDrawing(this.ballastBrush, null,
                        new PathGeometry(new PathFigureCollection
                        {
                        new PathFigure(new Point(-this.Diameter / 2, -this.RailWidth).Rotate(angle * i), new PathSegmentCollection
                        {
                            new LineSegment(new Point(-this.DeckLength / 2, -this.RailWidth).Rotate(angle * i), true),
                            new LineSegment(new Point(-this.DeckLength / 2,  this.RailWidth).Rotate(angle * i), true),
                            new LineSegment(new Point(-this.Diameter / 2,  this.RailWidth).Rotate(angle * i), true),
                            new LineSegment(new Point(-this.Diameter / 2, -this.RailWidth).Rotate(angle * i), true)
                        }, true)
                        })));
                }
                drawingRail.Children.Add(new GeometryDrawing(this.ballastBrush, null, new RectangleGeometry(new Rect(-this.DeckLength / 2, -this.RailWidth, this.DeckLength, this.RailWidth * 2))));
            }
            
            drawingRail.Children.Add(StraitRail(this.DeckLength));

            for (int i = 0; i < railNum; i++)
            {

                drawingRail.Children.Add(new GeometryDrawing(null, this.railPen, new LineGeometry(new Point(-this.Diameter / 2, -this.RailWidth / 2).Rotate(angle * i), new Point(-this.DeckLength / 2, -this.RailWidth / 2).Rotate(angle * i)))); 
                drawingRail.Children.Add(new GeometryDrawing(null, this.railPen, new LineGeometry(new Point(-this.Diameter / 2, +this.RailWidth / 2).Rotate(angle * i), new Point(-this.DeckLength / 2, +this.RailWidth / 2).Rotate(angle * i))));

                double length1 = this.Diameter / 2 - this.DeckLength / 2;
                int num1 = (int)Math.Round(length1 / (this.RailWidth / 2));
                double sleepersDistance1 = length1 / num1;

                for (int j = 0; j < num1; j++)
                {
                    drawingRail.Children.Add(new GeometryDrawing(null, this.sleeperPen, new LineGeometry(
                        new Point(-this.Diameter / 2 + sleepersDistance1 / 2 + sleepersDistance1 * j, -this.sleeperWidth / 2).Rotate(angle * i),
                        new Point(-this.Diameter / 2 + sleepersDistance1 / 2 + sleepersDistance1 * j, +this.sleeperWidth / 2).Rotate(angle * i))));
                }
            }
            return drawingRail;
        }

        protected override List<TrackDockPoint> CreateDockPoints()
        {
            int railNum = this.RailNum switch
            {
                TrackTurntableRailNum.TurntableNum_24 => 24,
                TrackTurntableRailNum.TurntableNum_30 => 30,
                TrackTurntableRailNum.TurntableNum_40 => 40,
                TrackTurntableRailNum.TurntableNum_48 => 48,
                _ => 0
            };

            double angle = 360 / railNum;

            var dockPoints = new List<TrackDockPoint>();
            for (int i = 0; i < railNum; i++)
            {
                Point point = new Point(0, this.Diameter / 2).Rotate(angle * i);
                dockPoints.Add(new TrackDockPoint(i, point, angle * i + 45, this.dockType));
            }
            return dockPoints;
        }

        #endregion
    }
}
