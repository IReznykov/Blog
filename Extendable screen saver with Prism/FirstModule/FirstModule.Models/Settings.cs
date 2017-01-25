using System;
using System.ComponentModel;
using System.Drawing;
using Ikc5.Prism.Settings;
using Ikc5.Prism.Settings.Models;
using Ikc5.ScreenSaver.Common.Models.Types;
using Color = System.Windows.Media.Color;

namespace Ikc5.ScreenSaver.FirstModule.Models
{
	/// <summary>
	/// Settings for the first module.
	/// Base class initiates properties with default values.
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

		private int _cellWidth;
		private int _cellHeight;
		private int _iterationDelay;
		private Color _startColor;
		private Color _finishColor;
		private Color _borderColor;
		private AnimationType _animationType;
		private int _animationDelay;

		/// <summary>
		/// Desired width of cell.
		/// </summary>
		[DefaultValue(40)]
		public int CellWidth
		{
			get { return _cellWidth; }
			set { SetProperty(ref _cellWidth, value); }
		}

		/// <summary>
		/// Desired height of cell.
		/// </summary>
		[DefaultValue(40)]
		public int CellHeight
		{
			get { return _cellHeight; }
			set { SetProperty(ref _cellHeight, value); }
		}

		/// <summary>
		/// Delay between iterations.
		/// </summary>
		[DefaultValue(1000)]
		public int IterationDelay
		{
			get { return _iterationDelay; }
			set { SetProperty(ref _iterationDelay, value); }
		}

		/// <summary>
		/// Start, the lighter, color of cells.
		/// </summary>
		[DefaultValue(typeof (Color), "#FF8A2BE2")]
		public Color StartColor
		{
			get { return _startColor; }
			set { SetProperty(ref _startColor, value); }
		}

		/// <summary>
		/// Finish, the darker, color of cells.
		/// </summary>
		[DefaultValue(typeof (Color), "#FF00008B")]
		public Color FinishColor
		{
			get { return _finishColor; }
			set { SetProperty(ref _finishColor, value); }
		}

		/// <summary>
		/// Color of borders around cells.
		/// </summary>
		[DefaultValue(typeof(Color), "#FF191970")]
		public Color BorderColor
		{
			get { return _borderColor; }
			set { SetProperty(ref _borderColor, value); }
		}

		/// <summary>
		/// Animation type when cell is appearing/disappearing.
		/// </summary>
		[DefaultValue(AnimationType.Fade)]
		public AnimationType AnimationType
		{
			get { return _animationType; }
			set { SetProperty(ref _animationType, value); }
		}

		/// <summary>
		/// Delay of animation.
		/// </summary>
		[DefaultValue(150)]
		public int AnimationDelay
		{
			get { return _animationDelay; }
			set { SetProperty(ref _animationDelay, value); }
		}

		#endregion
	}
}