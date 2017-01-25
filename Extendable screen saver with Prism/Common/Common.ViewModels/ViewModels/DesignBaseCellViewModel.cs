using System.Windows.Input;
using Ikc5.ScreenSaver.Common.Models;

namespace Ikc5.ScreenSaver.Common.Views.ViewModels
{
	public class DesignBaseCellViewModel : IBaseCellViewModel
	{
		public ICell Cell { get; set; } = new DesignCell();
	}
}
