using System.Collections.ObjectModel;
using System.Windows.Input;
using Ikc5.ScreenSaver.Common.Views.ViewModels;
using Ikc5.ScreenSaver.SecondModule.Models;

namespace Ikc5.ScreenSaver.SecondModule.ViewModels
{
	public interface IMainViewModel : IDynamicGridViewModel<ICellViewModel>
	{
		/// <summary>
		/// Module settings.
		/// </summary>
		ISettings Settings { get; }

	}

}