using Microsoft.Win32;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Threading;
using System.Xml.Serialization;
using System.Text.Json.Serialization;

namespace Rail.Mvvm
{
    public abstract class BaseViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        public BaseViewModel()
        {
            this.SelectFolderCommand = new DelegateCommand<string>(OnSelectFolder);
        }

        #region Update

        /// <summary>
        /// Should be overwritten in derived classes to update the values;
        /// </summary>
        /// <returns>true if one ore more values are changed, else false</returns>
        public virtual bool OnUpdate()
        {
            return true;
        }
               
        #endregion

        #region Select Folder

        /// <summary>
        /// Select folder command for a select folder botton near to a path edit field.
        /// </summary>
        /// <example>
        /// &lt;Button Grid.Column="2" Grid.Row="0" Content="..." Command="{Binding SelectFolderCommand}" CommandParameter="PathProperty" Margin="3"/&gt;
        /// </example>
        [XmlIgnore, JsonIgnore]
        public DelegateCommand<string> SelectFolderCommand { get; private set; }
        
        /// <summary>
        /// Handler for folder select command
        /// </summary>
        /// <param name="propertyName">Name of the property with the folder path to select.</param>
        protected virtual void OnSelectFolder(string propertyName)
        {
            string path = (string)this.GetType().GetProperty(propertyName).GetValue(this, null);
            FolderBrowserDialog dlg = new FolderBrowserDialog() { SelectedPath = path, ShowNewFolderButton = true };
            if (dlg.ShowDialog().Value)
            {
                this.GetType().GetProperty(propertyName).SetValue(this, dlg.SelectedPath, null);
            }
        }
        
        #endregion
        
        #region INotifyPropertyChanged
        
        public event PropertyChangedEventHandler PropertyChanged;

        private delegate void NotifyPropertyChangedDeleagte(string propertyName);     

        protected virtual void NotifyPropertyChanged(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentNullException("propertyName");
            }
            if (GetType().GetProperty(propertyName) == null)
            {
                throw new ArgumentOutOfRangeException("propertyName");
            }

            if (Dispatcher.CurrentDispatcher.CheckAccess())
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            else
            {
                Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.DataBind, new NotifyPropertyChangedDeleagte(NotifyPropertyChanged), propertyName);
            }
        }

        protected virtual void NotifyAllPropertiesChanged()
        {
            if (Dispatcher.CurrentDispatcher.CheckAccess())
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
            }
            else
            {
                Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.DataBind, new NotifyPropertyChangedDeleagte(NotifyPropertyChanged), null);
            }
        }

        #endregion

        #region IDataErrorInfo

        public virtual string Error
        {
            get
            {
                return string.Empty;
            }
        }

        public virtual string this[string columnName]
        {
            get
            {
                return string.Empty;
            }
        }

        #endregion
    }
}
