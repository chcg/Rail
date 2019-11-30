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
    public class ItemCrossing : ItemBase<TrackCrossing>
    {
        public ItemCrossing(TrackCrossing track, double x, double y, double angle)
            : base(track, x, y, angle)
        {
            Update();
        }

        protected override void Update()
        {
            if (this.DockPoints == null)
            {
                this.DockPoints = new List<DockPoint>(4) { new DockPoint(this), new DockPoint(this), new DockPoint(this), new DockPoint(this) };
            }
            this.DockPoints[0].Position = new Point(-this.track.Length1 / 2.0, 0.0).Rotate(this.angle).Move(this.Position);  
            this.DockPoints[0].Angle = this.angle + 135;
            this.DockPoints[1].Position = new Point(this.track.Length1 / 2.0, 0.0).Rotate(this.angle).Move(this.Position);  
            this.DockPoints[1].Angle = this.angle + 315;
            this.DockPoints[2].Position = new Point(-this.track.Length1 / 2.0, 0.0).Rotate(this.angle + this.track.Angle).Move(this.Position); 
            this.DockPoints[2].Angle = this.angle + 135 + this.track.Angle;
            this.DockPoints[3].Position = new Point(this.track.Length1 / 2.0, 0.0).Rotate(this.angle + this.track.Angle).Move(this.Position);
            this.DockPoints[3].Angle = this.angle + 315 + this.track.Angle;
        }

        protected override Geometry CreateGeometry()
        {
            return new CombinedGeometry(

            new PathGeometry(new PathFigureCollection
            {
                new PathFigure(new Point(-this.track.Length1 / 2.0, -railWidth), new PathSegmentCollection
                {
                    new LineSegment(new Point( this.track.Length1 / 2.0, -railWidth), true),
                    new LineSegment(new Point( this.track.Length1 / 2.0,  railWidth), true),
                    new LineSegment(new Point(-this.track.Length1 / 2.0,  railWidth), true),
                    new LineSegment(new Point(-this.track.Length1 / 2.0, -railWidth), true)
                }, true)
            }),
            new PathGeometry(new PathFigureCollection
            {
                new PathFigure(new Point(-this.track.Length1 / 2.0, -railWidth).Rotate(this.track.Angle), new PathSegmentCollection
                {
                    new LineSegment(new Point( this.track.Length1 / 2.0, -railWidth).Rotate(this.track.Angle), true),
                    new LineSegment(new Point( this.track.Length1 / 2.0,  railWidth).Rotate(this.track.Angle), true),
                    new LineSegment(new Point(-this.track.Length1 / 2.0,  railWidth).Rotate(this.track.Angle), true),
                    new LineSegment(new Point(-this.track.Length1 / 2.0, -railWidth).Rotate(this.track.Angle), true)
                }, true)

            }));
        }

        public override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            drawingContext.DrawText(this.Position, (this.angle + 90.0) % 180.0 - 90.0, this.Text);
        }
    }
}
