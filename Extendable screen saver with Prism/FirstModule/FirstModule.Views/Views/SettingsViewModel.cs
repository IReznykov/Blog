using Ikc5.Prism.Settings;
using Ikc5.Prism.Settings.ViewModels;
using Ikc5.ScreenSaver.Common.Models.Types;
using Ikc5.ScreenSaver.FirstModule.Models;
using Ikc5.ScreenSaver.FirstModule.ViewModels;
using Color = System.Windows.Media.Color;

namespace Ikc5.ScreenSaver.FirstModule.Views
{
	/// <summary>
	/// View model for module's settings
	/// https://ireznykov.com/2016/10/16/examples-of-using-ikc5-prism-settings/
	/// https://ireznykov.com/2016/10/15/nuget-package-ikc5-prism-settings/
	/// </summary>
	public class SettingsViewModel : UserSettingsViewModel<ISettings>, ISettingsViewModel
	{
		public SettingsViewModel(ISettings settingsModel, IUserSettingsService userSettingsService)
			: base(settingsModel as IUserSettings, userSettingsService)
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

		#endregion ISettings

		#region ISettingsViewModel

		/// <summary>
		/// Width of cell.
		/// </summary>
		public int CellWidth
		{
			get { return _cellWidth; }
			set { SetProperty(ref _cellWidth, value); }
		}

		/// <summary>
		/// Height of cell.
		/// </summary>
		public int CellHeight
		{
			get { return _cellHeight; }
			set { SetProperty(ref _cellHeight, value); }
		}

		/// <summary>
		/// Delay between iterations.
		/// </summary>
		public int IterationDelay
		{
			get { return _iterationDelay; }
			set { SetProperty(ref _iterationDelay, value); }
		}

		/// <summary>
		/// Start, the lighter, color of cells.
		/// </summary>
		public Color StartColor
		{
			get { return _startColor; }
			set { SetProperty(ref _startColor, value); }
		}

		/// <summary>
		/// Finish, the darker, color of cells.
		/// </summary>
		public Color FinishColor
		{
			get { return _finishColor; }
			set { SetProperty(ref _finishColor, value); }
		}

		/// <summary>
		/// Color of borders around cells.
		/// </summary>
		public Color BorderColor
		{
			get { return _borderColor; }
			set { SetProperty(ref _borderColor, value); }
		}

		/// <summary>
		/// Animation type when cell is appearing/disappearing.
		/// </summary>
		public AnimationType AnimationType
		{
			get { return _animationType; }
			set { SetProperty(ref _animationType, value); }
		}

		/// <summary>
		/// Delay of animation.
		/// </summary>
		public int AnimationDelay
		{
			get { return _animationDelay; }
			set { SetProperty(ref _animationDelay, value); }
		}

		#endregion ISettingsViewModel
	}
}
