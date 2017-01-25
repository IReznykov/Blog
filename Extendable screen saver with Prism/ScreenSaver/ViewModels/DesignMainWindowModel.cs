using System.Windows.Input;
using Ikc5.ScreenSaver.Models;

namespace Ikc5.ScreenSaver.ViewModels
{
	public class DesignMainWindowModel : IMainWindowModel
	{
		public ISettings Settings { get; } = null;
		public ICommand SettingsCommand { get; } = null;
		public ICommand RestartCommand { get; } = null;
		public ICommand AboutCommand { get; } = null;
		public ICommand SetViewCommand { get; } = null;
	}
}
