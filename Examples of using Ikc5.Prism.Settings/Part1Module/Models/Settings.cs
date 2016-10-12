using System;
using System.ComponentModel;
using System.Windows.Media;
using Ikc5.Prism.Settings;
using Ikc5.Prism.Settings.Models;

namespace Part1Module.Models
{
	[Serializable]
	public class Settings : UserSettings, ISettings
	{
		public Settings(IUserSettingsService userSettingsService, IUserSettingsProvider<Settings> userSettingsProvider)
			: base(userSettingsService, userSettingsProvider)
		{
		}

		#region ISettings

		private string _name;               // = "Part1";
		private Color _backgroundColor;     // = Colors.DarkSeaGreen;
		private bool _isChecked;            // = true;
		private int _count;                 // = 100;

		/// <summary>
		/// Module's name.
		/// </summary>
		[DefaultValue("Part1")]
		public string Name
		{
			get { return _name; }
			set { SetProperty(ref _name, value); }
		}

		/// <summary>
		/// Background color.
		/// </summary>
		[DefaultValue(typeof(Color), "#FF8FBC8F")]
		public Color BackgroundColor
		{
			get { return _backgroundColor; }
			set { SetProperty(ref _backgroundColor, value); }
		}

		/// <summary>
		/// Checked state.
		/// </summary>
		[DefaultValue(true)]
		public bool IsChecked
		{
			get { return _isChecked; }
			set { SetProperty(ref _isChecked, value); }
		}

		/// <summary>
		/// Desired count of something.
		/// </summary>
		[DefaultValue(100)]
		public int Count
		{
			get { return _count; }
			set { SetProperty(ref _count, value); }
		}

		#endregion

	}
}
