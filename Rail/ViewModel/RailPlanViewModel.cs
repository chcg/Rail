using Rail.Controls;
using Rail.Model;
using Rail.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Rail.ViewModel
{
    public class RailPlanViewModel : BaseViewModel
    {
        private TrackDiagram trackDiagram;

        private double zoomFactor = 1.0;
        private double groundWidth = 2000.0;
        private double groundHeight = 1000.0;

        public const double R1 = 360.0;
        public const double R2 = 437.5;
        public const double R3 = 515.0;
        public const double R4 = 579.3;
        public const double R5 = 643.6;
        public const double R6 = 1114.6;

        public DelegateCommand<RailTrack> AddTrackCommand { get; private set; }

        public RailPlanViewModel(TrackDiagram trackDiagram)
        {
            this.AddTrackCommand = new DelegateCommand<RailTrack>(OnAddTrack);
            
            this.RailMaterials = new List<RailMaterial>
            {
                // strait tracks
                new RailMaterial() { Id = 064, Type = RailType.Strait, Length =  64.3, Decription = "Strait 64.3 mm" },
                new RailMaterial() { Id = 071, Type = RailType.Strait, Length =  70.8, Decription = "Strait 70.8 mm" },
                new RailMaterial() { Id = 077, Type = RailType.Strait, Length =  77.5, Decription = "Strait 77.5 mm" },
                new RailMaterial() { Id = 094, Type = RailType.Strait, Length =  94.2, Decription = "Strait 94.2 mm" },
                new RailMaterial() { Id = 172, Type = RailType.Strait, Length = 171.7, Decription = "Strait 171.7 mm" },
                new RailMaterial() { Id = 188, Type = RailType.Strait, Length = 188.3, Decription = "Strait 188.3 mm" },
                new RailMaterial() { Id = 229, Type = RailType.Strait, Length = 229.3, Decription = "Strait 229.3 mm" },
                new RailMaterial() { Id = 236, Type = RailType.Strait, Length = 236.1, Decription = "Strait 236.1 mm" },
                new RailMaterial() { Id = 360, Type = RailType.Strait, Length = 360.0, Decription = "Strait 360.0 mm" },

                // curved tracks
                new RailMaterial() { Id = 107, Type = RailType.Curved, Radius = R1, Angle =  7.5, Decription = "Curved R1 360.0 mm 7.5°" },
                new RailMaterial() { Id = 115, Type = RailType.Curved, Radius = R1, Angle = 15.0, Decription = "Curved R1 360.0 mm 15.0°" },
                new RailMaterial() { Id = 130, Type = RailType.Curved, Radius = R1, Angle = 30.0, Decription = "Curved R1 360.0 mm 30.0°" },
                new RailMaterial() { Id = 206, Type = RailType.Curved, Radius = R2, Angle =  5.7, Decription = "Curved R2 437.5 mm 5.7°" },
                new RailMaterial() { Id = 207, Type = RailType.Curved, Radius = R2, Angle =  7.5, Decription = "Curved R2 437.5 mm 7.5°" },
                new RailMaterial() { Id = 215, Type = RailType.Curved, Radius = R2, Angle = 15.0, Decription = "Curved R2 437.5 mm 15.0°" },

                new RailMaterial() { Id = 224, Type = RailType.Curved, Radius = R2, Angle = 24.3, Decription = "Curved R3 515.0 mm 24.3°" },
                
                new RailMaterial() { Id = 230, Type = RailType.Curved, Radius = R2, Angle = 30.0, Decription = "Curved R3 515.0 mm 30.0°" },
                new RailMaterial() { Id = 315, Type = RailType.Curved, Radius = R3, Angle = 15.0, Decription = "Curved R3 515.0 mm 15.0°" },
                new RailMaterial() { Id = 330, Type = RailType.Curved, Radius = R3, Angle = 30.0, Decription = "Curved R3 515.0 mm 30.0°" },
                new RailMaterial() { Id = 430, Type = RailType.Curved, Radius = R4, Angle = 30.0, Decription = "Curved R4 579.3 mm 30.0°" },
                new RailMaterial() { Id = 530, Type = RailType.Curved, Radius = R5, Angle = 30.0, Decription = "Curved R5 643.6 mm 30.0°" },
                new RailMaterial() { Id = 912, Type = RailType.Curved, Radius = R6, Angle = 12.1, Decription = "Curved R6 1114.6 mm 12.1°" },

                // turnout tracks
                new RailMaterial() { Id = 611, Type = RailType.LeftTurnout, Length = 188.3, Radius = R2, Angle = 24.3, Decription = "Left Turnout" },
                new RailMaterial() { Id = 612, Type = RailType.RightTurnout, Length = 188.3, Radius = R2, Angle = 24.3, Decription = "Right Turnout" },
                new RailMaterial() { Id = 711, Type = RailType.LeftTurnout, Length = 236.1, Radius = R6, Angle = 12.1, Decription = "Left Wide Radius Turnout" },
                new RailMaterial() { Id = 712, Type = RailType.RightTurnout, Length = 236.1, Radius = R6, Angle = 12.1, Decription = "Right Wide Radius Turnout" },                

                // curved Turnout
                new RailMaterial() { Id = 671, Type = RailType.LeftCurvedTurnout, Radius = R1, Angle = 30.0, Distance = 77.5, Decription = "Left Curved Turnout" },
                //new RailMaterial() { Id = 672, Type = RailType.RightCurvedTurnout, Radius = R1, Angle = 30.0, Distance = 77.5, Decription = "Right Curved Turnout" },
                //new RailMaterial() { Id = 771, Type = RailType.LeftCurvedTurnout, Radius = R3 , Angle = 30.0, Distance = 64.0, Decription = "Left Wide Radius Curved Turnout" },
                //new RailMaterial() { Id = 772, Type = RailType.RightCurvedTurnout, Radius = R3 , Angle = 30.0, Distance = 64.0, Decription = "Right Wide Radius Curved Turnout" },
                
                // double slip switch
                new RailMaterial() { Id = 624, Type = RailType.DoubleSlipSwitch, Length = 188.3, Angle = 24.3, Decription = "Double Slip Switch" },

                // double turnout
                new RailMaterial() { Id = 630, Type = RailType.DoubleTurnout, Length = 188.3, Radius = 437.5, Angle = 24.3, Decription = "Double Turnout" },
                
                // crossing
                new RailMaterial() { Id = 640, Type = RailType.Crossing, Length = 188.3, Angle = 24.3, Decription = "Crossing 24.3°" },
                new RailMaterial() { Id = 649, Type = RailType.Crossing, Length = 103.3, Angle = 48.6, Decription = "Crossing 48.6°" },
                new RailMaterial() { Id = 740, Type = RailType.Crossing, Length = 236.1, Angle = 12.1, Decription = "Crossing 12.1°" },

                // bumper
                new RailMaterial { Id = 977, Type = RailType.Bumper, Length = 77.5 + 5.0, Decription = "Bumper" }
            };

            this.RailMaterialTracks = new ObservableCollection<RailTrack>(this.RailMaterials.Select(r => RailTrack.Create(r, 0, 0, RailTypePresentAngle(r))));

            this.RailTracks = new ObservableCollection<RailTrack>(
                trackDiagram.Tracks.Select(t => RailTrack.Create(this.RailMaterials.Where(m => m.Id == t.Id).FirstOrDefault(), t.X, t.Y, t.Angle)));
            //for (int i = 0; i < trackDiagram.Tracks.Count; i++ )
            //{
            //    RailTracks[] docks = trackDiagram.Tracks[i].Docks.Select(d => d < 0 ? null : this.RailTracks[d]).ToArray();
            //    this.RailTracks[i].DockPoints
            //}

            this.trackDiagram = trackDiagram;
            this.GroundWidth = trackDiagram.Width;
            this.GroundHeight = trackDiagram.Height;
            this.ZoomFactor = TrackDiagram.Zoom;
        }

        private double RailTypePresentAngle(RailMaterial railMaterial)
        {
            double angle = 0.0;
            switch (railMaterial.Type)
            {
            case RailType.Curved: angle = 90.0 - railMaterial.Angle / 2.0; break;
            case RailType.DoubleSlipSwitch:
            case RailType.Crossing: angle = -railMaterial.Angle / 2.0; break;
            case RailType.LeftCurvedTurnout: angle = 90.0 - railMaterial.Angle; break;
            }
            return angle;
        }

        public List<RailMaterial> RailMaterials { get; private set; }

        public ObservableCollection<RailTrack> RailMaterialTracks { get; private set; }

        public ObservableCollection<RailTrack> RailTracks { get; private set; }

       // public ObservableCollection<RailConnection> RailConnections { get; private set; }

        
        public TrackDiagram TrackDiagram
        {
            get
            {
                return this.trackDiagram;
            }
            set
            {
                this.trackDiagram = value;
                NotifyPropertyChanged("TrackDiagram");
            }
        }

        public double ZoomFactor
        {
            get
            {
                return this.zoomFactor;
            }
            set
            {
                this.zoomFactor = value;
                NotifyPropertyChanged("ZoomFactor");
            }
        }

        public double GroundWidth
        {
            get
            {
                return this.groundWidth;
            }
            set
            {
                this.groundWidth = value;
                NotifyPropertyChanged("GroundWidth");
            }
        }

        public double GroundHeight
        {
            get
            {
                return this.groundHeight;
            }
            set
            {
                this.groundHeight = value;
                NotifyPropertyChanged("GroundHeight");
            }
        }

        private void OnAddTrack(RailTrack track)
        {
            track.Position = new Point(this.GroundWidth / 2.0, this.groundHeight / 2.0);
            this.RailTracks.Add(track);
        }
    }
}
