using System.Windows.Input;
using WpfApplication.Models;

namespace WpfApplication.ViewModels
{
	public interface ICellViewModel
	{
		/// <summary>
		/// Cell model.
		/// </summary>
		ICell Cell { get; set; }
		/// <summary>
		/// Command that allows change state of the cell.
		/// </summary>
		ICommand ChangeCellStateCommand { get; }
	}
}