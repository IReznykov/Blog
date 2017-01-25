using System.Collections.ObjectModel;
using System.Windows.Input;
using Ikc5.ScreenSaver.Common.Views.ViewModels;
using Ikc5.ScreenSaver.FirstModule.Models;

namespace Ikc5.ScreenSaver.FirstModule.ViewModels
{
	public interface IMainViewModel : IDynamicGridViewModel<ICellViewModel>
	{
		/// <summary>
		/// Module settings.
		/// </summary>
		ISettings Settings { get; }

	}

}