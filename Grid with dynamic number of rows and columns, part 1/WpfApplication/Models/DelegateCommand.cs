using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfApplication.Models
{
	/// <summary>
	/// Taken from https://wpftutorial.net/DelegateCommand.html + Resharper
	/// </summary>
	public class DelegateCommand : ICommand
	{
		private readonly Predicate<object> _canExecute;
		private readonly Action<object> _execute;

		public event EventHandler CanExecuteChanged;

		public DelegateCommand(Action<object> execute)
					   : this(execute, null)
		{
		}

		public DelegateCommand(Action<object> execute,
					   Predicate<object> canExecute)
		{
			_execute = execute;
			_canExecute = canExecute;
		}

		public virtual bool CanExecute(object parameter)
		{
			if (_canExecute == null)
			{
				return true;
			}

			return _canExecute(parameter);
		}

		public virtual void Execute(object parameter)
		{
			_execute(parameter);
		}

		public void RaiseCanExecuteChanged()
		{
			CanExecuteChanged?.Invoke(this, EventArgs.Empty);
		}
	}

}
