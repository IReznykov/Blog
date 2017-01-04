using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using Ikc5.TypeLibrary;
using Ikc5.TypeLibrary.Logging;
using WpfApplication.Models;
using WpfApplication.Properties;

namespace WpfApplication.ViewModels
{
	public class DynamicGridViewModel : BaseNotifyPropertyChanged, IDynamicGridViewModel
	{
		private readonly DispatcherTimer _resizeTimer;
		private readonly ILogger _logger;

		public DynamicGridViewModel(ILogger logger)
		{
			logger.ThrowIfNull(nameof(logger));
			_logger = logger;

			this.SetDefaultValues();

			_resizeTimer = new DispatcherTimer
			{
				Interval = TimeSpan.FromMilliseconds(100),
			};
			_resizeTimer.Tick += ResizeTimerTick;
			_logger.Log("DynamicGridViewModel constructor is completed");
		}

		#region Initialization and recreating

		/// <summary>
		/// Cancellation token allows cancel current resizing operation
		/// and start another one.
		/// </summary>
		private CancellationTokenSource _cancellationSource = null;

		private async void CreateOrUpdateCellViewModels()
		{
			_logger.LogStart("Start");

			// stop previous tasks that creates viewModels
			if (_cancellationSource != null && _cancellationSource.Token.CanBeCanceled)
			{
				_cancellationSource.Cancel();
			}

			if (Cells == null)
			{
				Cells = new ObservableCollection<ObservableCollection<ICellViewModel>>();
			}

			try
			{
				_cancellationSource = new CancellationTokenSource();
				await CreateCellViewModelsAsync(0, _cancellationSource.Token).ConfigureAwait(false);
			}
			catch (OperationCanceledException ex)
			{
				_logger.Exception(ex);
			}
			catch (AggregateException ex)
			{
				foreach (var innerException in ex.InnerExceptions)
				{
					_logger.Exception(innerException);
				}
			}
			finally
			{
				_cancellationSource = null;
			}
			_logger.LogEnd("Completed - but add cells in asynchronous way");
		}

		/// <summary>
		/// Recurrent method that analyzes current 2-dimensional array of
		/// view models and adapt it to current size of data model. Method
		/// starts from 0th row and call itself for all necessary rows.
		/// </summary>
		/// <param name="rowNumber">Row number to analyze</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns></returns>
		private Task CreateCellViewModelsAsync(int rowNumber, CancellationToken cancellationToken)
		{
			return Task.Run(async () =>
			{
				cancellationToken.ThrowIfCancellationRequested();
				_logger.Log($"Process {rowNumber} row of cells");
				var positionToProcess = rowNumber;

				if (rowNumber >= GridHeight)
				{
					// either "check" call for the last+1 row, or call for
					// old rows that should be deleted 
					if (rowNumber < Cells.Count)
					{
						_logger.Log($"Remove {rowNumber} row of cells");
						// In order to have responsive GUI, ApplicationIdle is quite good
						// Other priority could be used making adding rows quicker.
						Application.Current.Dispatcher.Invoke(
									() => Cells.RemoveAt(positionToProcess),
									DispatcherPriority.ApplicationIdle,
									cancellationToken);
					}
					else
					{
						_logger.Log($"Empty check of {rowNumber} row of cells");
					}
				}
				else if (rowNumber < Cells.Count)
				{
					_logger.Log($"Update {rowNumber} row of cells");
					// call for rows that already created and should be reInit
					// and maybe changed in their size
					Application.Current.Dispatcher.Invoke(
								() => UpdateCellViewModelRow(positionToProcess),
								DispatcherPriority.ApplicationIdle,
								cancellationToken);
				}
				else
				{
					_logger.Log($"Add {rowNumber} row of cells");
					// In order to have responsive GUI, ApplicationIdle is quite good
					// Other priority could be used making adding rows quicker.
					Application.Current.Dispatcher.Invoke(
								() => CreateCellViewModelRow(positionToProcess),
								DispatcherPriority.ApplicationIdle,
								cancellationToken);
				}

				// create asynchronous task for processing next row
				if (rowNumber < GridHeight)
				{
					await CreateCellViewModelsAsync(++rowNumber, cancellationToken).ConfigureAwait(false);
				}
				else if (rowNumber < Cells.Count)
				{
					await CreateCellViewModelsAsync(rowNumber, cancellationToken).ConfigureAwait(false);
				}

			}, cancellationToken);
		}

		/// <summary>
		/// Add new row of cell view models that corresponds to
		/// rowNumber row in data model.
		/// </summary>
		/// <param name="rowNumber">Number of row in data model.</param>
		private void CreateCellViewModelRow(int rowNumber)
		{
			_logger.Log($"Create {rowNumber} row of cells");
			var row = new ObservableCollection<ICellViewModel>();
			for (var x = 0; x < GridWidth; x++)
			{
				var cellViewModel = new CellViewModel(CellSet.GetCell(x, rowNumber));
				row.Add(cellViewModel);
			}

			_logger.Log($"{rowNumber} row of cells is ready for rendering");
			Cells.Add(row);
		}

		/// <summary>
		/// Add or remove cell view models to the row.
		/// </summary>
		/// <param name="rowNumber">Number of row in data model.</param>
		private void UpdateCellViewModelRow(int rowNumber)
		{
			var row = Cells[rowNumber];
			// delete extra cells
			while (row.Count > GridWidth)
				row.RemoveAt(GridWidth);
			for (var pos = 0; pos < GridWidth; pos++)
			{
				// create new ViewModel or update existent one
				var cell = CellSet.GetCell(pos, rowNumber);
				if (pos < row.Count)
					row[pos].Cell = cell;
				else
				{
					var cellViewModel = new CellViewModel(cell);
					row.Add(cellViewModel);
				}
			}
		}

		#endregion

		#region IDynamicGridViewModel

		private int _viewWidth;
		private int _viewHeight;
		private int _cellWidth;
		private int _cellHeight;
		private int _gridWidth;
		private int _gridHeight;
		private CellSet _cellSet;
		private ObservableCollection<ObservableCollection<ICellViewModel>> _cells;

		private Color _startColor;                  // = Colors.AliceBlue;
		private Color _finishColor;                 // = Colors.DarkBlue;
		private Color _borderColor;                 // = Colors.Gray;

		/// <summary>
		/// Width of current view - expected to be bound to view's actual
		/// width in OneWay binding.
		/// </summary>
		[DefaultValue(0)]
		public int ViewWidth
		{
			get { return _viewWidth; }
			set { SetProperty(ref _viewWidth, value); }
		}

		/// <summary>
		/// Height of current view - expected to be bound to view's actual
		/// height in OneWay binding.
		/// </summary>
		[DefaultValue(0)]
		public int ViewHeight
		{
			get { return _viewHeight; }
			set { SetProperty(ref _viewHeight, value); }
		}

		/// <summary>
		/// Width of the cell.
		/// </summary>
		[DefaultValue(0)]
		public int CellWidth
		{
			get { return _cellWidth; }
			set { SetProperty(ref _cellWidth, value); }
		}

		/// <summary>
		/// Height of the cell.
		/// </summary>
		[DefaultValue(0)]
		public int CellHeight
		{
			get { return _cellHeight; }
			set { SetProperty(ref _cellHeight, value); }
		}

		/// <summary>
		/// Count of grid columns.
		/// </summary>
		[DefaultValue(0)]
		public int GridWidth
		{
			get { return _gridWidth; }
			set { SetProperty(ref _gridWidth, value); }
		}

		/// <summary>
		/// Count of grid rows.
		/// </summary>
		[DefaultValue(0)]
		public int GridHeight
		{
			get { return _gridHeight; }
			set { SetProperty(ref _gridHeight, value); }
		}

		/// <summary>
		/// Data model.
		/// </summary>
		public CellSet CellSet
		{
			get { return _cellSet; }
			set { SetProperty(ref _cellSet, value); }
		}

		/// <summary>
		/// 2-dimensional collections for CellViewModels.
		/// </summary>
		public ObservableCollection<ObservableCollection<ICellViewModel>> Cells
		{
			get { return _cells; }
			set { SetProperty(ref _cells, value); }
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

		#region Resizing

		protected override void OnPropertyChanged(string propertyName = null)
		{
			base.OnPropertyChanged(propertyName);

			if (string.Equals(propertyName, nameof(ViewHeight), StringComparison.InvariantCultureIgnoreCase) ||
				string.Equals(propertyName, nameof(ViewWidth), StringComparison.InvariantCultureIgnoreCase) ||
				string.Equals(propertyName, nameof(CellHeight), StringComparison.InvariantCultureIgnoreCase) ||
				string.Equals(propertyName, nameof(CellWidth), StringComparison.InvariantCultureIgnoreCase))
			{
				ImplementNewSize();
			}
		}

		/// <summary>
		/// Start timer when one of the view's dimensions is changed and wait for another.
		/// </summary>
		private void ImplementNewSize()
		{
			if (ViewHeight == 0 || ViewWidth == 0)
				return;

			if (_resizeTimer.IsEnabled)
				_resizeTimer.Stop();

			_resizeTimer.Start();
		}

		/// <summary>
		/// Method change data model and grid size due to change of view size.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ResizeTimerTick(object sender, EventArgs e)
		{
			_resizeTimer.Stop();

			if (ViewHeight == 0 || ViewWidth == 0)
				return;

			var newWidth = System.Math.Max(1, (int)System.Math.Ceiling((double)ViewWidth / CellWidth));
			var newHeight = System.Math.Max(1, (int)System.Math.Ceiling((double)ViewHeight / CellHeight));
			if (CellSet != null &&
				GridWidth == newWidth &&
				GridHeight == newHeight)
			{
				// the same size, nothing to do
				return;
			}

			// preserve current points
			var currentPoints = CellSet?.GetPoints().Where(point => point.X < newWidth && point.Y < newHeight);
			CellSet = new CellSet(newWidth, newHeight);
			GridWidth = CellSet.Width;
			GridHeight = CellSet.Height;

			if (currentPoints != null)
				CellSet.SetPoints(currentPoints);
			CreateOrUpdateCellViewModels();
		}

		#endregion
	}
}
