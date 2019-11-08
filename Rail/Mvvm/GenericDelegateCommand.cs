using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Rail.Mvvm
{
    /// <summary>
    ///     This class allows delegating the commanding logic to methods passed as parameters,
    ///     and enables a View to bind commands to objects that are not part of the element tree.
    /// </summary>
    /// <typeparam name="T">Type of the parameter passed to the delegates</typeparam>
    public class DelegateCommand<T> : ICommand
    {
        private readonly Action<T> execute;
        private readonly Func<T, bool> canExecute;
        //private List<WeakReference> _canExecuteChangedHandlers;

        

        /// <summary>
        ///     Constructor
        /// </summary>
        public DelegateCommand(Action<T> execute, Func<T, bool> canExecuteMethod = null)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            this.execute = execute;
            this.canExecute = canExecute ?? new Func<T, bool>(t => true);
        }
        
        /// <summary>
        ///     Method to determine if the command can be executed
        /// </summary>
        public bool CanExecute(object parameter)
        {
            return this.canExecute((T)parameter);
        }

        /// <summary>
        ///     Execution of the command
        /// </summary>
        public void Execute(object parameter)
        {
            this.execute((T)parameter);
        }

        ///// <summary>
        /////     Raises the CanExecuteChaged event
        ///// </summary>
        //public void RaiseCanExecuteChanged()
        //{
        //    OnCanExecuteChanged();
        //}

        ///// <summary>
        /////     Protected virtual method to raise CanExecuteChanged event
        ///// </summary>
        //protected virtual void OnCanExecuteChanged()
        //{
        //    CommandManagerHelper.CallWeakReferenceHandlers(_canExecuteChangedHandlers);
        //}

        ///// <summary>
        /////     Property to enable or disable CommandManager's automatic requery on this command
        ///// </summary>
        //public bool IsAutomaticRequeryDisabled
        //{
        //    get
        //    {
        //        return _isAutomaticRequeryDisabled;
        //    }
        //    set
        //    {
        //        if (_isAutomaticRequeryDisabled != value)
        //        {
        //            if (value)
        //            {
        //                CommandManagerHelper.RemoveHandlersFromRequerySuggested(_canExecuteChangedHandlers);
        //            }
        //            else
        //            {
        //                CommandManagerHelper.AddHandlersToRequerySuggested(_canExecuteChangedHandlers);
        //            }
        //            _isAutomaticRequeryDisabled = value;
        //        }
        //    }
        //}

        

        /// <summary>
        ///     ICommand.CanExecuteChanged implementation
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
                //CommandManagerHelper.AddWeakReferenceHandler(ref this.canExecuteChangedHandlers, value, 2);
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
                //CommandManagerHelper.RemoveWeakReferenceHandler(_canExecuteChangedHandlers, value);
            }
        }
    }
}
