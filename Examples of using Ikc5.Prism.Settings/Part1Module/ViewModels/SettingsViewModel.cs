using System.Windows.Media;
using Ikc5.Prism.Settings;
using Ikc5.Prism.Settings.ViewModels;
using Part1Module.Models;

namespace Part1Module.ViewModels
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
		private bool _isChecked;
		private int _count;

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
		/// Checked state.
		/// </summary>
		public bool IsChecked
		{
			get { return _isChecked; }
			set { SetProperty(ref _isChecked, value); }
		}

		/// <summary>
		/// Desired count of something.
		/// </summary>
		public int Count
		{
			get { return _count; }
			set { SetProperty(ref _count, value); }
		}

		#endregion ISettingsViewModel

	}
}