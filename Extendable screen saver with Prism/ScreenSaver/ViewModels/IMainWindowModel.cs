using System.Windows.Input;
using Ikc5.ScreenSaver.Models;

namespace Ikc5.ScreenSaver.ViewModels
{
	public interface IMainWindowModel
	{
		/// <summary>
		/// Main window settings.
		/// </summary>
		ISettings Settings { get; }

		/// <summary>
		/// Command shows settings dialog.
		/// </summary>
		ICommand SettingsCommand { get; }

		/// <summary>
		/// Command restarts screen saver.
		/// </summary>
		ICommand RestartCommand { get; }

		/// <summary>
		/// Command shows about dialog.
		/// </summary>
		ICommand AboutCommand { get; }

		/// <summary>
		/// Command sets current view.
		/// </summary>
		ICommand SetViewCommand { get; }

	}
}