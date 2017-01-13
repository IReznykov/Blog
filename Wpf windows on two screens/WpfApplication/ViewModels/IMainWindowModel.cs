using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Input;
using WpfApplication.Models;
using Color = System.Windows.Media.Color;

namespace WpfApplication.ViewModels
{
	public interface IMainWindowModel
	{
		/// <summary>
		/// Background color.
		/// </summary>
		Color BackgroundColor { get; }
		/// <summary>
		/// Width of the view.
		/// </summary>
		double ViewWidth { get; set; }
		/// <summary>
		/// Height of the view.
		/// </summary>
		double ViewHeight { get; set; }
		/// <summary>
		/// Set of rectangles.
		/// </summary>
		ObservableCollection<ScreenRectangle> Rectangles { get; }
	}
}