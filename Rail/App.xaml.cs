using Rail.View;
using Rail.ViewModel;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;

namespace Rail
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void OnStartup(object sender, StartupEventArgs e)
        {
            Trace.TraceInformation("Startup {0} {1}", DateTime.Now.ToLocalTime().ToShortTimeString(), DateTime.Now.ToLocalTime().ToShortDateString());

            CultureInfo.CurrentUICulture = new CultureInfo("de-DE");
            CultureInfo.CurrentCulture = new CultureInfo("de-DE");
            //CultureInfo.CurrentUICulture = new CultureInfo("en-US");    // for UI
            //CultureInfo.CurrentCulture = new CultureInfo("en-US");      // for ToString("F2")
            try
            {
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(OnUnhandledException);
                new MainView()/* { DataContext = new MainViewModel() }*/.Show();
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                MessageBox.Show(ex.ToString());
            }
        }

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            Exception ex = (Exception)args.ExceptionObject;
            Trace.TraceError(ex.ToString());
            MessageBox.Show(ex.ToString(), "Unhandled Error");
        }
    }
}
