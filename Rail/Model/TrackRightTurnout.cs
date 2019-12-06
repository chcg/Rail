using Rail.Misc;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Rail.Model
{
    public class TrackRightTurnout : TrackTurnout
    {
        public override void Update(double spacing)
        {
            base.Update(spacing);

            this.Geometry = new CombinedGeometry(
                CreateStraitTrackGeometry(this.Length),
                CreateRightTurnoutGeometry(this.Length, this.Angle, this.Radius));

            Point circleCenter = new Point(-this.Length / 2, this.Radius);
            this.DockPoints = new List<TrackDockPoint>
            {
                new TrackDockPoint(-this.Length / 2.0, 0.0, 90 + 45),
                new TrackDockPoint( this.Length / 2.0, 0.0, 180 + 90 + 45),
                new TrackDockPoint(new Point(-this.Length / 2.0, 0).Rotate(this.Angle, circleCenter), this.Angle - 45)
            };
        }
    }
}

