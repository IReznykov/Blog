using System.Windows.Media;
using Ikc5.Prism.Settings;
using Ikc5.Prism.Settings.ViewModels;
using Part2Module.Models;

namespace Part2Module.ViewModels
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
		private int _width;
		private int _height;

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

		/// <summary>
		/// Desired width of Part2.
		/// </summary>
		public int Width
		{
			get { return _width; }
			set { SetProperty(ref _width, value); }
		}

		/// <summary>
		/// Desired height of Part2.
		/// </summary>
		public int Height
		{
			get { return _height; }
			set { SetProperty(ref _height, value); }
		}

		#endregion ISettingsViewModel

	}
}