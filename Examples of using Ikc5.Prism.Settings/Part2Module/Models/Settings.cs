using System;
using System.ComponentModel;
using System.Windows.Media;
using Ikc5.Prism.Settings;
using Ikc5.Prism.Settings.Models;

namespace Part2Module.Models
{
	[Serializable]
	public class Settings : UserSettings, ISettings
	{
		public Settings(IUserSettingsService userSettingsService, IUserSettingsProvider<Settings> userSettingsProvider)
			: base(userSettingsService, userSettingsProvider)
		{
		}

		#region ISettings

		private string _name;               // = "Part2";
		private Color _backgroundColor;     // = Colors.DeepSkyBlue;
		private int _width;                 // = 25;
		private int _height;                // = 25;

		/// <summary>
		/// Module's name.
		/// </summary>
		[DefaultValue("Part2")]
		public string Name
		{
			get { return _name; }
			set { SetProperty(ref _name, value); }
		}

		/// <summary>
		/// Background color.
		/// </summary>
		[DefaultValue(typeof(Color), "#FF00BFFF")]
		public Color BackgroundColor
		{
			get { return _backgroundColor; }
			set { SetProperty(ref _backgroundColor, value); }
		}

		/// <summary>
		/// Desired width of Part2.
		/// </summary>
		[DefaultValue(25)]
		public int Width
		{
			get { return _width; }
			set { SetProperty(ref _width, value); }
		}

		/// <summary>
		/// Desired height of Part2.
		/// </summary>
		[DefaultValue(25)]
		public int Height
		{
			get { return _height; }
			set { SetProperty(ref _height, value); }
		}

		#endregion

	}
}
