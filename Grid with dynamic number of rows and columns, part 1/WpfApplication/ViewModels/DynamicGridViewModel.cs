using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media;
using Ikc5.TypeLibrary;
using WpfApplication.Models;

namespace WpfApplication.ViewModels
{
	public class DynamicGridViewModel : BaseNotifyPropertyChanged, IDynamicGridViewModel
	{
		public DynamicGridViewModel()
		{
			this.SetDefaultValues();
		}

		#region Initialization and recreating

		/// <summary>
		/// Create 2-dimensional array of cells.
		/// </summary>
		/// <returns></returns>
		private ObservableCollection<ObservableCollection<ICellViewModel>> CreateCells()
		{
			var cells = new ObservableCollection<ObservableCollection<ICellViewModel>>();
			for (var posRow = 0; posRow < GridHeight; posRow++)
			{
				var row = new ObservableCollection<ICellViewModel>();
				for (var posCol = 0; posCol < GridWidth; posCol++)
				{
					var cellViewModel = new CellViewModel(Cell.Empty);
					row.Add(cellViewModel);
				}
				cells.Add(row);
			}
			return cells;
		}

		#endregion

		#region IDynamicGridViewModel

		private ObservableCollection<ObservableCollection<ICellViewModel>> _cells;
		private int _gridWidth;
		private int _gridHeight;

		private Color _startColor;                  // = Colors.AliceBlue;
		private Color _finishColor;                 // = Colors.DarkBlue;
		private Color _borderColor;                 // = Colors.Gray;

		public ObservableCollection<ObservableCollection<ICellViewModel>> Cells
		{
			get { return _cells; }
			set { SetProperty(ref _cells, value); }
		}

		[DefaultValue(5)]
		public int GridWidth
		{
			get { return _gridWidth; }
			set
			{
				var oldValue = _gridWidth;
				SetProperty(ref _gridWidth, value);

				if (oldValue != value)
					Cells = CreateCells();
			}
		}

		[DefaultValue(5)]
		public int GridHeight
		{
			get { return _gridHeight; }
			set
			{
				var oldValue = _gridHeight;
				SetProperty(ref _gridHeight, value);

				if (oldValue != value)
					Cells = CreateCells();
			}
		}

		/// <summary>
		/// Start, the lighter, color of cells.
		/// </summary>
		[DefaultValue(typeof(Color), "#FFF0F8FF")]
		public Color StartColor
		{
			get { return _startColor; }
			set { SetProperty(ref _startColor, value); }
		}

		/// <summary>
		/// Finish, the darker, color of cells.
		/// </summary>
		[DefaultValue(typeof(Color), "#FF00008B")]
		public Color FinishColor
		{
			get { return _finishColor; }
			set { SetProperty(ref _finishColor, value); }
		}

		/// <summary>
		/// Color of borders around cells.
		/// </summary>
		[DefaultValue(typeof(Color), "#FF808080")]
		public Color BorderColor
		{
			get { return _borderColor; }
			set { SetProperty(ref _borderColor, value); }
		}

		#endregion
	}
}
