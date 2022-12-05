using Rail.Mvvm;
using Rail.Plugin;
using System;
using System.Windows;
using System.Windows.Controls.Ribbon;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Rangefinder
{
    public class RangefinderPlugin : IRailPlugin
    {
        private Window mainWindow;
        private IRailPlan railPlan;

        public DelegateCommand TestCommand { get; private set; }

        public RangefinderPlugin()
        {
            this.TestCommand = new DelegateCommand(OnTest); 
        }

        public void Register(Window mainWindow, IRailPlan railPlan, RibbonTab pluginRibbonTab)
        {
            this.mainWindow = mainWindow;
            this.railPlan = railPlan;            

            RibbonGroup ribbonGroup = new RibbonGroup();
            ribbonGroup.Header = "Rangefinder";
            ribbonGroup.Items.Add(new RibbonButton
            { 
                Label = "Test",
                Command = TestCommand,
                LargeImageSource = new BitmapImage(new Uri("Resource/icon.ico", UriKind.Relative))
            });
            pluginRibbonTab.Items.Add(ribbonGroup);
        }

        public void KeyEvent(RailKeyEventArgs args)
        {
            
        }

        public void MouseEvent(RailMouseEventArgs args)
        {
            if (args.ChangedButton == MouseButton.Left && 
                args.ButtonState == MouseButtonState.Pressed && 
                args.ClickCount == 1 && 
                args.Modifiers.HasFlag(ModifierKeys.Alt))
            {
                args.State = RailState.Connect;
            }

            if (args.ChangedButton == MouseButton.Left &&
                args.ButtonState == MouseButtonState.Released &&
                args.State == RailState.Connect)
            {
                args.State = RailState.None;
                MessageBox.Show(this.mainWindow, "Distance", "Distance", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void OnTest()
        {

        }
    }
}
