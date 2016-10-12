using System.Windows.Media;

namespace Part2Module.Models
{
	/// <summary>
	/// Common settings for Module.
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

		/// <summary>
		/// Desired width of Part2.
		/// </summary>
		int Width { get; set; }

		/// <summary>
		/// Desired height of Part2.
		/// </summary>
		int Height { get; set; }
	}
}