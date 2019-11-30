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
    public class ItemBumper: ItemBase<TrackBumper>
    {
        public ItemBumper(TrackBumper track, double x, double y, double angle)
            : base(track, x, y, angle)
        {
            Update();
        }

        protected override void Update()
        {
            if (this.DockPoints == null)
            {
                this.DockPoints = new List<DockPoint>(1) { new DockPoint(this) };
            }
            this.DockPoints[0].Position = new Point(-this.track.Length / 2.0, 0.0).Rotate(this.angle).Move(this.Position); 
            this.DockPoints[0].Angle = this.angle + 135;
        }

        protected override Geometry CreateGeometry()
        {
            return new PathGeometry(new PathFigureCollection
            {
                new PathFigure(new Point(-this.track.Length / 2.0, -railWidth), new PathSegmentCollection
                {
                    new LineSegment(new Point( this.track.Length / 2.0, -railWidth), true),
                    new LineSegment(new Point( this.track.Length / 2.0,  railWidth), true),
                    new LineSegment(new Point(-this.track.Length / 2.0,  railWidth), true),
                    new LineSegment(new Point(-this.track.Length / 2.0, -railWidth), true)
                }, true),
                new PathFigure(new Point(this.track.Length / 2.0 - railWidth, -railWidth / 2.0), new PathSegmentCollection
                {
                    new LineSegment(new Point(this.track.Length / 2.0 - railWidth, railWidth / 2.0), true),
                }, true)
            });
        }

        public override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            drawingContext.DrawText(this.Position, (this.angle + 90.0) % 180.0 - 90.0, this.Text);
        }
    }
}
