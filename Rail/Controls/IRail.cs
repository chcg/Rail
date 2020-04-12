using Rail.Model;
using Rail.Tracks;
using Rail.Tracks.Trigonometry;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Rail.Controls
{
    public interface IRail
    {
        event EventHandler RailChanged;

        int Width { get; }
        int Height { get; }

        Drawing PlateDrawing { get; }

        IEnumerable<RailLayer> Layers { get; }
        IEnumerable<RailBase> Rails { get; }

        // actions

        RailBase FindRailItem(Point point);
        RailDockPoint FindFreeDockPoint(Point point);

        void InsertRailItem(Point pos);
        void InsertRailItem(Point pos, TrackBase trackBase);
        void InsertRailItem(RailDockPoint railDockPoint);

        void DeleteRailItem(RailBase railItem);
        void DeleteSelectedRailItems();

        void SelectRailItem(RailBase railItem, bool addSelect);
        void SelectRectange(Rect rec, bool addSelect);
        void UnselectAllRailItems();

        void MoveRailItem(IEnumerable<RailBase> subgraph, Vector move);
        void RotateRailItem(IEnumerable<RailBase> subgraph, Point center, Rotation rotation);
        void BindDockingPoints(RailDockPoint from, RailDockPoint to);
        void SwitchRailItemDocking(RailItem railItem);

        void FindDocking(RailBase railItem);

        void ShowMeasure(Point from, Point to);

        bool IsAnchored(List<RailBase> rails);

        void OnEditRamp();
    }
}
