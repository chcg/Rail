using Rail.Model;
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
    public class ItemCurved : ItemBase<TrackCurved>
    {
        public ItemCurved(TrackCurved track, double x, double y, double angle)
            : base(track, x, y, angle)
        {
            Update();
        }

        protected override void Update()
        {
            if (this.DockPoints == null)
            {
                this.DockPoints = new List<DockPoint>(2) { new DockPoint(this), new DockPoint(this) };
            }

            Point circleCenter = this.Position + (Vector)Points.CircleCenter(this.track.Angle / 2 + this.angle, this.track.Radius);

            this.DockPoints[0].Position = new Point(this.track.Radius, 0).Rotate(this.angle).Move(circleCenter);
            this.DockPoints[0].Angle = this.angle + 225.0;
            this.DockPoints[1].Position = new Point(this.track.Radius, 0).Rotate(this.angle + this.track.Angle).Move(circleCenter);
            this.DockPoints[1].Angle = this.angle + this.track.Angle + 45.0;
        }
        
        protected override Geometry CreateGeometry()
        {
            return CreateCurvedTrackGeometry(this.track.Angle, this.track.Radius);
        }

        public override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            drawingContext.DrawText(this.Position, ((90.0 + this.track.Angle / 2 + this.angle) + 90.0) % 180.0 - 90.0, this.Text);
        }
    }
}
