using System;
using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddIn.Models
{
    public class Relay_Command : ICommand
    {
        private readonly Action<object> _execute; // Action<object>로 수정하여 파라미터를 받을 수 있도록
        private readonly Func<bool> _canExecute;

        public Relay_Command(Action<object> execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        public void Execute(object parameter)
        {
            _execute(parameter); // Execute 메서드에서 파라미터를 사용하도록 수정
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }



    }
}
