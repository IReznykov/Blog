using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using Ikc5.TypeLibrary.Logging;
using Microsoft.Practices.Unity;
using WpfApplication.ViewModels;
using WpfApplication.Views;
using Application = System.Windows.Application;

namespace WpfApplication
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private void InitContainer(IUnityContainer container)
		{
			container.RegisterType<ILogger, EmptyLogger>();
			container.RegisterType<IMainWindowModel, MainWindowModel>();
		}

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			IUnityContainer container = new UnityContainer();
			InitContainer(container);

			var logger = container.Resolve<ILogger>();
			logger.Log($"There are {Screen.AllScreens.Length} screens");

			// calculates text size in that main window (i.e. 100%, 125%,...)
			var ratio = Math.Max(Screen.PrimaryScreen.WorkingArea.Width / SystemParameters.PrimaryScreenWidth,
							Screen.PrimaryScreen.WorkingArea.Height / SystemParameters.PrimaryScreenHeight);

			var pos = 0;
			foreach (var screen in Screen.AllScreens)
			{
				logger.Log(
					$"#{pos + 1} screen, size = ({screen.WorkingArea.Left}, {screen.WorkingArea.Top}, {screen.WorkingArea.Width}, {screen.WorkingArea.Height}), " +
					(screen.Primary ? "primary screen" : "secondary screen"));

				// Show automata at all screen
				var mainViewModel = container.Resolve<IMainWindowModel>(
					new ParameterOverride("backgroundColor", _screenColors[Math.Min(pos++, _screenColors.Length - 1)]),
					new ParameterOverride("primary", screen.Primary),
					new ParameterOverride("displayName", screen.DeviceName));

				var window = new MainWindow(mainViewModel);
				if (screen.Primary)
					Current.MainWindow = window;

				window.Left = screen.WorkingArea.Left / ratio;
				window.Top = screen.WorkingArea.Top / ratio;
				window.Width = screen.WorkingArea.Width / ratio;
				window.Height = screen.WorkingArea.Height / ratio;
				window.Show();
				window.WindowState = WindowState.Maximized;
			}
			Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
		}

		private readonly Color[] _screenColors =
		{
			Colors.LightGray, Colors.DarkGray, Colors.Gray, Colors.SlateGray, Colors.DarkSlateGray
		};
	}
}
