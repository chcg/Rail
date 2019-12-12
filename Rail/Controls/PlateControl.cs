using Rail.Misc;
using Rail.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Rail.Controls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Rail.Controls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Rail.Controls;assembly=Rail.Controls"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:PlateControl/>
    ///
    /// </summary>
    public class PlateControl : ItemsControl
    {
        static PlateControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PlateControl), new FrameworkPropertyMetadata(typeof(PlateControl)));
        }

        public PlateControl()
        { }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
            case NotifyCollectionChangedAction.Add:
                e.NewItems.Cast<PointViewModel>().ForEach(p => p.OnChange += OnItemValueChanged);
                break;
            case NotifyCollectionChangedAction.Remove:
                e.OldItems.Cast<PointViewModel>().ForEach(p => p.OnChange -= OnItemValueChanged);
                break;
            }
            InvalidateVisual();
        }

        protected void OnItemValueChanged(object sender, EventArgs ev)
        {
            InvalidateVisual();
        }

        private readonly Pen blackPen = new Pen(Brushes.Black, 1);
        private readonly Brush plateBrush = new SolidColorBrush(Colors.Green); 
        private readonly double margin = 8;

        protected override void OnRender(DrawingContext drawingContext)
        {
            //base.OnRender(drawingContext);
            var list = this.ItemsSource.Cast<PointViewModel>();

            double w = this.ActualWidth - 2 * margin;
            double h = this.ActualHeight - 2 * margin;
            uint pw = list.Select(p => p.X).Max();
            uint ph = list.Select(p => p.Y).Max();
            
            double factor = Math.Min(w / pw, h / ph);
            Point start = new Point(margin, margin);

            TransformGroup transformGroup = new TransformGroup();
            transformGroup.Children.Add(new ScaleTransform(factor, factor));
            transformGroup.Children.Add(new TranslateTransform(margin, margin));
            drawingContext.PushTransform(transformGroup);
            drawingContext.DrawGeometry(plateBrush, blackPen, new PathGeometry(new PathFigureCollection
            {
                new PathFigure(start + list.FirstOrDefault(), new PathSegmentCollection
                (
                    list.Skip(1).Select(p => new LineSegment(start + p, true))
                ), true)
            }));
            drawingContext.Pop();
        }
    }
}
