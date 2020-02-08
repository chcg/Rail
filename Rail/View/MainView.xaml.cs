using Rail.Mvvm;
using System.Windows;

namespace Rail.View
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : FileRibbonView
    {
        public MainView()
        {
            InitializeComponent();
        }

        public void OnRefreshRailPlanControl(object sender, RoutedEventArgs e)
        {
            this.railPlanControl.InvalidateVisual();
        }
    }
}
