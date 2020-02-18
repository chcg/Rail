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

        /// <summary>
        /// Constructor
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
            OnCanExecuteChanged();
        }

        public void OnCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
            //this.CanExecuteChanged.Invoke(this, EventArgs.Empty);
        }


        /// <summary>
        /// ICommand.CanExecuteChanged implementation
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
