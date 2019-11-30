using System.Collections.Generic;
using System.Windows.Media;

namespace Rail.Model
{
    public class TrackDoubleTurnout : TrackTurnout
    {
        public override void Update(double spacing)
        {
            base.Update(spacing);

            this.Geometry = new CombinedGeometry(
                CreateStraitTrackGeometry(this.Length),
                new CombinedGeometry(
                    CreateLeftTurnoutGeometry(this.Length, this.Angle, this.Radius),
                    CreateRightTurnoutGeometry(this.Length, this.Angle, this.Radius)));

            this.DockPoints = new List<TrackDockPoint> 
            { 
                new TrackDockPoint(this, -this.Length / 2.0, 0.0, 135), 
                new TrackDockPoint(this,  this.Length / 2.0, 0.0, 315), 
                new TrackDockPoint(this, -this.Length / 2.0, 0, 315 - this.Angle), 
                new TrackDockPoint(this, -this.Length / 2.0, 0, 315 + 45 - this.Angle) 
            };
        }
    }
}
