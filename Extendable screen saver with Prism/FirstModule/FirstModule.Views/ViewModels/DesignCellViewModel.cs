using Ikc5.ScreenSaver.FirstModule.Models;
using Ikc5.ScreenSaver.Common.Views.ViewModels;

namespace Ikc5.ScreenSaver.FirstModule.ViewModels
{
	internal class DesignCellViewModel : DesignBaseCellViewModel, ICellViewModel
	{
		public ISettings Settings { get; } = new DesignSettings();
	}
}
