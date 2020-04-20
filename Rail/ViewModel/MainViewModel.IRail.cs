using Rail.Controls;
using Rail.Misc;
using Rail.Model;
using Rail.Tracks;
using Rail.Tracks.Trigonometry;
using Rail.View;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Rail.ViewModel
{
    public sealed partial class MainViewModel
    {
        public int Width
        {
            get
            {
                return (int)Math.Round(this.railPlan.PlatePoints.Select(p => p.X).Max());
            }
        }

        public int Height
        {
            get
            {
                return (int)Math.Round(this.railPlan.PlatePoints.Select(p => p.Y).Max());
            }
        }

        //private double zoomFactor;
        //public double ZoomFactor
        //{
        //    get
        //    {
        //        return this.zoomFactor;
        //    }
        //    set
        //    {
        //        this.zoomFactor = value;
        //        NotifyPropertyChanged(nameof(ZoomFactor));
        //    }
        //}

        //public double Width { get { return this.railPlan.Width; } }
        //public double Height { get { return this.railPlan.Height; } }

        #region properties

        public IEnumerable<RailBase> Rails
        {
            get
            {
                return this.railPlan.Rails;
            }
        }

        public IEnumerable<RailLayer> Layers
        {
            get
            {
                return this.railPlan.Layers;
            }
        }


        

        


        //private TrackBase selectedTrack;
        //public TrackBase SelectedTrack
        //{
        //    get
        //    {
        //        return this.selectedTrack;
        //    }
        //    set
        //    {
        //        this.selectedTrack = value;
        //        NotifyPropertyChanged(nameof(SelectedTrack));
        //        Invalidate();
        //    }
        //}

        //private TrackType selectedTrackType;
        //public TrackType SelectedTrackType
        //{
        //    get
        //    {
        //        return this.selectedTrackType;
        //    }
        //    set
        //    {
        //        this.selectedTrackType = value;
        //        NotifyPropertyChanged(nameof(SelectedTrackType));
        //        Invalidate();
        //    }
        //}
               

        private List<TrackMaterial> materials;
        public List<TrackMaterial> Materials
        {
            get
            {
                return this.materials;
            }
            set
            {
                this.materials = value;
                NotifyPropertyChanged(nameof(Materials));
            }
        }

        #endregion

        

        public IEnumerable<RailBase> SelectedRampRails
        {
            get
            {
                // check if all items are RailItems
                var selectedRailBase = SelectedRails.ToList();
                if (!selectedRailBase.All(r => r is RailItem))
                {
                    // not all items are RailItems
                    return null;
                }
                var selected = selectedRailBase.Cast<RailItem>().ToList();

                // find singe extern docked RailItem
                var externDocked = selected.SelectMany(r => r.DockPoints).Where(d => d.DockedWith != null && !selected.Contains(d.DockedWith.RailItem)).ToList();
                if (!externDocked.One())
                {
                    // more or less than one external dock
                    return null;
                }
                RailItem item = (RailItem)externDocked.Single().RailItem;

                // sort ramp items


                List<RailItem> rampList = new List<RailItem>();
                while (item != null)
                {
                    rampList.Add(item);
                    item = (RailItem)item.DockPoints.Where(d => d.IsDocked && selected.Contains(d.DockedWith.RailItem) && !rampList.Contains(d.DockedWith.RailItem)).SingleOrDefault()?.DockedWith.RailItem;
                }
                return rampList;
            }
        }


        private void Invalidate()
        {
            this.RailChanged?.Invoke(this, new EventArgs());
        }

        public RailBase FindRailItem(Point point)
        {
            RailBase track = this.railPlan.Rails.Where(t => t.IsInside(point)).FirstOrDefault();
            return track;
        }

        public RailDockPoint FindFreeDockPoint(Point point)
        {
            RailDockPoint dockPoint = this.railPlan.Rails.SelectMany(r => r.DockPoints).Where(d => !d.IsDocked).Where(d => d.IsInside(point)).FirstOrDefault();
            return dockPoint;
        }

        public void InsertRailItem(Point pos)
        {
            InsertRailItem(pos, this.SelectedTrack);
        }

        public void InsertRailItem(Point pos, TrackBase trackBase)
        {
            Debug.WriteLine($"InsertRailItem at ({pos.X},{pos.Y})");

            // insert selected track at mouse position
            this.railPlan.Rails.Add(new RailItem(trackBase, pos, this.InsertLayer.Id));
        }

        public void InsertRailItem(RailDockPoint railDockPoint)
        {
            Debug.WriteLine($"InsertRailItem at DockPoint ({railDockPoint.DebugDockPointIndex},{railDockPoint.DebugDockPointIndex})");

            RailBase railItem = new RailItem(this.SelectedTrack, new Point(0, 0), this.InsertLayer.Id);
            Point pos = ((RailItem)railItem).Track.DockPoints.First().Position;
            //RailDockPoint newRailDockPoint = railItem.DockPoints.First();

            railItem.Move((Vector)railDockPoint.Position + (Vector)pos);

            this.railPlan.Rails.Add(railItem);
            //FindDocking(this.actionTrack, this.dockedTracks);
        }

        public void DeleteRailItem(RailBase railItem)
        {
            // delete all docks of the item
            railItem.DockPoints.Where(dp => dp.IsDocked).ForEach(dp => dp.Undock());
            // remove the item
            this.railPlan.Rails.Remove(railItem);
        }

        public void DeleteSelectedRailItems()
        {
            // delete all docks of the item
            var list = SelectedRails.ToList();
            // remove the item
            list.ForEach(r => DeleteRailItem(r));
        }
        
        public void MoveRailItem(IEnumerable<RailBase> subgraph, Vector move)
        {
            //Debug.WriteLine($"MoveRailItem {railItem.DebugIndex} ({move.X:F2},{move.Y:F2}) with subgraph");

            subgraph.ForEach(t => t.Move(move));
        }

        public void RotateRailItem(IEnumerable<RailBase> subgraph, Point center, Rotation rotation)
        {
            subgraph.ForEach(t => t.Rotate(rotation, center));
        }

        public void BindDockingPoints(RailDockPoint from, RailDockPoint to)
        {
            if (from == null || to == null)
            {
                return;
            }

            RailBinder.Bind(this.railPlan, this.SelectedTrackType, from, to);
            Invalidate();
        }

        public void SwitchRailItemDocking(RailItem railItem)
        {

            // TODO
        }


        public bool FindDocking(RailBase railItem)
        {
            if (this.railPlan.Rails != null)
            {
                var dockPoints = railItem.DockPoints.Where(dp => !dp.IsDocked).ToList();
                var otherTracks =
                    //docked != null ?
                    //this.RailPlan.Rails.Where(t => t != railItem).Where(t => !docked.Contains(t)).ToList() :
                    this.railPlan.Rails.Where(t => t != railItem).ToList();

                //DebugDockPoints(dockPoints);
                //DebugRailItems(otherTracks);
                foreach (var dockPoint in dockPoints)
                {
                    foreach (RailBase t in otherTracks)
                    {
                        foreach (var dp in t.DockPoints.Where(dp => !dp.IsDocked))
                        {
                            //if (Math.Abs(dp.X - dockPoint.X) < dockDistance && Math.Abs(dp.Y - dockPoint.Y) < dockDistance)
                            if (dp.IsInside(dockPoint))
                            {
                                dockPoint.AdjustDock(dp);

                                //this.actionType = RailAction.None;
                                //return t;
                                return true;
                            }
                        }
                    }

                }
            }
            return false;
        }

        public void UpdateMaterials()
        {
            this.Materials = this.railPlan.Rails.SelectMany(r => r.Materials).GroupBy(m => m.Id).Select(g => new TrackMaterial
            {
                Id = g.First().Id,
                Number = g.Select(i => i.Number).Sum(),
                Manufacturer = g.First().Manufacturer,
                Article = g.First().Article,
                Name = g.First().Name
            }).OrderBy(g => g.Article).ToList();
        }

        public Drawing PlateDrawing
        {
            get
            {
                return new GeometryDrawing(TrackBrushes.Plate, plateFramePen, new PathGeometry(new PathFigureCollection
                {
                    new PathFigure(this.railPlan.PlatePoints.FirstOrDefault(), new PathSegmentCollection
                    (
                        this.railPlan.PlatePoints.Skip(1).Select(p => new LineSegment(p, true))
                    ), true)
                }));
            }
        }

        public void ShowMeasure(Point from, Point to)
        {
            MeasureViewModel viewModel = new MeasureViewModel
            {
                Distance = from.Distance(to),
                DistanceX = Math.Abs(from.X - to.X),
                DistanceY = Math.Abs(from.Y - to.Y)
            };
            new MeasureView { DataContext = viewModel }.ShowDialog();
        }
    }
}
