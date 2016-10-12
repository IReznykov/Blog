using System.Windows.Media;

namespace Part1Module.Models
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
		/// Checked state.
		/// </summary>
		bool IsChecked { get; set; }

		/// <summary>
		/// Desired count of something.
		/// </summary>
		int Count { get; set; }
	}
}