using Rail.Controls;
using Rail.Model;
using Rail.Mvvm;
using Rail.View;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
                string path = System.AppDomain.CurrentDomain.BaseDirectory;
                tracks = Tracks.Load(Path.Combine(path, "Tracks.xml"));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in File Tracks.xml\r\n" + ex.Message);
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
            CreateViewModel vm = new CreateViewModel(tracks);
            CreateView dlg = new CreateView();
            dlg.DataContext = vm;
            dlg.Owner = Application.Current.MainWindow;
            if (dlg.ShowDialog().Value == true)
            {
                this.FileChanged = false;
                this.project = Project.CreateProject();
                this.project.TrackDiagram.Width = vm.Width;
                this.project.TrackDiagram.Height = vm.Width;

                this.RailPlan = new RailPlanViewModel(this.project.TrackDiagram);
                this.FileChanged = true;
                this.FilePath = null;
            }
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
