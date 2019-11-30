using Rail.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Rail.Controls
{
    public class ItemLeftTurnout : ItemBase<TrackLeftTurnout>
    {

        public ItemLeftTurnout(TrackLeftTurnout track, double x, double y, double angle)
            : base(track, x, y, angle)
        {
            Update();
        }

        protected override void Update()
        {
            if (this.DockPoints == null)
            {
                this.DockPoints = new List<DockPoint>(3) { new DockPoint(this), new DockPoint(this), new DockPoint(this) };
            }
            this.DockPoints[0].Position = new Point(-this.track.Length / 2.0, 0.0).Rotate(this.angle).Move(this.Position);
            this.DockPoints[0].Angle = this.angle + 90 + 45;
            this.DockPoints[1].Position = new Point(this.track.Length / 2.0, 0.0).Rotate(this.angle).Move(this.Position);
            this.DockPoints[1].Angle = this.angle + 180 + 90 + 45;

            this.DockPoints[2].Position = new Point(-this.track.Length / 2.0, 0).Rotate(-this.track.Angle, new Point(-this.track.Length / 2.0, -this.track.Radius))
                .Rotate(this.angle).Move(this.Position);
            this.DockPoints[2].Angle = this.angle + 180 + 90 + 45 - this.track.Angle;
        }

        protected override Geometry CreateGeometry()
        {
            return new CombinedGeometry(
                CreateStraitTrackGeometry(this.track.Length),
                CreateLeftTurnoutGeometry(-this.track.Length / 2.0, 0.0, this.track.Angle, this.track.Radius));
        }

        public override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            drawingContext.DrawText(this.Position, (this.angle + 90.0) % 180.0 - 90.0, this.Text);
        }
    }
}
