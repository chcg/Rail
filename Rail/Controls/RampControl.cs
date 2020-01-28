using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
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
using System.Linq;
using Rail.Model;
using Rail.Trigonometry;
using System.Globalization;

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
    ///     <MyNamespace:RampControl/>
    ///
    /// </summary>
    public class RampControl : Control
    {
        static RampControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RampControl), new FrameworkPropertyMetadata(typeof(RampControl)));
        }

        private readonly Pen blackPen = new Pen(Brushes.Black, 1);
        private readonly Brush whiteBrush = new SolidColorBrush(Colors.White);
        private readonly Brush blackBrush = new SolidColorBrush(Colors.Black);

        #region ramp property

        public static readonly DependencyProperty RampProperty =
            DependencyProperty.Register("Ramp", typeof(RailRamp), typeof(RampControl),
                new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnRampChanged)));
        
        public RailRamp Ramp
        {
            get
            {
                return (RailRamp)GetValue(RampProperty);
            }
            set
            {
                SetValue(RampProperty, value);
            }
        }

        private static void OnRampChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RampControl rampControl = (RampControl)o;
            rampControl.InvalidateVisual();
        }

        #endregion
        protected override void OnRender(DrawingContext drawingContext)
        {
            drawingContext.DrawRectangle(whiteBrush, null, new Rect(0, 0, this.ActualWidth, this.ActualHeight));

            if (this.Ramp == null)
            {
                return;
            }

            double width = this.Ramp.Rails.Sum(r => r.Length);
            double height = this.Ramp.LayerHeigh;

            var transform = new TransformGroup();
            transform.Children.Add(new ScaleTransform(this.ActualWidth / width, this.ActualHeight / height));
            //transform.Children.Add(new TranslateTransform(0, height));
            drawingContext.PushTransform(transform);


            //drawingContext.DrawRectangle(null, blackPen, new Rect(10, 10, width-20, height-20));
            Point start = new Point(0, height);
            foreach (var item in this.Ramp.Rails)
            {
                var p = new Point(item.Length, 0);
                p = p.Rotate(-item.Gradient);

                drawingContext.DrawLine(blackPen, start, start + (Vector)p);

                double perc = Gradient.AngleToPercent(item.Gradient);

                Point pos = start.Y > height / 2 ? start + new Vector(item.Length / 2, -28) : start + new Vector(item.Length / 2, 0);
                drawingContext.DrawText(new FormattedText($"{perc:F2}", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Verdana"), 28, blackBrush, 1.25), pos);



                start += new Vector(item.Length, p.Y);

                drawingContext.DrawEllipse(null, blackPen, start, 5, 5);
            }


            drawingContext.Pop();
        }
    }
}
