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
    public class ItemLeftCurvedTurnout : ItemBase<TrackLeftCurvedTurnout>
    {

        public ItemLeftCurvedTurnout(TrackLeftCurvedTurnout track, double x, double y, double angle)
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
            
            //this.DockPoints[0].Position = new Point(-this.railMaterial.Length / 2.0, 0.0).Rotate(this.angle).Move(this.Position);
            //this.DockPoints[0].Angle = this.angle + 135;
            //this.DockPoints[1].Position = new Point(this.railMaterial.Length / 2.0, 0.0).Rotate(this.angle).Move(this.Position);
            //this.DockPoints[1].Angle = this.angle + 315;
            //this.DockPoints[2].Position = new Point(this.railMaterial.Length / 2.0, 0.0).Rotate(this.angle).Move(this.Position);
            //this.DockPoints[2].Angle = this.angle + 315;
        }

        protected override Geometry CreateGeometry()
        {
            Point circleCenter = this.Position + (Vector)Points.CircleCenter(this.track.Angle / 2 + this.angle, this.track.Radius);
            Point splitPoint = new Point(this.track.Radius, 0).Rotate(this.angle + this.track.Angle).Move(circleCenter);

            //this.DockPoints[0].Position = new Point(this.railMaterial.Radius, 0).Rotate(this.angle).Move(circleCenter);
            //this.DockPoints[0].Angle = this.angle + 225.0;
            //this.DockPoints[1].Position = new Point(this.railMaterial.Radius, 0).Rotate(this.angle + this.railMaterial.Angle).Move(circleCenter);
            //this.DockPoints[1].Angle = this.angle + this.railMaterial.Angle + 45.0;

            //return CreateStraitTrackGeometry(200);

            //this.DockPoints[0].Angle = this.angle + 225.0;

            //Point p = new Point(this.railMaterial.Radius, 0).Rotate(this.railMaterial.Angle);
            //p = p + new Vector(-350, -100);

            //Point circleCenter = this.Position + (Vector)Points.CircleCenter(this.railMaterial.Angle / 2 + this.angle, this.railMaterial.Radius);

            //Point p = new Point(this.railMaterial.Radius, 0).Move(circleCenter);
            Point p = new Point(this.track.Radius, 0).Rotate(this.track.Angle).Move(circleCenter);
            
            double a = this.track.Angle + 45.0;
            

            return new CombinedGeometry(GeometryCombineMode.Xor,
                CreateCurvedTrackGeometry(this.track.Angle, this.track.Radius),
                new CombinedGeometry(GeometryCombineMode.Xor,
                    new EllipseGeometry(p, 8, 8),
                    new EllipseGeometry(p.Move(this.track.Radius - 0 /*this.railMaterial.Distance*/, 0).Rotate(a, p), 8, 8))
                    );

                //CreateLeftTurnoutGeometryx(p.X, p.Y, this.railMaterial.Angle, this.railMaterial.Radius - this.railMaterial.Distance));
            //    CreateLeftCurvedTurnoutGeometry(this.railMaterial.Angle, this.railMaterial.Radius, this.railMaterial.Distance)
               //CreateCurvedTrackGeometry(this.railMaterial.Angle, this.railMaterial.Radius)
            //);

            //Point circleCenter = Points.CircleCenter(this.railMaterial.Angle / 2, this.railMaterial.Radius);

            //return new PathGeometry(new PathFigureCollection
            //{
            //    new PathFigure(circleCenter + (Vector)Points.Rotate(0.0, this.railMaterial.Radius - railWidth), new PathSegmentCollection
            //    {
            //        new LineSegment(circleCenter + (Vector)Points.Rotate(0.0                    , this.railMaterial.Radius + railWidth), true),
            //        new ArcSegment (circleCenter + (Vector)Points.Rotate(this.railMaterial.Angle, this.railMaterial.Radius + railWidth), new Size(this.railMaterial.Radius + railWidth, this.railMaterial.Radius + railWidth), this.railMaterial.Angle, false, SweepDirection.Clockwise, true),

            //        new LineSegment(circleCenter + (Vector)Points.Rotate(this.railMaterial.Angle, this.railMaterial.Radius - railWidth), true),
            //        new ArcSegment (circleCenter + (Vector)Points.Rotate(0.0                    , this.railMaterial.Radius - railWidth), new Size(this.railMaterial.Radius - railWidth, this.railMaterial.Radius - railWidth), this.railMaterial.Angle, false, SweepDirection.Counterclockwise, true)
            //    }, true)
            //});
        }

        protected Geometry CreateLeftTurnoutGeometryx(double x, double y, double angle, double radius)
        {
            Size innerSize = new Size(radius - railWidth, radius - railWidth);
            Size outerSize = new Size(radius + railWidth, radius + railWidth);
            Point circleCenter = new Point(x, y - radius);

            return new PathGeometry(new PathFigureCollection
            {
                new PathFigure(new Point(x, y - railWidth), new PathSegmentCollection
                {
                    new LineSegment(new Point(x, y + railWidth), true),
                    new ArcSegment (new Point(x, y + railWidth).Rotate(-angle, circleCenter), innerSize, angle, false, SweepDirection.Counterclockwise, true),

                    new LineSegment(new Point(x, y - railWidth).Rotate(-angle, circleCenter), true),                 
                    new ArcSegment (new Point(x, y - railWidth), outerSize, angle, false, SweepDirection.Clockwise, true),
                }, true)
            });
        }

        protected Geometry CreateLeftCurvedTurnoutGeometry(double angle, double outerRistance, double innerRadius)
        {
            Point outerCircleCenter = this.Position + (Vector)Points.CircleCenter(angle / 2, outerRistance);
            Point splitPoint = new Point(outerRistance, 0).Rotate(angle).Move(outerCircleCenter);

            Size innerSize = new Size(innerRadius - railWidth, innerRadius - railWidth);
            Size outerSize = new Size(innerRadius + railWidth, innerRadius + railWidth);
            //Point circleCenter = innerRadius;
            return null;
        }

        public override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            drawingContext.DrawText(this.Position, this.angle, this.Text);
        }
    }
}
