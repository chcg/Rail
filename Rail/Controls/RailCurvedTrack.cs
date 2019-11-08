using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Rail.Controls
{
    public class RailCurvedTrack : RailTrack
    {
        public RailCurvedTrack(RailMaterial railMaterial, double x, double y, double angle)
            : base(railMaterial, x, y, angle)
        {
            Update();
        }

        protected override void Update()
        {
            if (this.DockPoints == null)
            {
                this.DockPoints = new List<DockPoint>(2) { new DockPoint(this), new DockPoint(this) };
            }

            Point circleCenter = this.Position + (Vector)Points.CircleCenter(this.railMaterial.Angle / 2 + this.angle, this.railMaterial.Radius);

            this.DockPoints[0].Position = new Point(this.railMaterial.Radius, 0).Rotate(this.angle).Move(circleCenter);
            this.DockPoints[0].Angle = this.angle + 225.0;
            this.DockPoints[1].Position = new Point(this.railMaterial.Radius, 0).Rotate(this.angle + this.railMaterial.Angle).Move(circleCenter);
            this.DockPoints[1].Angle = this.angle + this.railMaterial.Angle + 45.0;
        }
        
        protected override Geometry CreateGeometry()
        {
            return CreateCurvedTrackGeometry(this.railMaterial.Angle, this.railMaterial.Radius);
        }

        public override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            drawingContext.DrawText(this.Position, ((90.0 + this.railMaterial.Angle / 2 + this.angle) + 90.0) % 180.0 - 90.0, this.Text);
        }
    }
}
