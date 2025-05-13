using System;
using System.Windows.Input;

namespace Cbddo.eYazisma.Test.App.Tipler
{
    public class RelayCommand : ICommand
    {
        readonly Action<object> _execute;
        readonly Predicate<object> _canExecute;

        public RelayCommand(Action<object> execute) : this(execute, null)
        {
            try
            {
                _execute = execute ?? throw new NotImplementedException("Not implemented");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            try
            {
                if (null == execute)
                {
                    _execute = null;
                    throw new NotImplementedException("Not implemented");
                }

                _execute = execute;
                _canExecute = canExecute;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        private void InvalidateCanExecute()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
