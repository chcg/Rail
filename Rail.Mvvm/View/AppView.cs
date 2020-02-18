using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shell;

namespace Rail.Mvvm
{
    public class AppView : Window
    {
        public AppView()
        {
            this.ResizeMode = ResizeMode.CanResizeWithGrip;
            this.SetBinding(Window.TitleProperty, new Binding("Title"));

            TaskbarItemInfo taskbarItemInfo = new TaskbarItemInfo();
            BindingOperations.SetBinding(taskbarItemInfo, TaskbarItemInfo.DescriptionProperty, new Binding("Title"));
            BindingOperations.SetBinding(taskbarItemInfo, TaskbarItemInfo.ProgressStateProperty, new Binding("ProgressState"));
            BindingOperations.SetBinding(taskbarItemInfo, TaskbarItemInfo.ProgressValueProperty, new Binding("ProgressValue"));
            this.TaskbarItemInfo = taskbarItemInfo;

            this.SetKeyBinding(Key.F5, "RefreshCommand");
            this.SetEventBinding("Loaded", "StartupCommand");
            this.SetEventBinding("Closing", "ClosingCommand");
        }

        private Window FindWindow(DependencyObject obj)
        {
            if (obj != null)
            {
                if (obj.GetType() == typeof(Window))
                {
                    return (Window)obj;
                }

                int c = VisualTreeHelper.GetChildrenCount(obj);
                for (int i = 0; i < c; i++)
                {
                    DependencyObject childReturn = FindWindow(VisualTreeHelper.GetChild(obj, i));
                    if (childReturn != null)
                    {
                        return (Window)childReturn;
                    }
                }
            }
            return null;
        }
    }
}
