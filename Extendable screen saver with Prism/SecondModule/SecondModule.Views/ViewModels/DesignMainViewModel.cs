using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Ikc5.ScreenSaver.Common.Models;
using Ikc5.ScreenSaver.SecondModule.Models;

namespace Ikc5.ScreenSaver.SecondModule.ViewModels
{
	internal class DesignMainViewModel : IMainViewModel
	{
		public int ViewWidth { get; set; } = 300;
		public int ViewHeight { get; set; } = 200;
		public int CellWidth { get; set; } = 25;
		public int CellHeight { get; set; } = 25;
		public int GridWidth { get; } = 12;
		public int GridHeight { get; } = 8;
		public CellSet CellSet { get; } = null;

		public ObservableCollection<ObservableCollection<ICellViewModel>> Cells => null;

		public ISettings Settings { get; } = new DesignSettings();

		public ICommand IterateCommand { get; } = null;
		public ICommand StartIteratingCommand { get; } = null;
		public ICommand StopIteratingCommand { get; } = null;
		public ICommand RestartCommand { get; } = null;
		public bool IsActive { get; set; } = false;
		public event EventHandler IsActiveChanged = delegate { };

	}
}
