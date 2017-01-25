using Ikc5.ScreenSaver.Common.Models;
using Ikc5.ScreenSaver.Common.Views.ViewModels;
using Ikc5.ScreenSaver.FirstModule.Models;

namespace Ikc5.ScreenSaver.FirstModule.ViewModels
{
	public interface ICellViewModel : IBaseCellViewModel
	{
		ISettings Settings { get; }
	}
}