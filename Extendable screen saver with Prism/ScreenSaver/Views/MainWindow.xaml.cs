using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Ikc5.Prism.Settings;
using Ikc5.ScreenSaver.ViewModels;
using Ikc5.TypeLibrary.Logging;
using Microsoft.Practices.Unity;

namespace Ikc5.ScreenSaver.Views
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private readonly IUnityContainer _container;
		private ILogger _logger;
		private readonly DispatcherTimer _mouseTimer;

		public MainWindow(IUnityContainer container)
		{
			_container = container;
			DataContext = _container.Resolve<IMainWindowModel>();
			Loaded += MainWindow_Loaded;
			InitializeComponent();

			_mouseTimer = new DispatcherTimer
			{
				IsEnabled = false,
				Interval = TimeSpan.FromMilliseconds(1000),
			};
			_mouseTimer.Tick += MouseTimerTick;
			Mouse.OverrideCursor = Cursors.None;
		}

		private void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			Mouse.OverrideCursor = Cursors.None;
			var dataContext = DataContext as IMainWindowModel;
			dataContext?.SetViewCommand.Execute(null); 
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing(e);

			// serialize settings
			var serializedData = new object();

			var userSettingsService = _container.Resolve<IUserSettingsService>();
			if (userSettingsService.SerializeCommand.CanExecute(serializedData))
				userSettingsService.SerializeCommand.Execute(serializedData);
		}

		#region Mouse operations

		/// <summary>
		/// Method hides mouse cursor.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MouseTimerTick(object sender, EventArgs e)
		{
			_mouseTimer.Stop();
			Mouse.OverrideCursor = Cursors.None;
		}

		private void MainWindow_MouseMove(object sender, MouseEventArgs e)
		{
			if (_isMenuOpen)
				return;

			Mouse.OverrideCursor = Cursors.Arrow;
			_mouseTimer.Stop();
			_mouseTimer.Start();
		}

		private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			_logger.Log("Window MouseLeftButtonDown");
			Application.Current.Shutdown();
		}

		private void Window_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
		{
			_logger.Log("Window MouseRightButtonDown");
		}

		#endregion

		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			_logger.Log("Window KeyDown");
			Application.Current.Shutdown();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			_logger = _container.Resolve<ILogger>();
			_logger.Log($"Window OnLoaded, IsVisible={IsVisible}, IsActive={IsActive}");
			Mouse.OverrideCursor = Cursors.None;
		}

		private void MenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			// show cursor if new dialog is opened
			Mouse.OverrideCursor = Cursors.Arrow;
			_mouseTimer.Stop();
			_isMenuOpen = false;
		}

		private bool _isMenuOpen = false;

		private void ContextMenu_OnOpened(object sender, RoutedEventArgs e)
		{
			// show cursor until menu is opened
			_mouseTimer.Stop();
			_isMenuOpen = true;
			Mouse.OverrideCursor = Cursors.Arrow;
		}

		private void ContextMenu_OnClosed(object sender, RoutedEventArgs e)
		{
			if (!_isMenuOpen)
				return;

			_isMenuOpen = false;
			Mouse.OverrideCursor = Cursors.None;
		}
	}
}
