using System.Windows;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shell;

namespace Rail.Mvvm
{
    public class AppRibbonView : RibbonWindow
    {
        public AppRibbonView()
        {
            this.ResizeMode = ResizeMode.CanResizeWithGrip;
            this.SetBinding(RibbonWindow.TitleProperty, new Binding("Title"));

            TaskbarItemInfo taskbarItemInfo = new TaskbarItemInfo();
            BindingOperations.SetBinding(taskbarItemInfo, TaskbarItemInfo.DescriptionProperty, new Binding("Title"));
            BindingOperations.SetBinding(taskbarItemInfo, TaskbarItemInfo.ProgressStateProperty, new Binding("ProgressState"));
            BindingOperations.SetBinding(taskbarItemInfo, TaskbarItemInfo.ProgressValueProperty, new Binding("ProgressValue"));
            this.TaskbarItemInfo = taskbarItemInfo;

            this.SetKeyBinding(Key.F5, "RefreshCommand");
            this.SetEventBinding("Loaded", "StartupCommand");
            this.SetEventBinding("Closing", "ClosingCommand");
        }

        private Ribbon FindRibbon(DependencyObject obj)
        {
            if (obj != null)
            {
                if (obj.GetType() == typeof(Ribbon))
                {
                    return (Ribbon)obj;
                }

                int c= VisualTreeHelper.GetChildrenCount(obj);
                //int c2 = VisualTreeHelper.g.GetChildrenCount(obj);

                for (int i = 0; i < c; i++)
                {
                    DependencyObject childReturn = FindRibbon(VisualTreeHelper.GetChild(obj, i));
                    if (childReturn != null)
                    {
                        return (Ribbon)childReturn;
                    }
                }
            }
            return null;
        }
    }
}
