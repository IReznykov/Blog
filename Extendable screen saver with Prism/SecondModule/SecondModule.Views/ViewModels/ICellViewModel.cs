using Ikc5.ScreenSaver.Common.Models;
using Ikc5.ScreenSaver.Common.Views.ViewModels;
using Ikc5.ScreenSaver.SecondModule.Models;

namespace Ikc5.ScreenSaver.SecondModule.ViewModels
{
	public interface ICellViewModel : IBaseCellViewModel
	{
		ISettings Settings { get; }
	}
}