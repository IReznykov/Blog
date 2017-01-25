using System.Windows.Input;
using Ikc5.ScreenSaver.Common.Models;

namespace Ikc5.ScreenSaver.Common.Views.ViewModels
{
public interface IBaseCellViewModel
{
	/// <summary>
	/// Cell model.
	/// </summary>
	ICell Cell { get; set; }
	}
}