using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Rail.Controls
{
    public class RailMaterialBar : ListViewItem //ItemsControl // Control
    {
        public double size = 100.0;
        public double zoom = 0.5;

        static RailMaterialBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RailMaterialBar), new FrameworkPropertyMetadata(typeof(RailMaterialBar)));
        }

        public RailMaterialBar()
        { }

        #region Tracks

        public static readonly DependencyProperty TracksProperty =
            DependencyProperty.Register("Tracks", typeof(ObservableCollection<ItemBase>), typeof(RailMaterialBar),
                new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnTracksChanged)));

        public ObservableCollection<ItemBase> Tracks
        {
            get
            {
                return (ObservableCollection<ItemBase>)GetValue(TracksProperty);
            }
            set
            {
                SetValue(TracksProperty, value);
            }
        }

        private static void OnTracksChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RailMaterialBar railMaterialBar = (RailMaterialBar)o;
            railMaterialBar.Height = railMaterialBar.Tracks.Count * railMaterialBar.size;
            railMaterialBar.InvalidateVisual();
        }

        #endregion

        #region Command

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(RailMaterialBar),
                new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnCommandChanged)));

        public ICommand Command
        {
            get
            {
                return (ICommand)GetValue(CommandProperty);
            }
            set
            {
                SetValue(CommandProperty, value);
            }
        }

        private static void OnCommandChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RailMaterialBar railMaterialBar = (RailMaterialBar)o;
            //railMaterialBar.Height = railMaterialBar.Tracks.Count * railMaterialBar.size;
            //railMaterialBar.InvalidateVisual();
        }

        #endregion
        protected override void OnRender(DrawingContext drawingContext)
        {
            drawingContext.DrawRectangle(this.Background, null, new Rect(0, 0, this.ActualWidth, this.ActualHeight));

            drawingContext.PushTransform(new ScaleTransform(zoom, zoom));

            // draw tracks
            if (this.Tracks != null)
            {
                double y = size / 2;
                foreach (ItemBase track in this.Tracks)
                {
                    if (track != null)
                    {
                        track.Position = new Point(this.ActualWidth / 2.0 / zoom, y);
                        track.OnRender(drawingContext);
                        y += size;
                    }
                }
            }

            drawingContext.Pop();

            base.OnRender(drawingContext);
        }

        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            int index = (int)(e.GetPosition(this).Scale(1.0 / this.zoom).Y / size);

            if (this.Command != null && this.Command.CanExecute(null))
            {
//                this.Command.Execute(this.Tracks[index].Clone());
            }
            base.OnMouseDoubleClick(e);
        }
    }
}
