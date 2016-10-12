using System;
using System.ComponentModel;
using System.Windows.Media;
using Ikc5.Prism.Settings;
using Ikc5.Prism.Settings.Models;

namespace WpfApplication.Models
{
	[Serializable]
	public class Settings : UserSettings, ISettings
	{
		public Settings(IUserSettingsService userSettingsService, IUserSettingsProvider<Settings> userSettingsProvider)
			: base(userSettingsService, userSettingsProvider)
		{
		}

		#region ISettings

		private string _name;               // = "Application";
		private Color _backgroundColor;     // = Colors.DarkGray;

		/// <summary>
		/// Module's name.
		/// </summary>
		[DefaultValue("Application")]
		public string Name
		{
			get { return _name; }
			set { SetProperty(ref _name, value); }
		}

		/// <summary>
		/// Color of the background.
		/// </summary>
		[DefaultValue(typeof(Color), "#FFA9A9A9")]
		public Color BackgroundColor
		{
			get { return _backgroundColor; }
			set { SetProperty(ref _backgroundColor, value); }
		}

		#endregion

	}
}
