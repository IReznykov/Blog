using Ikc5.ScreenSaver.SecondModule.Models;
using Ikc5.ScreenSaver.Common.Views.ViewModels;

namespace Ikc5.ScreenSaver.SecondModule.ViewModels
{
	internal class DesignCellViewModel : DesignBaseCellViewModel, ICellViewModel
	{
		public ISettings Settings { get; } = new DesignSettings();
	}
}
