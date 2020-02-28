using Rail.Tracks;
using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace Rail.TrackEditor.Controls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Rail.TrackEditor.Controls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Rail.TrackEditor.Controls;assembly=Rail.TrackEditor.Controls"
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
    ///     <MyNamespace:TrackControl/>
    ///
    /// </summary>
    public class TrackControl : Control
    {
        static TrackControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TrackControl), new FrameworkPropertyMetadata(typeof(TrackControl)));
        }

        public static readonly DependencyProperty ShowRailProperty =
            DependencyProperty.Register("ShowRail", typeof(bool), typeof(TrackControl),
                new FrameworkPropertyMetadata(false));

        public bool ShowRail
        {
            get
            {
                return (bool)GetValue(ShowRailProperty);
            }
            set
            {
                SetValue(ShowRailProperty, value);
            }
        }

        public static readonly DependencyProperty TrackProperty =
            DependencyProperty.Register("Track", typeof(TrackBase), typeof(TrackControl),
                new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnTrackChanged)));

        public TrackBase Track
        {
            get
            {
                return (TrackBase)GetValue(TrackProperty);
            }
            set
            {
                SetValue(TrackProperty, value);
            }
        }

        private static void OnTrackChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            TrackControl trackControl = (TrackControl)o;
            trackControl.InvalidateVisual();
        }

        private readonly Pen blackPen = new Pen(Brushes.Black, 1);

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            drawingContext.DrawRectangle(this.Background, null, new Rect(0, 0, this.ActualWidth, this.ActualHeight));

            if (this.Track == null)
            {
                return;
            }
            
            Geometry geometry = this.Track.TrackGeometry.Clone();
            Size size = geometry.Bounds.Size;
            double zoom = Math.Min(20.0 / this.Track.RailWidth, Math.Min((this.ActualHeight - 10) / size.Height, (this.ActualWidth - 10) / size.Width));
            double my = (geometry.Bounds.Bottom + geometry.Bounds.Top) / 2;

            if (ShowRail)
            {
                // set zero point to center
                drawingContext.PushTransform(new TransformGroup()
                {
                    Children = new TransformCollection
                    {
                        new ScaleTransform(zoom, zoom),
                        new TranslateTransform(this.ActualWidth / 2, this.ActualHeight / 2 - my)
                    }
                });

                this.Track.RenderRail(drawingContext);

                drawingContext.Pop();
            }
            else
            {
                // set zero point to center
                drawingContext.PushTransform(new TranslateTransform(this.ActualWidth / 2, this.ActualHeight / 2 - my));


                geometry.Transform = new ScaleTransform(zoom, zoom);
                drawingContext.DrawGeometry(null, blackPen, geometry);

                drawingContext.Pop();
            }
        }
    }
}
