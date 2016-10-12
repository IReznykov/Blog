using System.Windows.Media;
using Ikc5.Prism.Settings;
using Ikc5.Prism.Settings.ViewModels;
using WpfApplication.Models;

namespace WpfApplication.ViewModels
{
	public class SettingsViewModel : UserSettingsViewModel<ISettings>, ISettingsViewModel
	{
		public SettingsViewModel(ISettings settingsModel, IUserSettingsService userSettingsService)
			: base(settingsModel as IUserSettings, userSettingsService)
		{
		}

		#region ISettings

		private string _name;
		private Color _backgroundColor;

		#endregion ISettings

		#region ISettingsViewModel

		/// <summary>
		/// Module's name.
		/// </summary>
		public string Name
		{
			get { return _name; }
			set { SetProperty(ref _name, value); }
		}

		/// <summary>
		/// Background color.
		/// </summary>
		public Color BackgroundColor
		{
			get { return _backgroundColor; }
			set { SetProperty(ref _backgroundColor, value); }
		}

		#endregion ISettingsViewModel

	}
}