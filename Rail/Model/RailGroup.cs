﻿using Rail.Controls;
using Rail.Trigonometry;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;
using Rail.Misc;

namespace Rail.Model
{
    public class RailGroup : RailBase
    {
        protected readonly Pen linePen = new Pen(TrackBrushes.TrackFrame, 2);
        protected readonly Pen dotPen = new Pen(Brushes.White, 2) { DashStyle = DashStyles.Dot };

        public RailGroup()
        { }

        public RailGroup(IEnumerable<RailBase> railItems)
        {
            this.DebugIndex = globalDebugIndex++;

            //var allDockPoints = railItems.SelectMany(r => r.DockPoints).ToList();
            var extDockPoints = railItems.SelectMany(r => r.DockPoints).Where(d => !d.IsDocked || !railItems.Contains(d.DockedWith.RailItem)).ToList(); 

            RailItem firstRailItme = (RailItem)railItems.FirstOrDefault();
            this.Layer = firstRailItme.Layer;
            this.Position = firstRailItme.Position; // set before new RailGroupItem
            this.Angle = 0.0;

            railItems.DebugList($"RailGroup Const original rails");
            this.Rails = railItems.Cast<RailItem>().Select(i => new RailGroupItem(i, this)).ToList();
            this.Rails.ForEach(r => r.IsSelected = false);

            extDockPoints.ForEach(d => d.RailItem = this);

            // remove ext from GroupItems
            this.Rails.ForEach(r => extDockPoints.ForEach(e => r.DockPoints.Remove(e)));
                    
            

            this.DockPoints = extDockPoints;

            //this.Rails.DebugList($"RailGroup DebugIndex={DebugIndex} Rails");
            DebugInfo();

            
            this.IsSelected = true;

            //// find external dock points and group them
            //this.DockPoints = this.Rails.SelectMany(r => r.DockPoints).Where(d => !d.IsDocked).ToList();
            ////var allDockPoints = this.Rails.SelectMany(r => r.DockPoints).ToList();
            //var allDockPointsx = this.Rails.SelectMany(r => r.DockPoints).Where(d => d.IsDocked).Select(d => d.DockedWith.RailItem).ToList();

            //this.DockPoints = this.Rails.SelectMany(r => r.DockPoints).Where(d =>

            //d.IsDocked && this.Rails.Contains(d.DockedWith.RailItem)).ToList();

            
            CreateCombinedGeometries();
        }

        public IEnumerable<RailItem> Resolve()
        {
            return this.Rails.Select(r => new RailItem(r)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        [XmlArray("Rails")]
        [XmlArrayItem("Rail")]
        public List<RailGroupItem> Rails { get; set; }

        [XmlIgnore, JsonIgnore]
        public override List<TrackMaterial> Materials
        {
            get { return this.Rails.SelectMany(r => r.Materials).ToList(); }
        }


        public override RailBase Clone()
        {
            var clone = new RailGroup()
            {
                DebugIndex = globalDebugIndex++,
                Position = this.Position,
                Angle = this.Angle,
                Layer = this.Layer,
                //DockPoints = this.DockPoints.Select(d => d.Clone()).ToList(),
                Rails = this.Rails.Select(r => (RailGroupItem)r.Clone()).ToList()
                 
            };
            clone.DockPoints = this.DockPoints.Select(d => d.Clone(clone)).ToList();
            return clone;
        }

        private Geometry combinedGeometryTracks;
        private Geometry combinedGeometryRail;

        private void CreateCombinedGeometries()
        {
            this.combinedGeometryTracks = this.Rails.Select(r =>
            {
                TransformGroup transformGroup = new TransformGroup();
                transformGroup.Children.Add(new RotateTransform(r.Angle));
                transformGroup.Children.Add(new TranslateTransform(r.Position.X, r.Position.Y));
                
                //Transform transformGroup = r.RailTransform;

                Geometry geometry = r.Track.GeometryTracks.Clone();
                geometry.Transform = transformGroup;
                return geometry;
            }).Aggregate((a, b) => new CombinedGeometry(GeometryCombineMode.Union, a, b));

            this.combinedGeometryRail = this.Rails.Select(r =>
            {
                
                Geometry geometry = r.Track.GeometryRail.Clone();
                geometry.Transform = r.RailTransform;
                return geometry;
            }).Aggregate((a, b) => new CombinedGeometry(GeometryCombineMode.Union, a, b));
        }

        public override void DrawRailItem(DrawingContext drawingContext, RailViewMode viewMode, RailLayer layer)
        {
            drawingContext.PushTransform(this.RailTransform);

            this.Rails.ForEach(r => r.DrawRailItem(drawingContext, viewMode, layer));

            if (this.IsSelected && viewMode < RailViewMode.Terrain)
            {
                /*
                //GeometryGroup geometryGroup = new GeometryGroup();
                //geometryGroup.FillRule = FillRule.EvenOdd;

                var x = this.Rails.Select(r =>
                {
                    Geometry geometrie = viewMode switch { RailViewMode.Tracks => r.Track.GeometryTracks, RailViewMode.Rail => r.Track.GeometryRail, _ => null };

                    TransformGroup transformGroup = new TransformGroup();
                    transformGroup.Children.Add(new RotateTransform(r.Angle));
                    transformGroup.Children.Add(new TranslateTransform(r.Position.X, r.Position.Y));

                    Geometry newGeometry = geometrie.Clone();
                    newGeometry.Transform = transformGroup;
                    //geometryGroup.Children.Add(newGeometry);
                    return newGeometry;
                }).ToArray();

                CombinedGeometry combinedGeometry = new CombinedGeometry(GeometryCombineMode.Union, x[0], x[1]);

                //GeometryGroup combinedGeometry = new GeometryGroup();
                //combinedGeometry.FillRule = FillRule.Nonzero;
                //combinedGeometry.Children.Add(x[0]);
                //combinedGeometry.Children.Add(x[1]);

                drawingContext.DrawDrawing(new GeometryDrawing(null, this.linePen, combinedGeometry));
                drawingContext.DrawDrawing(new GeometryDrawing(null, this.dotPen, combinedGeometry));
                */
                Geometry geometrie = viewMode switch { RailViewMode.Tracks => combinedGeometryTracks, RailViewMode.Rail => combinedGeometryRail, _ => null };
                drawingContext.DrawDrawing(new GeometryDrawing(null, this.linePen, geometrie));
                drawingContext.DrawDrawing(new GeometryDrawing(null, this.dotPen, geometrie));
            }

            drawingContext.Pop();
        }
               
        
        protected override Geometry GetGeometry(RailViewMode viewMode, Transform transform)
        {
            Geometry geometry = viewMode switch { RailViewMode.Tracks => this.combinedGeometryTracks.Clone(), RailViewMode.Rail => this.combinedGeometryRail.Clone(), _ => null };
            geometry.Transform = this.RailTransform;
            return geometry;
        }

        public override RailBase Move(Vector vec)
        {
            base.Move(vec);
            this.Rails.ForEach(r => r.Move(vec));
            return this;
        }

        public override void Rotate(Rotation rotation, Point center)
        {
            base.Rotate(rotation, center);
            this.Rails.ForEach(r => r.Rotate(rotation, center));
        }

        #region Debug

        [Conditional("DEBUG")]
        public new void DebugInfo()
        {
            Debug.WriteLine($"RailGroup DebugIndex={DebugIndex}");
            Debug.Indent();
            this.DockPoints.DebugList();
            this.Rails.DebugList("Children");
            Debug.Unindent();
        }

        #endregion
    }
}
