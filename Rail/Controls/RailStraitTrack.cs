using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Rail.Controls
{
    public class RailStraitTrack : RailTrack
    {
        public RailStraitTrack(RailMaterial railMaterial, double x, double y, double angle)
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
            this.DockPoints[0].Position = new Point(-this.railMaterial.Length / 2.0, 0.0).Rotate(this.angle).Move(this.Position); 
            this.DockPoints[0].Angle = this.angle + 135;
            this.DockPoints[1].Position = new Point(this.railMaterial.Length / 2.0, 0.0).Rotate(this.angle).Move(this.Position); 
            this.DockPoints[1].Angle = this.angle + 315;
        }

        protected override Geometry CreateGeometry()
        {
            return CreateStraitTrackGeometry(this.railMaterial.Length);
        }

        public override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            drawingContext.DrawText(this.Position, (this.angle + 90.0) % 180.0 - 90.0, this.Text);
        }
    }
}
