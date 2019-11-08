using Rail.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Shell;
using System.Windows.Threading;

namespace Rail.Mvvm
{
    public abstract class AppViewModel : BaseViewModel
    {
        private TaskbarItemProgressState progressState = TaskbarItemProgressState.None;
        private double progressValue = 0.0;
        private string statusText = "Ready";

        public DelegateCommand StartupCommand { get; private set; }
        public DelegateCommand RefreshCommand { get; private set; }
        public DelegateCommand ImportCommand { get; private set; }
        public DelegateCommand ExportCommand { get; private set; }
        public DelegateCommand UndoCommand { get; private set; }
        public DelegateCommand RedoCommand { get; private set; }
        public DelegateCommand OptionsCommand { get; private set; }
        public DelegateCommand AboutCommand { get; private set; }
        public DelegateCommand HelpCommand { get; private set; }
        public DelegateCommand ExitCommand { get; private set; }
        public DelegateCommand<CancelEventArgs> ClosingCommand { get; set; }

        public AppViewModel()
        {
            //Settings.Default.UpgradeFirst();

            this.StartupCommand = new DelegateCommand(OnStartup);
            this.RefreshCommand = new DelegateCommand(OnRefresh, OnCanRefresh);
            this.ImportCommand = new DelegateCommand(OnImport, OnCanImport);
            this.ExportCommand = new DelegateCommand(OnExport, OnCanExport);
            this.UndoCommand = new DelegateCommand(OnUndo, OnCanUndo);
            this.RedoCommand = new DelegateCommand(OnRedo, OnCanRedo);
            this.OptionsCommand = new DelegateCommand(OnOptions, OnCanOptions);
            this.AboutCommand = new DelegateCommand(OnAbout);
            this.HelpCommand = new DelegateCommand(OnHelp);
            this.ExitCommand = new DelegateCommand(OnExit, OnCanExit);
            this.ClosingCommand = new DelegateCommand<CancelEventArgs>(OnClosing);

            //BaseEngine.ProgressState += OnProgressState;
            //BaseEngine.ProgressValue += OnProgressValue;
            //BaseEngine.ProgressText += OnProgressText;
        }

        /// <summary>
        /// Check if update exists and start updating
        /// </summary>
        /// <param name="pathes"></param>
        /// <remarks>Call on OnActivate</remarks>
        //protected virtual void CheckForUpdate(List<string> pathes)
        //{
        //    Version appVersion = Assembly.GetAssembly(this.GetType()).GetName().Version;
        //    Trace.TraceInformation("Current app version: {0}", appVersion);
            
        //    string infoPath = pathes.Select(p => Path.Combine(p, "Update.info")).FirstOrDefault(f => File.Exists(f));
        //    Trace.TraceInformation("Update.info path: {0}", infoPath);
        //    if (!string.IsNullOrEmpty(infoPath))
        //    {
        //        try
        //        {
        //            Update update = Update.Load(infoPath);
        //            string msiPath = update.Locations.FirstOrDefault(f => File.Exists(f));
        //            Version updateVersion = Version.Parse(update.Version);
        //            Trace.TraceInformation("Update.info version: {0} msi path: {1}", updateVersion, msiPath);
        //            if (updateVersion > appVersion && !string.IsNullOrEmpty(msiPath))
        //            {
        //                if (MessageBox.Show("A new application version exists.\r\nWould you like to update?", "Info", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
        //                {
        //                    Trace.TraceInformation("Start update");
        //                    Process.Start(msiPath);
        //                    Application.Current.MainWindow.Close();
        //                }
        //            }
        //        }
        //        catch(Exception ex)
        //        {
        //            Trace.TraceError(ex.ToString());
        //        }
        //    }
        //}

        //private void OnProgressState(ProgressStateEventArgs args)
        //{
        //    this.ProgressState = args.State;
        //}

        //private void OnProgressValue(ProgressValueEventArgs args)
        //{
        //    this.ProgressValue = args.Value;
        //}

        //private void OnProgressText(ProgressTextEventArgs args)
        //{
        //    this.StatusText = args.Text;
        //}

        #region properties
        
        /// <summary>
        /// The main title of the application. Displayed in the main window header and in the taskbar.
        /// </summary>
        public virtual string Title
        {
            get
            {
                Assembly app = Assembly.GetEntryAssembly();
                return app.GetCustomAttribute<AssemblyTitleAttribute>().Title + " " + app.GetName().Version.ToString(2);
            }
        }
        
        /// <summary>
        /// Progress state of the application. Displayed in the taskbar and in the statusbar.
        /// </summary>
        public TaskbarItemProgressState ProgressState
        {
            get
            {
                return this.progressState;
            }
            set
            {
                if (this.progressState != value)
                {
                    this.progressState = value;
                    NotifyPropertyChanged("ProgressState");
                }
            }
        }
        
        /// <summary>
        /// Progress value of the application. Displayed in the taskbar and in the statusbar.
        /// </summary>
        /// <remarks>
        /// The progress value is only visible if the <see cref="ProgressState"/> is not None and is in the range between 0.0 and 1.0.
        /// </remarks>
        public double ProgressValue
        {
            get
            {
                return this.progressValue;
            }
            set
            {
                if (this.progressValue != value)
                {
                    this.progressValue = value;
                    NotifyPropertyChanged("ProgressValue");

                    this.ProgressState = this.progressValue < 0.0 ? TaskbarItemProgressState.None : TaskbarItemProgressState.Normal;
                }
            }
        }
        

        /// <summary>
        /// Status text in status line.
        /// </summary>
        public string StatusText
        {
            get
            {
                return this.statusText;
            }
            set
            {
                this.statusText = value;
                NotifyPropertyChanged("StatusText");
            }
        }

        public bool IsWorking
        {
            get { return false; } // BaseEngine.IsAnyRunning; }
        }
        
        #endregion

        #region command methods

        public virtual void OnStartup()
        {
            if (Application.Current == null)
            {
                // for testing
                OnActivate();
            }
            else
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() => OnActivate()), DispatcherPriority.ContextIdle, null);
            }
        }

        protected virtual void OnActivate()
        { }

        protected virtual bool OnCanRefresh()
        {
            return true;
        }

        protected virtual void OnRefresh()
        { }

        protected virtual void OnImport()
        { }

        protected virtual bool OnCanImport()
        {
            return this.ProgressState == TaskbarItemProgressState.None;
        }

        protected virtual void OnExport()
        { }
        
        protected virtual bool OnCanExport()
        {
            return this.ProgressState == TaskbarItemProgressState.None;
        }

        protected virtual void OnUndo()
        { }
        
        protected virtual bool OnCanUndo()
        {
            return false;
        }

        protected virtual void OnRedo()
        { }

        protected virtual bool OnCanRedo()
        {
            return false;
        }

        protected virtual void OnOptions()
        { }

        protected virtual bool OnCanOptions()
        {
            return true;
        }

        protected virtual void OnAbout()
        { }

        protected virtual void OnHelp()
        {
            string path = Path.ChangeExtension(Assembly.GetEntryAssembly().Location, ".chm");
            if (File.Exists(path))
            {
                System.Diagnostics.Process.Start(path);
            }
            else
            {
                MessageBox.Show(string.Format("Help file \"{0}\" not found!", path), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        protected virtual void OnExit()
        {
            Application.Current.MainWindow.Close();
        }
        
        protected virtual bool OnCanExit()
        {
            return true;
        }
        
        private void OnClosing(CancelEventArgs e)
        {
            if (e != null)
            {
                e.Cancel = !OnClosing();
            }
        }

        protected virtual bool OnClosing()
        {
            return true;
        }

        #endregion
    }
}
