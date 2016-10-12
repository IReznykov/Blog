using System.Windows.Media;

namespace WpfApplication.Models
{
	/// <summary>
	/// Common settings for Application.
	/// </summary>
	public interface ISettings
	{
		/// <summary>
		/// Module's name.
		/// </summary>
		string Name { get; set; }

		/// <summary>
		/// Background color.
		/// </summary>s
		Color BackgroundColor { get; set; }
	}
}