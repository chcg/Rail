using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Model
{
    public class TrackDoubleTurnout : TrackTurnout
    {
        public override void Update(double spacing)
        {
            base.Update(spacing);

            this.geometry = new CombinedGeometry(
                CreateStraitTrackGeometry(this.Length),
                new CombinedGeometry(
                    CreateLeftTurnoutGeometry(this.Length, this.Angle, this.Radius),
                    CreateRightTurnoutGeometry(this.Length, this.Angle, this.Radius)));
        }
    }
}
