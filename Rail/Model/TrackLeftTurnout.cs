using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Rail.Model
{
    public class TrackLeftTurnout : TrackTurnout
    {
        public override void Update(double spacing)
        {
            base.Update(spacing);

            this.geometry = new CombinedGeometry(
                CreateStraitTrackGeometry(this.Length),
                CreateLeftTurnoutGeometry(this.Length, this.Angle, this.Radius));
        }
    }
}
