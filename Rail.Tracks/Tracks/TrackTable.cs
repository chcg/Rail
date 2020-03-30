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
    public class TrackTable : TrackBaseSingle
    {
        public TrackTable()
        {
            this.TableType = TrackTableType.Turntable24;
        }

        #region store

        [XmlElement("TableType")]
        public TrackTableType TableType { get; set; }

        [XmlElement("DeckLength")]
        public Guid DeckLengthId { get; set; }

        [XmlElement("ConnectionLength")]
        public Guid ConnectionLengthId { get; set; }

        [XmlElement("ConnectionDistance")]
        public Guid ConnectionDistanceId { get; set; }


        public bool ShouldSerializeConnectionDistanceId() { return this.ConnectionDistanceId != Guid.Empty; }

        #endregion

        #region intern

        [XmlIgnore, JsonIgnore]
        public double DeckLength { get; set; }

        [XmlIgnore, JsonIgnore]
        public double ConnectionLength { get; set; }

        [XmlIgnore, JsonIgnore]
        public double ConnectionDistance { get; set; }

        #endregion

        #region override

        [XmlIgnore, JsonIgnore]
        public override TrackTypes TrackType { get { return TrackTypes.Table; } }

        /// <summary>
        /// Ramp length
        /// </summary>
        /// <remarks>Turntable can not be used in ramps</remarks>
        [XmlIgnore, JsonIgnore]
        public override double RampLength { get { return 0; } }


        public override void Update(TrackType trackType)
        {
            this.DeckLength = GetValue(trackType.Lengths, this.DeckLengthId);
            this.ConnectionLength = GetValue(trackType.Lengths, this.ConnectionLengthId);
            this.ConnectionDistance = GetValueOrNull(trackType.Lengths, this.ConnectionDistanceId);

            if (this.TableType.HasFlag(TrackTableType.Turntable24))
            {
                this.Name = $"{Resources.TrackTurntable}";
            }
            else if(this.TableType.HasFlag(TrackTableType.Transfer55))
            {
                this.Name = $"{Resources.TrackTransferTable}";
            }
            else if (this.TableType.HasFlag(TrackTableType.Segment320))
            {
                this.Name = $"{Resources.TrackSegmentTurntable}";
            }
                
            this.Description = $"{this.Article} {this.Name}";

            base.Update(trackType);
        }

        protected override Geometry CreateGeometry()
        {
            if (this.TableType.HasFlag(TrackTableType.Turntable24))
            {
                double diameter = this.DeckLength + this.ConnectionLength * 2;
                return new EllipseGeometry(new Point(0, 0), diameter / 2, diameter / 2);
            }
            else if (this.TableType.HasFlag(TrackTableType.Transfer55))
            {
                double width = this.DeckLength + this.ConnectionLength * 2;
                double height = 7 * this.ConnectionDistance;
                return new RectangleGeometry(new Rect(-width / 2, -height / 2, width, height));
            }
            else if (this.TableType.HasFlag(TrackTableType.Segment320))
            {
                return null;
            }
            return null;
        }

        protected override Drawing CreateRailDrawing()
        {
            if (this.TableType.HasFlag(TrackTableType.Turntable24))
            {
                int railNum = this.TableType switch
                {
                    TrackTableType.Turntable24 => 24,
                    TrackTableType.Turntable30 => 30,
                    TrackTableType.Turntable40 => 40,
                    TrackTableType.Turntable48 => 48,
                    _ => 0
                };
                double diameter = this.DeckLength + this.ConnectionLength * 2;
                double angle = 360.0 / railNum;

                DrawingGroup drawingRail = new DrawingGroup();
                // background
                drawingRail.Children.Add(new GeometryDrawing(new SolidColorBrush(Colors.DarkGray), linePen, new EllipseGeometry(new Point(0, 0), diameter / 2, diameter / 2)));
                drawingRail.Children.Add(new GeometryDrawing(new SolidColorBrush(Colors.Gray), linePen, new EllipseGeometry(new Point(0, 0), this.DeckLength / 2, this.DeckLength / 2)));
                if (this.HasBallast)
                {
                    for (int i = 0; i < railNum; i++)
                    {
                        drawingRail.Children.Add(new GeometryDrawing(this.ballastBrush, null,
                            new PathGeometry(new PathFigureCollection
                            {
                        new PathFigure(new Point(-diameter / 2, -this.RailWidth).Rotate(angle * i), new PathSegmentCollection
                        {
                            new LineSegment(new Point(-this.DeckLength / 2, -this.RailWidth).Rotate(angle * i), true),
                            new LineSegment(new Point(-this.DeckLength / 2,  this.RailWidth).Rotate(angle * i), true),
                            new LineSegment(new Point(-diameter / 2,  this.RailWidth).Rotate(angle * i), true),
                            new LineSegment(new Point(-diameter / 2, -this.RailWidth).Rotate(angle * i), true)
                        }, true)
                            })));
                    }
                    drawingRail.Children.Add(new GeometryDrawing(this.ballastBrush, null, new RectangleGeometry(new Rect(-this.DeckLength / 2, -this.RailWidth, this.DeckLength, this.RailWidth * 2))));
                }

                drawingRail.Children.Add(StraitRail(this.DeckLength));

                for (int i = 0; i < railNum; i++)
                {

                    drawingRail.Children.Add(new GeometryDrawing(null, this.railPen, new LineGeometry(new Point(-diameter / 2, -this.RailWidth / 2).Rotate(angle * i), new Point(-this.DeckLength / 2, -this.RailWidth / 2).Rotate(angle * i))));
                    drawingRail.Children.Add(new GeometryDrawing(null, this.railPen, new LineGeometry(new Point(-diameter / 2, +this.RailWidth / 2).Rotate(angle * i), new Point(-this.DeckLength / 2, +this.RailWidth / 2).Rotate(angle * i))));

                    double length1 = diameter / 2 - this.DeckLength / 2;
                    int num1 = (int)Math.Round(length1 / (this.RailWidth / 2));
                    double sleepersDistance1 = length1 / num1;

                    for (int j = 0; j < num1; j++)
                    {
                        drawingRail.Children.Add(new GeometryDrawing(null, this.sleeperPen, new LineGeometry(
                            new Point(-diameter / 2 + sleepersDistance1 / 2 + sleepersDistance1 * j, -this.sleeperWidth / 2).Rotate(angle * i),
                            new Point(-diameter / 2 + sleepersDistance1 / 2 + sleepersDistance1 * j, +this.sleeperWidth / 2).Rotate(angle * i))));
                    }
                }
                return drawingRail;
            }
            else if (this.TableType.HasFlag(TrackTableType.Transfer55))
            {
                double width = this.DeckLength + this.ConnectionLength * 2;
                double height = 7 * this.ConnectionDistance;

                double rim = this.ConnectionLength;

                DrawingGroup drawingRail = new DrawingGroup();
                // background
                drawingRail.Children.Add(new GeometryDrawing(new SolidColorBrush(Colors.DarkGray), linePen, new RectangleGeometry(new Rect(-width / 2, -height / 2, width, height))));
                drawingRail.Children.Add(new GeometryDrawing(new SolidColorBrush(Colors.Gray), linePen, new RectangleGeometry(new Rect(-width / 2, -height / 2, rim, height))));
                drawingRail.Children.Add(new GeometryDrawing(new SolidColorBrush(Colors.Gray), linePen, new RectangleGeometry(new Rect(width / 2 - rim, -height / 2, rim, height))));
                if (this.HasBallast)
                {
                    //drawingRail.Children.Add(StraitBallast(this.Length, StraitOrientation.Center, 0, null));
                }
                //drawingRail.Children.Add(StraitRail(this.Length));
                return drawingRail;
            }
            else if (this.TableType.HasFlag(TrackTableType.Segment320))
            {
                return null;
            }
            return null;
        }

        protected override List<TrackDockPoint> CreateDockPoints()
        {
            if (this.TableType.HasFlag(TrackTableType.Turntable24))
            {
                int railNum = this.TableType switch
                {
                    TrackTableType.Turntable24 => 24,
                    TrackTableType.Turntable30 => 30,
                    TrackTableType.Turntable40 => 40,
                    TrackTableType.Turntable48 => 48,
                    _ => 0
                };

                double angle = 360.0 / railNum;
                double diameter = this.DeckLength + this.ConnectionLength * 2;

                var dockPoints = new List<TrackDockPoint>();
                for (int i = 0; i < railNum; i++)
                {
                    Point point = new Point(0, diameter / 2).Rotate(angle * i);
                    dockPoints.Add(new TrackDockPoint(i, point, angle * i + 45, this.dockType));
                }
                return dockPoints;
            }
            else if (this.TableType.HasFlag(TrackTableType.Transfer55))
            {
                double width = this.DeckLength + this.ConnectionLength * 2;
                double height = 7 * this.ConnectionDistance;

                return new List<TrackDockPoint>
                {
                    new TrackDockPoint(0, new Point(-width / 2.0, 0.0), 135, this.dockType),
                    new TrackDockPoint(1, new Point(+width / 2.0, 0.0), 315, this.dockType)
                };
            }
            else if (this.TableType.HasFlag(TrackTableType.Segment320))
            {
                return new List<TrackDockPoint>();
            }
            return null;
        }

        #endregion
    }
}
