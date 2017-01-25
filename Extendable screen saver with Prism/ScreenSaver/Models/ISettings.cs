using System.Drawing;
using Ikc5.ScreenSaver.Common.Models.Types;
using Color = System.Windows.Media.Color;

namespace Ikc5.ScreenSaver.Models
{
	/// <summary>
	/// Common settings for Screen Saver.
	/// https://ireznykov.com/2016/10/16/examples-of-using-ikc5-prism-settings/
	/// https://ireznykov.com/2016/10/15/nuget-package-ikc5-prism-settings/
	/// </summary>
	public interface ISettings
	{
		/// <summary>
		/// Name of active module.
		/// </summary>
		string ModuleName { get; set; }

		/// <summary>
		/// Type of the window at secondary monitors.
		/// </summary>
		SecondaryMonitorType SecondaryMonitorType { get; set; }

		/// <summary>
		/// Color of the background.
		/// </summary>
		Color BackgroundColor { get; set; }
	}
}