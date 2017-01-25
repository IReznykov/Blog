using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Xml.Serialization;
using Ikc5.Prism.Settings;
using Ikc5.Prism.Settings.Models;
using Ikc5.ScreenSaver.Common.Models.Types;
using Color = System.Windows.Media.Color;

namespace Ikc5.ScreenSaver.Models
{
	/// <summary>
	/// Common settings for Screen Saver.
	/// https://ireznykov.com/2016/10/16/examples-of-using-ikc5-prism-settings/
	/// https://ireznykov.com/2016/10/15/nuget-package-ikc5-prism-settings/
	/// </summary>
	[Serializable]
	public class Settings : UserSettings, ISettings
	{
		public Settings(IUserSettingsService userSettingsService, IUserSettingsProvider<Settings> userSettingsProvider)
			: base(userSettingsService, userSettingsProvider)
		{
		}

		#region ISettings

		private string _moduleName;
		private SecondaryMonitorType _secondaryMonitorType;
		private Color _backgroundColor;

		/// <summary>
		/// Name of active module.
		/// </summary>
		[DefaultValue(null)]
		public string ModuleName
		{
			get { return _moduleName; }
			set { SetProperty(ref _moduleName, value); }
		}

		/// <summary>
		/// Type of the window at secondary monitors.
		/// </summary>
		[DefaultValue(SecondaryMonitorType.Empty)]
		public SecondaryMonitorType SecondaryMonitorType
		{
			get { return _secondaryMonitorType; }
			set { SetProperty(ref _secondaryMonitorType, value); }
		}

		/// <summary>
		/// Color of the background.
		/// </summary>
		[DefaultValue(typeof(Color), "#FF000000")]
		public Color BackgroundColor
		{
			get { return _backgroundColor; }
			set { SetProperty(ref _backgroundColor, value); }
		}

		#endregion

	}
}
