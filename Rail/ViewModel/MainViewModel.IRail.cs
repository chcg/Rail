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


        private RailSelectedMode selectedMode;
        public RailSelectedMode SelectedMode
        {
            get
            {
                return this.selectedMode;
            }
            set
            {
                this.selectedMode = value;
                NotifyPropertyChanged(nameof(SelectedMode));
            }
        }

        private RailBase selectedRail;
        public RailBase SelectedRail
        {
            get
            {
                return this.selectedRail;
            }
            set
            {
                this.selectedRail = value;
                NotifyPropertyChanged(nameof(SelectedRail));
            }
        }


        private TrackBase selectedTrack;
        public TrackBase SelectedTrack
        {
            get
            {
                return this.selectedTrack;
            }
            set
            {
                this.selectedTrack = value;
                NotifyPropertyChanged(nameof(SelectedTrack));
                Invalidate();
            }
        }

        private TrackType selectedTrackType;
        public TrackType SelectedTrackType
        {
            get
            {
                return this.selectedTrackType;
            }
            set
            {
                this.selectedTrackType = value;
                NotifyPropertyChanged(nameof(SelectedTrackType));
                Invalidate();
            }
        }

        private RailLayer insertLayer;
        public RailLayer InsertLayer
        {
            get
            {
                return this.insertLayer;
            }
            set
            {
                this.insertLayer = value;
                NotifyPropertyChanged(nameof(InsertLayer));
                Invalidate();
            }
        }

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

        public List<RailBase> SelectedRails
        {
            get
            {
                return this.railPlan.Rails.Where(r => r.IsSelected).ToList();
            }
        }

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

        public void SelectRailItem(RailBase railItem, bool addSelect)
        {
            if (addSelect)
            {
                railItem.IsSelected = !railItem.IsSelected;
            }
            else
            {
                this.railPlan.Rails.ForEach(r => r.IsSelected = false);
                railItem.IsSelected = true;
            }
        }

        public void SelectRectange(Rect rec, bool addSelect)
        {
            if (!addSelect)
            {
                this.railPlan.Rails.ForEach(r => r.IsSelected = false);
            }
            this.railPlan.Rails.Where(r => r.IsInside(rec)).ForEach(r => r.IsSelected = true);
        }

        public void UnselectAllRailItems()
        {
            this.railPlan.Rails.ForEach(r => r.IsSelected = false);
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


        public void FindDocking(RailBase railItem)
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
                            }
                        }
                    }

                }
            }
            //return null;
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

        #region history

        private int historyIndex = -1;
        private readonly List<RailPlan> history = new List<RailPlan>();

        protected override void OnUndo()
        {
            if (OnCanUndo())
            {
                this.railPlan = history[--historyIndex];
            }
        }

        protected override bool OnCanUndo()
        {
            return historyIndex > 0;
        }

        protected override void OnRedo()
        {
            if (OnCanRedo())
            {
                this.railPlan = history[++historyIndex];
            }
        }

        protected override bool OnCanRedo()
        {
            return historyIndex >= 0 && historyIndex < history.Count - 1;
        }

        /// <summary>
        /// call always befor manipulating the RailPlan
        /// </summary>
        [Conditional("USERHISTORY")]
        public void StoreToHistory()
        {
            if (historyIndex >= 0 && historyIndex < history.Count - 1)
            {
                this.history.RemoveRange(historyIndex + 1, history.Count - 1 - historyIndex);
            }
            this.history.Add(this.railPlan.Clone());
            historyIndex = this.history.Count - 1;
        }

        #endregion

        #region copy & past

        private List<RailBase> copy = null;
        private int copyFactor;

        public void Clone()
        {
            // clone tree
            this.railPlan = this.railPlan.Clone();
            // clone dock point links
            RailDockPoint.CloneDockPointLinks();
        }

        private void OnCopy()
        {
            if (OnCanCopy())
            {
                this.copy = SelectedRails.ToList();
                this.copyFactor = 1;
            }
        }

        private bool OnCanCopy()
        {
            return this.railPlan.Rails.Any(r => r.IsSelected);
        }

        private void OnCut()
        {
            if (OnCanCut())
            {
                this.copy = SelectedRails.ToList();
                this.copy.ForEach(r => DeleteRailItem(r));
                this.copyFactor = 1;
                StoreToHistory();
                this.Invalidate();
            }
        }
        private bool OnCanCut()
        {
            return this.railPlan.Rails.Any(r => r.IsSelected);
        }

        private void OnPaste()
        {
            if (OnCanPaste())
            {
                this.railPlan.Rails.AddRange(copy.Select(r => r.Clone().Move(new Vector(copyPositionDrift * this.copyFactor, copyPositionDrift * this.copyFactor))));
                this.copyFactor++;
                // clone dock point links
                RailDockPoint.CloneDockPointLinks();

                StoreToHistory();
                this.Invalidate();
            }
        }

        private bool OnCanPaste()
        {
            return copy != null;
        }

        private void OnDelete()
        {
            if (OnCanDelete())
            {
                var list = SelectedRails.ToList();
                list.ForEach(r => this.DeleteRailItem(r));
                StoreToHistory();
                this.Invalidate();
            }
        }

        private bool OnCanDelete()
        {
            return this.railPlan.Rails.Any(r => r.IsSelected);
        }

        private void OnDuplicate()
        {
            if (OnCanDuplicate())
            {
                var selectedRails = SelectedRails;
                this.railPlan.Rails.AddRange(selectedRails.Select(r => r.Clone().Move(new Vector(copyPositionDrift * this.copyFactor, copyPositionDrift * this.copyFactor))));
                this.copyFactor = 1;
                // clone dock point links
                RailDockPoint.CloneDockPointLinks();

                StoreToHistory();
                this.Invalidate();
            }
        }

        private bool OnCanDuplicate()
        {
            return this.railPlan.Rails.Any(r => r.IsSelected);
        }

        private void OnSelectAll()
        {
            this.railPlan.Rails.ForEach(r => r.IsSelected = true);
            this.Invalidate();
        }

        private bool OnCanSelectAll()
        {
            return this.railPlan.Rails.Count() > 0;
        }

        //private void OnUnselectAll()
        //{
        //    this.RailPlan.Rails.ForEach(r => r.IsSelected = false);
        //    this.Invalidate();
        //}

        #endregion

        #region anchor


        private bool OnCanAnchor()
        {
            return this.SelectedMode == RailSelectedMode.Single && !this.selectedRail.IsAnchored;
        }

        private void OnAnchor()
        {
            if (OnCanAnchor())
            {
                this.SelectedRail.IsAnchored = true;
                this.Invalidate();
            }
        }

        private bool OnCanUnanchor()
        {
            return this.SelectedMode == RailSelectedMode.Single && this.selectedRail.IsAnchored;
        }

        private void OnUnanchor()
        {
            if (OnCanUnanchor())
            {
                this.selectedRail.IsAnchored = false;
                this.Invalidate();
            }
        }

        public bool IsAnchored(List<RailBase> rails)
        {
            return rails.Any(r => r.IsAnchored);
        }

        #endregion

        #region group

        private bool OnCanCreateGroup()
        {
            return
                this.SelectedMode == RailSelectedMode.Multi &&
                // cannot group other group
                this.SelectedRails.All(r => r is RailItem) &&
                // all must have the same layer
                this.SelectedRails.Select(r => r.Layer).Distinct().Count() == 1;
        }

        private void OnCreateGroup()
        {
            if (OnCanCreateGroup())
            {
                // take all selected rails
                var selectedRails = this.SelectedRails.ToArray();

                // create rail group
                this.railPlan.Rails.Add(new RailGroup(selectedRails));

                // remove from Rails
                selectedRails.ForEach(r => this.railPlan.Rails.Remove(r));

                Invalidate();
            }
        }

        private void OnResolveGroup()
        {
            if (OnCanResolveGroup() && this.SelectedRail is RailGroup railGroup)
            {
                this.railPlan.Rails.AddRange(railGroup.Resolve());
                this.railPlan.Rails.Remove(railGroup);
                Invalidate();
            }
        }

        private bool OnCanResolveGroup()
        {
            return this.SelectedMode == RailSelectedMode.Single && this.selectedRail is RailGroup;
        }

        private void OnSaveAsGroup()
        {
        }

        private bool OnCanSaveAsGroup()
        {
            return this.SelectedMode == RailSelectedMode.Single && this.selectedRail is RailGroup;
        }

        #endregion

        #region ramp

        private bool OnCanCreateRamp()
        {
            return
                this.SelectedMode == RailSelectedMode.Multi &&
                SelectedRails.All(r => r is RailItem) &&
                SelectedRails.Count() >= 8;
        }

        private void OnCreateRamp()
        {
            // take all selected rails
            var selectedRails = SelectedRails;

            RailRamp railRamp = new RailRamp(selectedRails);

            RampView rampView = new RampView { DataContext = new RampViewModel { RailRamp = railRamp, LayerHight = railRamp.LayerHeigh } };
            if (rampView.ShowDialog().Value)
            {
                // remove from Rails
                selectedRails.ForEach(r => this.railPlan.Rails.Remove(r));
                // add rail group
                this.railPlan.Rails.Add(railRamp);
                Invalidate();
            }
        }

        private void OnDeleteRamp()
        {
            if (OnCanDeleteRamp() && this.selectedRail is RailRamp railRamp)
            {
                this.railPlan.Rails.AddRange(railRamp.Resolve());
                this.railPlan.Rails.Remove(railRamp);
                Invalidate();
            }
        }

        private bool OnCanDeleteRamp()
        {
            return this.SelectedMode == RailSelectedMode.Single && this.selectedRail is RailRamp;
        }

        public void OnEditRamp()
        {
            if (OnCanEditRamp())
            {
                RampView rampView = new RampView { DataContext = new RampViewModel { RailRamp = (RailRamp)this.selectedRail } };
                if (rampView.ShowDialog().Value)
                {
                }
            }
        }

        private bool OnCanEditRamp()
        {
            return this.SelectedMode == RailSelectedMode.Single && this.selectedRail is RailRamp;
        }
        #endregion

        #region helix

        private void OnCreateHelix()
        { }
        private bool OnCanCreateHelix()
        {
            return this.SelectedMode == RailSelectedMode.Multi &&
                    SelectedRails.All(r => r is RailItem ri && ri.Track is TrackCurved) &&
                    SelectedRails.Count() >= 16;
        }

        private void OnDeleteHelix()
        { }

        private bool OnCanDeleteHelix()
        {
            return this.SelectedMode == RailSelectedMode.Single && this.selectedRail is RailHelix;
        }

        private void OnEditHelix()
        { }

        private bool OnCanEditHelix()
        {
            return this.SelectedMode == RailSelectedMode.Single && this.selectedRail is RailHelix;
        }

        #endregion

        #region Measure

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

        #endregion
    }
}
