using Rail.Tracks.Properties;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Tracks
{
    public class TrackTransferTable : TrackBaseSingle
    {
        #region store

        [XmlElement("TransferTableType")]
        public TrackTransferTableType TransferTableType { get; set; }

        [XmlElement("DeckLength")]
        public Guid DeckLengthId { get; set; }

        [XmlElement("ConnectionLength")]
        public Guid ConnectionLengthId { get; set; }

        [XmlElement("ConnectionDistance")]
        public Guid ConnectionDistanceId { get; set; }

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
            this.ConnectionDistance = GetValue(trackType.Lengths, this.ConnectionDistanceId);

            this.Name = $"{Resources.TrackTransferTable}";
            this.Description = $"{this.Article} {Resources.TrackTransferTable}";

            base.Update(trackType);
        }

        protected override Geometry CreateGeometry()
        {
            double width = this.DeckLength + this.ConnectionLength * 2;
            double height = 7 * this.ConnectionDistance;
            return new RectangleGeometry(new Rect(-width / 2, -height / 2, width, height));
        }

        protected override Drawing CreateRailDrawing()
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

        protected override List<TrackDockPoint> CreateDockPoints()
        {
            double width = this.DeckLength + this.ConnectionLength * 2;
            double height = 7 * this.ConnectionDistance;

            return new List<TrackDockPoint>
            {
                new TrackDockPoint(0, new Point(-width / 2.0, 0.0), 135, this.dockType),
                new TrackDockPoint(1, new Point(+width / 2.0, 0.0), 315, this.dockType)
            };
        }

        #endregion
    }
}
