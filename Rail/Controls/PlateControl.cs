using System;
using System.Collections.Generic;
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
    public class PlateControl : Control
    {
        static PlateControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PlateControl), new FrameworkPropertyMetadata(typeof(PlateControl)));
        }

        public PlateControl()
        {
        }

        public static readonly DependencyProperty Width1Property =
            DependencyProperty.Register("Width1", typeof(int), typeof(PlateControl),
                new FrameworkPropertyMetadata(1500, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnChanged)));

        public int Width1
        {
            get
            {
                return (int)GetValue(Width1Property);
            }
            set
            {
                SetValue(Width1Property, value);
            }
        }

        public static readonly DependencyProperty Width2Property =
            DependencyProperty.Register("Width2", typeof(int), typeof(PlateControl),
                new FrameworkPropertyMetadata(3000, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnChanged)));

        public int Width2
        {
            get
            {
                return (int)GetValue(Width2Property);
            }
            set
            {
                SetValue(Width2Property, value);
            }
        }

        public static readonly DependencyProperty Width3Property =
            DependencyProperty.Register("Width3", typeof(int), typeof(PlateControl),
                new FrameworkPropertyMetadata(1500, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnChanged)));

        public int Width3
        {
            get
            {
                return (int)GetValue(Width3Property);
            }
            set
            {
                SetValue(Width3Property, value);
            }
        }

        public static readonly DependencyProperty Height1Property =
            DependencyProperty.Register("Height1", typeof(int), typeof(PlateControl),
                new FrameworkPropertyMetadata(3000, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnChanged)));

        public int Height1
        {
            get
            {
                return (int)GetValue(Height1Property);
            }
            set
            {
                SetValue(Height1Property, value);
            }
        }


        public static readonly DependencyProperty Height2Property =
            DependencyProperty.Register("Height2", typeof(int), typeof(PlateControl),
                new FrameworkPropertyMetadata(1000, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnChanged)));

        public int Height2
        {
            get
            {
                return (int)GetValue(Height2Property);
            }
            set
            {
                SetValue(Height2Property, value);
            }
        }

        public static readonly DependencyProperty Height3Property =
            DependencyProperty.Register("Height3", typeof(int), typeof(PlateControl),
                new FrameworkPropertyMetadata(3000, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnChanged)));

        public int Height3
        {
            get
            {
                return (int)GetValue(Height3Property);
            }
            set
            {
                SetValue(Height3Property, value);
            }
        }

        private static void OnChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            PlateControl plateControl = (PlateControl)o;
            plateControl.InvalidateVisual();
        }
        
        private readonly Pen blackPen = new Pen(Brushes.Black, 1);
        private readonly Brush plateBrush = new SolidColorBrush(Colors.Green); 
        private readonly double margin = 8;

        protected override void OnRender(DrawingContext drawingContext)
        {
            //base.OnRender(drawingContext);

            double w = this.ActualWidth - 2 * margin;
            double h = this.ActualHeight - 2 * margin;
            double factor = Math.Min(w / (this.Width1 + this.Width2 + this.Width3), h / Math.Max(this.Height1, Math.Max(this.Height2, this.Height3)));

            double x = (this.ActualWidth  - (this.Width1 + this.Width2 + this.Width3) * factor) / 2;
            double y = (this.ActualHeight - Math.Max(this.Height1, Math.Max(this.Height2, this.Height3)) * factor) / 2;

            drawingContext.DrawGeometry(plateBrush, blackPen, new PathGeometry(new PathFigureCollection
            {
                new PathFigure(new Point(x, y), new PathSegmentCollection
                {
                    new LineSegment(new Point(x,                          y + (this.Height1) * factor), true),
                    new LineSegment(new Point(x + (this.Width1) * factor, y + (this.Height1) * factor), true),
                    new LineSegment(new Point(x + (this.Width1) * factor, y + (this.Height2) * factor), true),
                    new LineSegment(new Point(x + (this.Width1 + this.Width2) * factor, y + (this.Height2) * factor), true),
                    new LineSegment(new Point(x + (this.Width1 + this.Width2) * factor, y + (this.Height3) * factor), true),
                    new LineSegment(new Point(x + (this.Width1 + this.Width2 + this.Width3) * factor, y + (this.Height3) * factor), true),
                    new LineSegment(new Point(x + (this.Width1 + this.Width2 + this.Width3) * factor, y), true),
                }, true)
            }));
        }
    }
}
