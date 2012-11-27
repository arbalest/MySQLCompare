using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace MySQLCompare.Architecture
{
    class AppCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;
        private EventHandler _canExecuteChanged;

        public AppCommand(Action<object> execute, Predicate<object> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        /// <summary>
        /// Event that UIElement instances may subscribe to, so they can be notified of when
        /// they should call "CanExecute" to enable or disable a command.
        /// 
        /// The subscriber is then added to two places: the local "_canExecuteChanged" method,
        /// which this class or any consumer of this class can call when it knows the status of the command
        /// might have changed; and the CommandManager's RequerySuggested event, which the CommandManager calls.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add 
            {
                _canExecuteChanged += value;
                CommandManager.RequerySuggested += value;
            }
            
            remove 
            {
                _canExecuteChanged -= value;
                CommandManager.RequerySuggested -= value; 
            }
        }

        /// <summary>
        /// The method to execute when a command is invoked.
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        /// <summary>
        /// The method that determines whether a command should be invoked or not.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }

        /// <summary>
        /// Allows this class to notify subscribers that the command's "CanExecute" might have changed.
        /// </summary>
        protected virtual void OnCanExecuteChanged()
        {
            _canExecuteChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Allows any consumer of this class to request that subscribers be notified that the
        /// command's "CanExecute" might have changed.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            OnCanExecuteChanged();
        }
    }
}
