using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Ikc5.ScreenSaver.Common.Models;
using Ikc5.ScreenSaver.Common.Views.Properties;
using Ikc5.TypeLibrary;
using Ikc5.TypeLibrary.Logging;
using Prism;
using Prism.Commands;
using Prism.Mvvm;
using Point = System.Drawing.Point;

namespace Ikc5.ScreenSaver.Common.Views.ViewModels
{
	public abstract class DynamicGridViewModel<TCellViewModel> : BindableBase, IDynamicGridViewModel<TCellViewModel> where TCellViewModel : IBaseCellViewModel
	{
		private bool _postponedTimer = false;
		private readonly DispatcherTimer _resizeTimer;
		private readonly DispatcherTimer _iterateTimer;
		protected readonly ILogger Logger;

		protected DynamicGridViewModel(
			int iterationDelay,
			ICommandProvider commandProvider,
			ILogger logger)
		{
			logger.ThrowIfNull(nameof(logger));
			Logger = logger;

			commandProvider.ThrowIfNull(nameof(commandProvider));

			this.SetDefaultValues();

			_resizeTimer = new DispatcherTimer
			{
				Interval = TimeSpan.FromMilliseconds(100),
			};
			_resizeTimer.Tick += ResizeTimerTick;

			IterationDelay = iterationDelay;
			_iterateTimer = new DispatcherTimer
			{
				Interval = TimeSpan.FromMilliseconds(IterationDelay),
			};
			_iterateTimer.Tick += IterateTimerTick;

			// create commands
			IterateCommand = new DelegateCommand(Iterate, () => CanIterate)
				{ IsActive = IsActive };
			StartIteratingCommand = new DelegateCommand(StartTimer, () => CanStartIterating)
				{ IsActive = IsActive };
			StopIteratingCommand = new DelegateCommand(StopTimer, () => CanStopIterating)
				{ IsActive = IsActive };
			RestartCommand = new DelegateCommand(Restart, () => CanRestart)
				{ IsActive = IsActive };

			// register command in composite commands
			commandProvider.IterateCommand.RegisterCommand(IterateCommand);
			commandProvider.StartIteratingCommand.RegisterCommand(StartIteratingCommand);
			commandProvider.StopIteratingCommand.RegisterCommand(StopIteratingCommand);
			commandProvider.RestartCommand.RegisterCommand(RestartCommand);

			SetCommandProviderMode(CommandProviderMode.None);
			Logger.Log("DynamicGridViewModel constructor is completed");
		}

		#region Initialization and recreating

		protected IEnumerable<Point> GenerateRandomPoints(int seed = 5)
		{
			var listPoints = new List<Point>();
			var random = new Random();
			for (var y = 0; y < GridHeight; y++)
			{
				var rowBytes = new byte[GridWidth];
				random.NextBytes(rowBytes);
				for (var x = 0; x < GridWidth; x++)
				{
					if (rowBytes[x] % seed == 0)
						listPoints.Add(new Point(x, y));
				}
			}
			return listPoints;
		}

		/// <summary>
		/// Abstract method creates derived class for Cell view model.
		/// </summary>
		/// <param name="cell"></param>
		/// <returns></returns>
		protected abstract TCellViewModel CreateCellViewModel(ICell cell);

		/// <summary>
		/// Cancellation token allows cancel current resizing operation
		/// and start another one.
		/// </summary>
		private CancellationTokenSource _cancellationSource = null;

		private async void CreateOrUpdateCellViewModels()
		{
			Logger.LogStart("Start");

			// stop previous tasks that creates viewModels
			if (_cancellationSource != null && _cancellationSource.Token.CanBeCanceled)
			{
				_cancellationSource.Cancel();
			}

			if (Cells == null)
			{
				Cells = new ObservableCollection<ObservableCollection<TCellViewModel>>();
			}

			try
			{
				_cancellationSource = new CancellationTokenSource();
				await CreateCellViewModelsAsync(0, _cancellationSource.Token).ConfigureAwait(false);
			}
			catch (OperationCanceledException ex)
			{
				Logger.Exception(ex);
			}
			catch (AggregateException ex)
			{
				foreach (var innerException in ex.InnerExceptions)
				{
					Logger.Exception(innerException);
				}
			}
			finally
			{
				_cancellationSource = null;
			}
			Logger.LogEnd("Completed - but add cells in asynchronous way");
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
				Logger.Log($"Process {rowNumber} row of cells");
				var positionToProcess = rowNumber;

				if (rowNumber >= GridHeight)
				{
					// either "check" call for the last+1 row, or call for
					// old rows that should be deleted 
					if (rowNumber < Cells.Count)
					{
						Logger.Log($"Remove {rowNumber} row of cells");
						// In order to have responsive GUI, ApplicationIdle is quite good
						// Other priority could be used making adding rows quicker.
						Application.Current.Dispatcher.Invoke(
									() => Cells.RemoveAt(positionToProcess),
									DispatcherPriority.ApplicationIdle,
									cancellationToken);
					}
					else
					{
						Logger.Log($"Empty check of {rowNumber} row of cells");
					}
				}
				else if (rowNumber < Cells.Count)
				{
					Logger.Log($"Update {rowNumber} row of cells");
					// call for rows that already created and should be reInit
					// and maybe changed in their size
					Application.Current.Dispatcher.Invoke(
								() => UpdateCellViewModelRow(positionToProcess),
								DispatcherPriority.ApplicationIdle,
								cancellationToken);
				}
				else
				{
					Logger.Log($"Add {rowNumber} row of cells");
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
			Logger.Log($"Create {rowNumber} row of cells");
			var row = new ObservableCollection<TCellViewModel>();
			for (var x = 0; x < GridWidth; x++)
			{
				var cellViewModel = CreateCellViewModel(CellSet.GetCell(x, rowNumber));
				row.Add(cellViewModel);
			}

			Logger.Log($"{rowNumber} row of cells is ready for rendering");
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
					var cellViewModel = CreateCellViewModel(cell);
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
		private ObservableCollection<ObservableCollection<TCellViewModel>> _cells;

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
		public ObservableCollection<ObservableCollection<TCellViewModel>> Cells
		{
			get { return _cells; }
			set { SetProperty(ref _cells, value); }
		}

		/// <summary>
		/// Iterates screen saver at one cycle.
		/// </summary>
		public ICommand IterateCommand { get; }

		/// <summary>
		/// Command starts iterating.
		/// </summary>
		public ICommand StartIteratingCommand { get; }

		/// <summary>
		/// Command stops iterating.
		/// </summary>
		public ICommand StopIteratingCommand { get; }

		/// <summary>
		/// Command reinit cell set.
		/// </summary>
		public ICommand RestartCommand { get; }

		#region CanExecute commands

		/// <summary>
		/// Defines that IterateCommand could be executed.
		/// </summary>
		protected bool CanIterate { get; set; }

		/// <summary>
		/// Defines that StartIteratingCommand could be executed.
		/// </summary>
		protected bool CanStartIterating { get; set; }

		/// <summary>
		/// Defines that StopIteratingCommand could be executed.
		/// </summary>
		protected bool CanStopIterating { get; set; }

		/// <summary>
		/// Defines that RestartCommand could be executed.
		/// </summary>
		protected bool CanRestart { get; set; }

		#endregion
		#endregion

		#region IActiveAware

		private bool _isActive;

		/// <summary>
		/// Active view.
		/// </summary>
		[DefaultValue(false)]
		public bool IsActive
		{
			get { return _isActive; }
			set
			{
				if (_isActive == value)
					return;

				_isActive = value;
				OnIsActiveChanged();

				((DelegateCommand)IterateCommand).IsActive = _isActive;
				((DelegateCommand)StartIteratingCommand).IsActive = _isActive;
				((DelegateCommand)StopIteratingCommand).IsActive = _isActive;
				((DelegateCommand)RestartCommand).IsActive = _isActive;
			}
		}

		public event EventHandler IsActiveChanged = delegate { };

		protected virtual void OnIsActiveChanged()
		{
			IsActiveChanged.Invoke(this, EventArgs.Empty);
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

			var newSize = GetNewSize();
			int newWidth = (int)newSize.Width, newHeight = (int)newSize.Height;

			if (CellSet != null &&
				GridWidth == newWidth &&
				GridHeight == newHeight)
			{
				// the same size, nothing to do
				return;
			}

			// prevent timer's start until cell set will be recreate
			SetCommandProviderMode(CommandProviderMode.None);
			PauseIteration();

			// preserve current points
			var currentPoints = CellSet?.GetPoints().Where(point => point.X < newWidth && point.Y < newHeight);
			CellSet = new CellSet(newWidth, newHeight);
			GridWidth = CellSet.Width;
			GridHeight = CellSet.Height;

			if (currentPoints != null)
				CellSet.SetPoints(currentPoints);
			CreateOrUpdateCellViewModels();

			// post recreating actions
			if (_postponedTimer)
			{
				StartTimer();
			}
			else
			{
				SetCommandProviderMode(CommandProviderMode.Init);
			}
		}

		protected virtual Size GetNewSize()
		{
			return new Size(Math.Max(1, (int)Math.Ceiling((double)ViewWidth / CellWidth)),
							Math.Max(1, (int)Math.Ceiling((double)ViewHeight / CellHeight)));
		}

		#endregion

		#region Iterating

		private int _iterationDelay;

		protected int IterationDelay
		{
			get { return _iterationDelay; }
			set
			{
				_iterationDelay = Math.Max(100, value);
				if (_iterateTimer != null)
					_iterateTimer.Interval = TimeSpan.FromMilliseconds(IterationDelay);
			}
		}

		private void IterateTimerTick(object sender, EventArgs e)
		{
#if DEBUG
			Logger.Log("Timer ticks");
#endif
			Iterate();
		}

		private void Iterate()
		{
			if (CellSet == null)
				return;

			using (Application.Current.Dispatcher.DisableProcessing())
			{
				var points = GenerateRandomPoints(11);
				CellSet.InvertPoints(points);
			}
		}

		protected void PauseIteration()
		{
			if (_iterateTimer.IsEnabled)
			{
				_postponedTimer = true;
				_iterateTimer.Stop();
			}
		}

		protected void ContinueIteration()
		{
			if (_postponedTimer)
				StartTimer();
		}

		/// <summary>
		/// Start the timer for screen saver iterations. But method could have not effect
		/// if cell set still waits for all necessary data for creating. Then timer will
		/// start after cell set has been created.
		/// </summary>
		private void StartTimer()
		{
			if (CellSet == null || Cells == null)
				_postponedTimer = true;
			else
			{
				_iterateTimer.Start();
				_postponedTimer = false;
				SetCommandProviderMode(CommandProviderMode.Iterating);
			}
		}

		/// <summary>
		/// Stop iteration timer.
		/// </summary>
		private void StopTimer()
		{
			_iterateTimer.Stop();
			SetCommandProviderMode(CommandProviderMode.Init);
		}

		private void SetCommandProviderMode(CommandProviderMode mode)
		{
			switch (mode)
			{
			case CommandProviderMode.Init:
				CanIterate = true;
				CanStartIterating = true;
				CanStopIterating = false;
				CanRestart = true;
				break;

			case CommandProviderMode.Iterating:
				CanIterate = false;
				CanStartIterating = false;
				CanStopIterating = true;
				CanRestart = true;
				break;

			case CommandProviderMode.None:
			default:
				CanIterate = false;
				CanStartIterating = false;
				CanStopIterating = false;
				CanRestart = false;
				break;
			}

			CommandManager.InvalidateRequerySuggested();
		}

		private enum CommandProviderMode
		{
			None = 0,
			Init,
			Iterating,
		}

		/// <summary>
		/// Set new initial set of points.
		/// </summary>
		private void Restart()
		{
			if (CellSet == null)
				return;

			// prevent timer's start until cell set will be reinit
			SetCommandProviderMode(CommandProviderMode.None);
			PauseIteration();

			using (Application.Current.Dispatcher.DisableProcessing())
			{
				CellSet.Clear();
				var points = GenerateRandomPoints();
				CellSet.SetPoints(points);
			}

			// post recreating actions
			if (_postponedTimer)
			{
				StartTimer();
			}
			else
			{
				SetCommandProviderMode(CommandProviderMode.Init);
			}
		}

		#endregion
	}
}
