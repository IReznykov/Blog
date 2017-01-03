using System.Windows.Input;
using Ikc5.TypeLibrary;
using WpfApplication.Models;

namespace WpfApplication.ViewModels
{
	public class CellViewModel : BaseNotifyPropertyChanged, ICellViewModel
	{
		public CellViewModel(ICell cell = null)
		{
			ChangeCellStateCommand = new DelegateCommand(ChangeCellState, CanChangeCellState);
			if (cell != null)
			{
				Cell = cell;
			}
		}

		#region Cell model

		private ICell _cell;

		public ICell Cell
		{
			get { return _cell; }
			set { SetProperty(ref _cell, value); }
		}

		#endregion Cell model

		#region Commands

		public ICommand ChangeCellStateCommand { get; }

		private void ChangeCellState(object obj)
		{
			if (Cell != null)
				Cell.State = !Cell.State;
		}

		private static bool CanChangeCellState(object obj)
		{
			return true;
		}

		#endregion

	}
}
