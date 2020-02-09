using Rail.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Model
{
    public class RailRampItem : RailItem
    {
        [XmlAttribute("Gradient")]
        public double Gradient { get; set; }

        [XmlAttribute("Height")]
        public double Height { get; set; }

        [XmlIgnore, JsonIgnore]
        public double Length 
        { 
            get { return ((TrackStraight)this.Track).Length;  }
        }

        public override RailBase Clone()
        {
            var clone = new RailRampItem()
            {
                DebugIndex = globalDebugIndex++,
                Position = this.Position,
                Angle = this.Angle,
                Layer = this.Layer,
                //DockPoints = this.DockPoints.Select(d => d.Clone()).ToList(),
                TrackId = this.TrackId,
                Track = this.Track,
                Gradient = this.Gradient,
                Height = this.Height
            };
            clone.DockPoints = this.DockPoints.Select(d => d.Clone(clone)).ToList();
            return clone;
        }

        private static readonly double PIFactor = Math.PI / 180.0;

        public double SetGradient(double value)
        {
            this.Gradient = value;
            this.Height = Math.Sin(value * PIFactor) * this.Length;
            //this.Height = Math.Tan(value * PIFactor) * this.Length;
            return this.Height;
        }

        public double SetGradientInPercent(double value)
        {
            double val = Math.Atan(value / 100.0) / PIFactor;
            return SetGradient(val);
        }

        #region debug

        

        #endregion
    }
}
