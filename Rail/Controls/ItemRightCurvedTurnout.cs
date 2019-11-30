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
    class ItemRightCurvedTurnout: ItemBase<TrackRightCurvedTurnout>
    {

        public ItemRightCurvedTurnout(TrackRightCurvedTurnout track, double x, double y, double angle)
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
            this.DockPoints[0].Angle = this.angle + 135;
            this.DockPoints[1].Position = new Point(this.track.Length / 2.0, 0.0).Rotate(this.angle).Move(this.Position);
            this.DockPoints[1].Angle = this.angle + 315;
            this.DockPoints[2].Position = new Point(this.track.Length / 2.0, 0.0).Rotate(this.angle).Move(this.Position);
            this.DockPoints[2].Angle = this.angle + 315;
        }

        protected override Geometry CreateGeometry()
        {
            return CreateStraitTrackGeometry(10);
            //double baseAngle = 270.0;
            //Point circleCenter = new Point(
            //    -Cos(baseAngle + this.railMaterial.Angle / 2) * this.railMaterial.Radius,
            //    -Sin(baseAngle + this.railMaterial.Angle / 2) * this.railMaterial.Radius);

            //return new PathGeometry(new PathFigureCollection
            //{
            //    new PathFigure(new Point(-this.railMaterial.Length / 2.0, -railWidth), new PathSegmentCollection
            //    {
            //        new LineSegment(new Point( this.railMaterial.Length / 2.0, -railWidth), true),
            //        new LineSegment(new Point( this.railMaterial.Length / 2.0,  railWidth), true),
            //        new LineSegment(new Point(-this.railMaterial.Length / 2.0,  railWidth), true),
            //        new LineSegment(new Point(-this.railMaterial.Length / 2.0, -railWidth), true)
            //    }, true),
            //    new PathFigure(new Point(circleCenter.X + Cos(baseAngle) * (this.railMaterial.Radius - railWidth), circleCenter.Y + Sin(baseAngle) * (this.railMaterial.Radius - railWidth)), new PathSegmentCollection
            //    {
            //        new LineSegment(new Point(circleCenter.X + Cos(baseAngle                          ) * (this.railMaterial.Radius + railWidth), circleCenter.Y + Sin(baseAngle                          ) * (this.railMaterial.Radius + railWidth)), true),
            //        new ArcSegment (new Point(circleCenter.X + Cos(baseAngle + this.railMaterial.Angle) * (this.railMaterial.Radius + railWidth), circleCenter.Y + Sin(baseAngle + this.railMaterial.Angle) * (this.railMaterial.Radius + railWidth)), new Size(this.railMaterial.Radius + railWidth, this.railMaterial.Radius + railWidth), this.railMaterial.Angle, false, SweepDirection.Clockwise, true),
                   
            //        new LineSegment(new Point(circleCenter.X + Cos(baseAngle + this.railMaterial.Angle) * (this.railMaterial.Radius - railWidth), circleCenter.Y + Sin(baseAngle + this.railMaterial.Angle) * (this.railMaterial.Radius - railWidth)), true),
            //        new ArcSegment (new Point(circleCenter.X + Cos(baseAngle                          ) * (this.railMaterial.Radius - railWidth), circleCenter.Y + Sin(baseAngle                          ) * (this.railMaterial.Radius - railWidth)), new Size(this.railMaterial.Radius - railWidth, this.railMaterial.Radius - railWidth), this.railMaterial.Angle, false, SweepDirection.Counterclockwise, true)
            //    }, true)
            //});
        }

        public override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            drawingContext.DrawText(this.Position, this.angle, this.Text);
        }
    }
}
