using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;
using Ikc5.ScreenSaver.Common.Models;
using Prism;

namespace Ikc5.ScreenSaver.Common.Views.ViewModels
{
	public interface IDynamicGridViewModel<TCellViewModel> : IActiveAware where TCellViewModel : IBaseCellViewModel
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
		ObservableCollection<ObservableCollection<TCellViewModel>> Cells { get; }

		/// <summary>
		/// Iterates screen saver at one cycle.
		/// </summary>
		ICommand IterateCommand { get; }

		/// <summary>
		/// Command starts iterating.
		/// </summary>
		ICommand StartIteratingCommand { get; }

		/// <summary>
		/// Command stops iterating.
		/// </summary>
		ICommand StopIteratingCommand { get; }

		/// <summary>
		/// Set new initial set of points.
		/// </summary>
		ICommand RestartCommand { get; }
	}
}