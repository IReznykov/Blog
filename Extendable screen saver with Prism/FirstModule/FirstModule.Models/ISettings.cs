using System.Drawing;
using Ikc5.ScreenSaver.Common.Models.Types;
using Color = System.Windows.Media.Color;

namespace Ikc5.ScreenSaver.FirstModule.Models
{
	/// <summary>
	/// Settings for the first module.
	/// </summary>
	public interface ISettings
	{
		/// <summary>
		/// Desired width of cell.
		/// </summary>
		int CellWidth { get; set; }

		/// <summary>
		/// Desired height of cell.
		/// </summary>
		int CellHeight { get; set; }

		/// <summary>
		/// Delay between iterations.
		/// </summary>
		int IterationDelay { get; set; }

		/// <summary>
		/// Start, the lighter, color of cells.
		/// </summary>s
		Color StartColor { get; set; }

		/// <summary>
		/// Finish, the darker, color of cells.
		/// </summary>
		Color FinishColor { get; set; }

		/// <summary>
		/// Color of borders around cells.
		/// </summary>
		Color BorderColor { get; set; }

		/// <summary>
		/// Animation type when cell is appearing/disappearing.
		/// </summary>
		AnimationType AnimationType { get; set; }

		/// <summary>
		/// Delay of animation.
		/// </summary>
		int AnimationDelay { get; set; }
	}
}