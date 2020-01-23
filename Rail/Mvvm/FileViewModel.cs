using Microsoft.Win32;
using Rail.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shell;

namespace Rail.Mvvm
{
    /// <summary>
    /// View Model for single file handling
    /// </summary>
    public abstract class FileViewModel : AppViewModel, IDisposable 
    {
        private readonly string saveBefore = "Do you want to store current file before? If not all changes will be lost.";

        private bool disposed = false;
        protected string DefaultFileExt { get; set; }
        protected string FileFilter { get; set; }
        protected int MaxNumOfRecentFiles { get; set; }
        private ObservableCollection<FileItem> recentFiles;
        private string filePath;
        private bool fileChanged;
        private FileSystemWatcher watcher;


        public DelegateCommand NewCommand { get; private set; }
        public DelegateCommand OpenCommand { get; private set; }
        public DelegateCommand SaveCommand { get; private set; }
        public DelegateCommand SaveAsCommand { get; private set; }
        public DelegateCommand<string> RecentFileCommand { get; private set; }
        public DelegateCommand<DragEventArgs> DragCommand { get; private set; }
        public DelegateCommand<DragEventArgs> DropCommand { get; private set; }

        public FileViewModel()
        {
            this.FilePath = null;
            this.FileChanged = false;
            this.DefaultFileExt = "*.*";
            this.FileFilter = "All Files|*.*";
            this.MaxNumOfRecentFiles = 10;

            this.NewCommand = new DelegateCommand(OnNew, OnCanNew);
            this.OpenCommand = new DelegateCommand(OnOpen, OnCanOpen);
            this.SaveCommand = new DelegateCommand(OnSave, OnCanSave);
            this.SaveAsCommand = new DelegateCommand(OnSaveAs, OnCanSaveAs);
            this.RecentFileCommand = new DelegateCommand<string>(OnOpen);
            this.DragCommand = new DelegateCommand<DragEventArgs>(OnDrag);
            this.DropCommand = new DelegateCommand<DragEventArgs>(OnDrop);
        }

        public override void OnStartup()
        {
            this.watcher = new FileSystemWatcher() { NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName };
            this.watcher.Changed += new FileSystemEventHandler(OnFileChanged);
            this.watcher.Created += new FileSystemEventHandler(OnFileChanged);
            this.watcher.Deleted += new FileSystemEventHandler(OnFileChanged);
            this.watcher.Renamed += new RenamedEventHandler(OnFileRenamed);
                        
            LoadRecentFileList();

            string[] cmd = Environment.GetCommandLineArgs();
            if (cmd.Length > 1 && File.Exists(cmd[1]))
            {
                string path = Path.GetFullPath(cmd[1]);
                OnLoad(path);
                this.FilePath = path;
                this.watcher.Path = Path.GetDirectoryName(path);
                this.watcher.Filter = Path.GetFileName(path);
                this.watcher.EnableRaisingEvents = true;
            }
            else if (this.Autoload && File.Exists(this.AutoloadFile))
            {
                OnLoad(this.AutoloadFile);
                this.FilePath = this.AutoloadFile;
                this.watcher.Path = Path.GetDirectoryName(this.AutoloadFile);
                this.watcher.Filter = Path.GetFileName(this.AutoloadFile);
                this.watcher.EnableRaisingEvents = true;
            }
            base.OnStartup();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);    
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                if (this.watcher != null)
                {
                    this.watcher.Dispose();
                    this.watcher = null;
                }
            }
            this.disposed = true;
        }

        public override string Title
        {
            get
            {
                return string.Format("{0}{1}{2}", base.Title, string.IsNullOrEmpty(this.FilePath) ? "" : " - " + this.FileName, this.FileChanged ? " *" : "");
            }
        }

        public string FilePath
        {
            get
            {
                return this.filePath;
            }
            set
            {
                this.filePath = value;
                NotifyPropertyChanged("FilePath");
                NotifyPropertyChanged("FileName");
                NotifyPropertyChanged("Title");
            }
        }

        public string FileName
        {
            get
            {
                return Path.GetFileNameWithoutExtension(this.filePath);
            }
        }

        public bool FileChanged
        {
            get
            {
                return this.fileChanged;
            }
            set
            {
                this.fileChanged = value;
                NotifyPropertyChanged("FileChanged");
                NotifyPropertyChanged("Title");
            }
        }

        /// <summary>
        /// Collection with the recent files.
        /// </summary>
        public ObservableCollection<FileItem> RecentFiles
        {
            get
            {
                return this.recentFiles;
            }
        }

        protected virtual bool Autoload { get { return false; } }

        protected virtual string AutoloadFile { get { return string.Empty; } set { } }

        public abstract void OnCreate();

        public abstract void OnLoad(string path);

        public abstract void OnStore(string path);

        protected virtual bool OnCanNew()
        {
            return this.ProgressState == TaskbarItemProgressState.None;
        }
        
        protected virtual void OnNew()
        {
            if (StoreChanges())
            {
                //this.FilePath = null;
                //this.FileChanged = true;
                OnCreate();
            }
        }

        protected virtual bool OnCanOpen()
        {
            return this.ProgressState == TaskbarItemProgressState.None;
        }

        protected virtual void OnOpen()
        {
            if (StoreChanges())
            {
                OpenFileDialog dlg = new OpenFileDialog()
                {
                    CheckFileExists = true,
                    CheckPathExists = true,
                    DefaultExt = DefaultFileExt,
                    Filter = FileFilter,
                    Multiselect = false,
                    Title = "Open file ..."
                };
                if (dlg.ShowDialog().GetValueOrDefault())
                {
                    this.watcher.EnableRaisingEvents = false;
                    this.FilePath = dlg.FileName; 
                    OnLoad(dlg.FileName);
                    this.AutoloadFile = dlg.FileName;
                    this.FileChanged = false;
                    AddRecentFile(dlg.FileName);
                    this.watcher.Path = Path.GetDirectoryName(dlg.FileName);
                    this.watcher.Filter = Path.GetFileName(dlg.FileName);
                    this.watcher.EnableRaisingEvents = true;
                }
            }
        }

        protected virtual bool OnCanOpen(string path)
        {
            return this.ProgressState == TaskbarItemProgressState.None;
        }

        protected virtual void OnOpen(string path)
        {
            if (StoreChanges())
            {
                this.watcher.EnableRaisingEvents = false;
                OnLoad(path);
                this.FilePath = path;
                this.AutoloadFile = path;
                this.FileChanged = false;
                AddRecentFile(path);
                this.watcher.Path = Path.GetDirectoryName(path);
                this.watcher.Filter = Path.GetFileName(path);
                this.watcher.EnableRaisingEvents = true;
            }
        }

        protected virtual bool OnCanSave()
        {
            return this.ProgressState == TaskbarItemProgressState.None 
                && !string.IsNullOrEmpty(this.FilePath)
                && this.FileChanged;
        }

        protected virtual void OnSave()
        {
            this.watcher.EnableRaisingEvents = false;
            OnStore(this.FilePath);
            this.FileChanged = false;
            this.watcher.EnableRaisingEvents = true;
        }

        protected virtual bool OnCanSaveAs()
        {
            return this.ProgressState == TaskbarItemProgressState.None;
        }

        protected virtual void OnSaveAs()
        {
            SaveFileDialog dlg = new SaveFileDialog()
            {
                OverwritePrompt = true,
                ValidateNames = true,
                CheckPathExists = true,
                DefaultExt = DefaultFileExt,
                Filter = FileFilter,
                Title = "Save file as ..."
            };
            if (dlg.ShowDialog().GetValueOrDefault())
            {
                this.watcher.EnableRaisingEvents = false;
                OnStore(dlg.FileName);
                this.FilePath = dlg.FileName;
                this.AutoloadFile = dlg.FileName;
                this.FileChanged = false;
                AddRecentFile(dlg.FileName);
                this.watcher.Path = Path.GetDirectoryName(dlg.FileName);
                this.watcher.Filter = Path.GetFileName(dlg.FileName);
                this.watcher.EnableRaisingEvents = true;
            }
        }

        protected virtual void OnFileChanged(object source, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Changed)
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    if (MessageBox.Show(Application.Current.MainWindow, "File changed from outside.\r\nDo you want to reload the file and loose all changes?", "Warning", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        this.watcher.EnableRaisingEvents = false;
                        OnLoad(e.FullPath);
                        this.FileChanged = false;
                        this.watcher.EnableRaisingEvents = true;
                    }
                }));
            }
        }

        protected virtual void OnFileRenamed(object source, RenamedEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Renamed)
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    if  (MessageBox.Show(Application.Current.MainWindow, "File renamed from outside.\r\nDo you want to rename your file too?", "Warning", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        this.watcher.EnableRaisingEvents = false;
                        this.FilePath = e.FullPath;
                        this.AutoloadFile = e.FullPath;
                        this.watcher.Path = Path.GetDirectoryName(e.FullPath);
                        this.watcher.Filter = Path.GetFileName(e.FullPath);
                        this.watcher.EnableRaisingEvents = true;
                    }
                }));
            }
        }

        /// <summary>
        /// Drag handler for drag and drop files
        /// </summary>
        /// <param name="args"></param>
        /// <example>
        /// &lt;Window x:Class="InternalInvoiceManager.MainView" xmlns:i="clr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity"&gt;
        /// &lt;i:Interaction.Triggers&gt;
        ///     &lt;i:EventTrigger EventName="PreviewDragEnter"&gt;
        ///         &lt;EventToCommand Command="{Binding DragCommand}" PassEventArgsToCommand="True"/&gt;
        ///     &lt;/i:EventTrigger&gt;
        ///     &lt;i:EventTrigger EventName="PreviewDragOver"&gt;
        ///         &lt;EventToCommand Command="{Binding DragCommand}" PassEventArgsToCommand="True"/&gt;
        ///     &lt;/i:EventTrigger&gt;
        ///     &lt;i:EventTrigger EventName="PreviewDrop"&gt;
        ///     &lt;EventToCommand Command="{Binding DropCommand}" PassEventArgsToCommand="True"/&gt;
        ///     &lt;/i:EventTrigger&gt;
        /// &lt;/i:Interaction.Triggers&gt;
        /// &lt;/example&gt;
        protected virtual void OnDrag(DragEventArgs args)
        {
            bool isCorrect = true;
            if (args.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                string[] files = args.Data.GetData(DataFormats.FileDrop, true) as string[];
                foreach (string file in files)
                {
                    if (!File.Exists(file) || OnCanOpen(file))
                    {
                        isCorrect = false;
                    }
                }
            }
            args.Effects = isCorrect ? DragDropEffects.All : DragDropEffects.None;
            args.Handled = true; 
        }

        /// <summary>
        /// Drop handler for drag and drop files
        /// </summary>
        /// <param name="args"></param>
        /// <example>
        /// &lt;Window x:Class="InternalInvoiceManager.MainView" xmlns:i="clr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity"&gt;
        /// &lt;i:Interaction.Triggers&gt;
        ///     &lt;i:EventTrigger EventName="PreviewDragEnter"&gt;
        ///         &lt;EventToCommand Command="{Binding DragCommand}" PassEventArgsToCommand="True"/&gt;
        ///     &lt;/i:EventTrigger&gt;
        ///     &lt;i:EventTrigger EventName="PreviewDragOver"&gt;
        ///         &lt;EventToCommand Command="{Binding DragCommand}" PassEventArgsToCommand="True"/&gt;
        ///     &lt;/i:EventTrigger&gt;
        ///     &lt;i:EventTrigger EventName="PreviewDrop"&gt;
        ///     &lt;EventToCommand Command="{Binding DropCommand}" PassEventArgsToCommand="True"/&gt;
        ///     &lt;/i:EventTrigger&gt;
        /// &lt;/i:Interaction.Triggers&gt;
        /// &lt;/example&gt;
        protected virtual void OnDrop(DragEventArgs args)
        {
            if (args.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = args.Data.GetData(DataFormats.FileDrop, true) as string[];
                foreach (string file in files)
                {
                    OnOpen(file);
                }
            }
            args.Handled = true; 
        }

        ///// <summary>
        ///// Closing handler to store changed files on exit.
        ///// </summary>
        ///// <example>
        ///// &lt;Window x:Class="InternalInvoiceManager.MainView" xmlns:i="clr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity"&gt;
        ///// &lt;i:Interaction.Triggers&gt;
        ///// &lt;i:EventTrigger EventName="Closing"&gt;
        /////     &lt;EventToCommand Command="{Binding ClosingCommand}" PassEventArgsToCommand="True"/&gt;
        ///// &lt;/i:EventTrigger&gt;
        ///// </example>
        protected override bool OnClosing()
        {
            return StoreChanges();
        }

        protected virtual bool StoreChanges()
        {
            if (this.FileChanged)
            {
                switch (MessageBox.Show(saveBefore, "Info", MessageBoxButton.YesNoCancel, MessageBoxImage.Question))
                {
                case MessageBoxResult.Yes:
                    if (string.IsNullOrEmpty(this.FilePath))
                    {
                        OnSaveAs();
                    }
                    else
                    {
                        OnSave();
                    }
                    break;
                case MessageBoxResult.No:
                    break;
                case MessageBoxResult.Cancel:
                    return false;
                }
            }
            return true;
        }
                
        private void LoadRecentFileList()
        {
            if (this.recentFiles == null)
            {
                this.recentFiles = new ObservableCollection<FileItem>();
            }
            if (Settings.Default.RecentFiles != null)
            {
                foreach (string path in Settings.Default.RecentFiles)
                {
                    if (File.Exists(path))
                    {
                        this.recentFiles.Add(new FileItem(path));
                    }
                }
            }
        }

        protected void AddRecentFile(string path)
        {
            if (this.recentFiles == null)
            {
                this.recentFiles = new ObservableCollection<FileItem>();
            }
            if (this.recentFiles.Count <= this.MaxNumOfRecentFiles && !this.recentFiles.Any(i => i.Name == Path.GetFileName(path)))
            {
                this.recentFiles.Add(new FileItem(path));

                // store recent file list
                StringCollection col = new StringCollection();
                col.AddRange(this.recentFiles.Select(f => f.Path).ToArray());
                Settings.Default.RecentFiles = col;
                Settings.Default.Save();
            }
        }
    }
}
