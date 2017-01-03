using System.Windows.Input;
using WpfApplication.Models;

namespace WpfApplication.ViewModels
{
	public interface ICellViewModel
	{
		ICell Cell { get; set; }
		ICommand ChangeCellStateCommand { get; }
	}
}