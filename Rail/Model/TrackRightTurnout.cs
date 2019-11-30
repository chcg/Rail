using System.Windows.Media;

namespace Rail.Model
{
    public class TrackRightTurnout : TrackTurnout
    {
        public override void Update(double spacing)
        {
            base.Update(spacing);

            this.geometry = new CombinedGeometry(
                CreateStraitTrackGeometry(this.Length),
                CreateRightTurnoutGeometry(this.Length, this.Angle, this.Radius));
        }
    }
}
