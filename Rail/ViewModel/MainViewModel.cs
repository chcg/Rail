using Rail.Controls;
using Rail.Model;
using Rail.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rail.ViewModel
{
    public class MainViewModel : FileViewModel
    {
        private Project project;
        private double zoomFactor = 1.0;
        private Tracks tracks;

        public MainViewModel()
        {
            this.DefaultFileExt = "*.rail";
            this.FileFilter = "Rail Project|*.rail|All Files|*.*";
            //try
            //{
            //    this.RailPlan = new RailPlanViewModel();
            //}
            //catch (Exception ex)
            //{
            //    Debug.WriteLine(ex.ToString());
            //}
            try
            {
                tracks = Tracks.Load("Tracks.xml");
            }
            catch (Exception ex)
            {

            }
        }

        private RailPlanViewModel railPlan;

        public RailPlanViewModel RailPlan 
        {
            get
            {
                return this.railPlan;
            }
            set
            {
                this.railPlan = value; 
                NotifyPropertyChanged("RailPlan");
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
                this.RailPlan.ZoomFactor = value;
                NotifyPropertyChanged("ZoomFactor"); 
            }
        }

        public override void OnCreate()
        {
            this.FileChanged = false;
            this.project = Project.CreateProject();
            this.RailPlan = new RailPlanViewModel(this.project.TrackDiagram);
            this.FileChanged = true;
            this.FilePath = null;
        }

        public override void OnLoad(string path)
        {
            this.project = Project.Load(path);
            this.RailPlan = new RailPlanViewModel(this.project.TrackDiagram);
            this.FileChanged = true;
        }

        public override void OnStore(string path)
        {
            this.project.TrackDiagram.Tracks = 
                this.RailPlan.RailTracks.Select(t => new Track(t.Id, t.Position.X, t.Position.Y, t.Angle, FindDocks(t))).ToList();
            this.project.Save(path);
            this.FileChanged = true;
        }

        private int[] FindDocks(RailTrack track)
        {
            return track.DockPoints.Select((d, i) => d.Dock == null ? -1 : this.RailPlan.RailTracks.IndexOf(d.Dock.Track)).ToArray();
        }
    }
}
