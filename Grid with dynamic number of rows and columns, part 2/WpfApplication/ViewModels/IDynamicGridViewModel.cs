using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;
using WpfApplication.Models;

namespace WpfApplication.ViewModels
{
	public interface IDynamicGridViewModel
	{
		/// <summary>
		/// Width of current view - expected to be bound to view's actual
		/// width in OneWay binding.
		/// </summary>
		int ViewWidth { get; set; }

		/// <summary>
		/// Height of current view - expected to be bound to view's actual
		/// height in OneWay binding.
		/// </summary>
		int ViewHeight { get; set; }

		/// <summary>
		/// Width of the cell.
		/// </summary>
		int CellWidth { get; set; }

		/// <summary>
		/// Height of the cell.
		/// </summary>
		int CellHeight { get; set; }

		/// <summary>
		/// Count of grid columns.
		/// </summary>
		int GridWidth { get; }

		/// <summary>
		/// Count of grid rows.
		/// </summary>
		int GridHeight { get; }

		/// <summary>
		/// Data model.
		/// </summary>
		CellSet CellSet { get; }

		/// <summary>
		/// 2-dimensional collections for CellViewModels.
		/// </summary>
		ObservableCollection<ObservableCollection<ICellViewModel>> Cells { get; }

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
	}
}