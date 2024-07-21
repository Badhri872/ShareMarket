using System.Windows.Input;

namespace Innovators_ShareMarket.ViewModels
{
    public class SimpleCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Action<object> _genericExecute;
        public SimpleCommand(Action execute) 
        {
            _execute = execute;
        }

        public SimpleCommand(Action<object> execute)
        {
            _genericExecute = execute;
        }
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            if (_execute != null)
            {
                _execute();
            }
            else
            {
                _genericExecute(parameter);
            }
            
        }
    }
}
