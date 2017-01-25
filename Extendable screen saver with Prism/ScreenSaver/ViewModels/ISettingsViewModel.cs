using System.Collections;
using System.Collections.ObjectModel;
using Ikc5.ScreenSaver.Models;

namespace Ikc5.ScreenSaver.ViewModels
{
	public interface ISettingsViewModel : ISettings
	{
		ObservableCollection<string> ModuleNameCollection { get; }
	}
}