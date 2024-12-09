using System.Windows.Input;

namespace wpfapp.Utilities
{
    public class RelayCommandParams : ICommand
    {
        private readonly Action<object[]> _execute;
        private readonly Predicate<object[]> _canExecute;

        public RelayCommandParams(Action<object[]> execute, Predicate<object[]> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute((object[])parameter);
        }

        public void Execute(object parameter)
        {
            _execute((object[])parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
