using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using Ikc5.Prism.Common.Logging;
using Ikc5.ScreenSaver.Common.Models;
using Ikc5.ScreenSaver.Common.Models.Types;
using Ikc5.ScreenSaver.Models;
using Ikc5.ScreenSaver.Views;
using Microsoft.Practices.Unity;
using Prism.Logging;
using Prism.Unity;

namespace Ikc5.ScreenSaver
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App
	{
		private HwndSource _winWpfContent;

		protected override void OnStartup(StartupEventArgs e)
		{
			ILoggerFacade logger = null;
			UnityBootstrapper bootstrapper;

			try
			{
				base.OnStartup(e);
				// create and launch bootstrapper, but main window is not shown
				bootstrapper = new Bootstrapper();
				bootstrapper.Run();
				logger = bootstrapper.Container.Resolve<ILoggerFacade>();

				// parse input arguments
				logger.Log($"Start parameters: {string.Join("; ", e.Args)}", Category.Info);
			}
			catch (Exception ex)
			{
				logger?.Exception(ex);
				return;
			}

			try
			{
				// parse input arguments
				// There are three command-line parameters all screensavers need to handle:
				//	/ s – Show the screensaver
				//	/ p – Preview the screensaver
				//	/ c – Configure the screensaver
				var launchType = LaunchType.Default;
				var previewWindowDescriptor = 0;

				logger.Log($"Start parameters: {string.Join("; ", e.Args)}", Category.Info);
				if (e.Args.Length > 0)
				{
					var firstArgument = e.Args[0].ToLower().Trim();
					string secondArgument = null;

					// Handle cases where arguments are separated by colon. 
					// Examples: /c:1234567 or /P:1234567
					if (firstArgument.Length > 2)
					{
						secondArgument = firstArgument.Substring(3).Trim();
						firstArgument = firstArgument.Substring(0, 2);
					}
					else if (e.Args.Length > 1)
						secondArgument = e.Args[1];

					if (string.Equals("/c", firstArgument))
						launchType = LaunchType.Configure;
					else if (string.Equals("/s", firstArgument))
						launchType = LaunchType.Show;
					else if (string.Equals("/p", firstArgument))
						launchType = LaunchType.Preview;

					if (!string.IsNullOrEmpty(secondArgument))
						previewWindowDescriptor = Convert.ToInt32(secondArgument);
				}
				logger.Log($"Converted start parameters: launchType={launchType}, previewWindowDescriptor={previewWindowDescriptor}");

				switch (launchType)
				{
				case LaunchType.Default:
				case LaunchType.Show:
					// Normal screensaver mode. Either screen saver was launched normally,
					// or was launched from Preview button.
					{
						logger.Log($"There are {Screen.AllScreens.Length} screens");
						var pos = 0;
						var settings = bootstrapper.Container.Resolve<ISettings>();

						// calculates text size in that main window (i.e. 100%, 125%,...)
						var ratio = Math.Max(Screen.PrimaryScreen.WorkingArea.Width / SystemParameters.PrimaryScreenWidth,
										Screen.PrimaryScreen.WorkingArea.Height / SystemParameters.PrimaryScreenHeight);

						foreach (var screen in Screen.AllScreens)
						{
							logger.Log(
								$"#{++pos} screen, size = ({screen.WorkingArea.Left}, {screen.WorkingArea.Top}, {screen.WorkingArea.Width}, {screen.WorkingArea.Height}), " +
								(screen.Primary ? "primary screen" : "secondary screen"));

							Window window;
							if (screen.Primary)
								window = Current.MainWindow;
							else
							{
								switch (settings.SecondaryMonitorType)
								{
								case SecondaryMonitorType.MainWindow:
									window = bootstrapper.Container.Resolve<MainWindow>();
									break;

								case SecondaryMonitorType.Empty:
								default:
									window = bootstrapper.Container.Resolve<EmptyWindow>();
									break;
								}
							}

							window.Left = screen.WorkingArea.Left / ratio;
							window.Top = screen.WorkingArea.Top / ratio;
							window.Width = screen.WorkingArea.Width / ratio;
							window.Height = screen.WorkingArea.Height / ratio;
							window.Show();
							window.WindowState = WindowState.Maximized;
						}
						Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
					}
					break;

				case LaunchType.Configure:
					// Config mode, launched from Settings button in screen saver dialog
					{
						var settingsWindow = bootstrapper.Container.Resolve<SettingsWindow>();
						settingsWindow.Show();

						Current.ShutdownMode = ShutdownMode.OnLastWindowClose;
						Current.MainWindow.Close();
					}
					break;

				case LaunchType.Preview:
					// Preview mode - display in little window in Screen Saver dialog
					// (Not invoked with Preview button, which runs Screen Saver in normal /s mode).
					{
						var mainWindow = Current.MainWindow as MainWindow;
						if (mainWindow == null)
						{
							Current.Shutdown();
							return;
						}

						logger.Log("Init objects for preview mode");
						var pPreviewHandle = new IntPtr(previewWindowDescriptor);
						var lpRect = new RECT();
						var bGetRect = Win32API.GetClientRect(pPreviewHandle, ref lpRect);

						var sourceParams = new HwndSourceParameters("sourceParams")
						{
							PositionX = 0,
							PositionY = 0,
							Width = lpRect.Right - lpRect.Left,
							Height = lpRect.Bottom - lpRect.Top,
							ParentWindow = pPreviewHandle,
							WindowStyle = (int)(WindowStyles.WS_VISIBLE | WindowStyles.WS_CHILD | WindowStyles.WS_CLIPCHILDREN)
						};

						logger.Log($"Source param size = ({0}, {0}, {lpRect.Right - lpRect.Left}, {lpRect.Bottom - lpRect.Top})");
						_winWpfContent = new HwndSource(sourceParams)
						{
							RootVisual = mainWindow.MainGrid
						};

						// Event that triggers when parent window is disposed - used when doing
						// screen saver preview, so that we know when to exit. If we didn't
						// do this, Task Manager would get a new .scr instance every time
						// we opened Screen Saver dialog or switched dropdown to this saver.
						_winWpfContent.Disposed += (o, args) =>
						{
							logger.Log("_winWpfContent is Disposed, close main window and application");
							mainWindow.Close();
							Current.Shutdown();
						};
						logger.Log(
							$"MainWindow is shown in preview, IsVisible={mainWindow.IsVisible}, IsActive={mainWindow.IsActive}, Owner={mainWindow.Owner?.Title}" +
							$", Rect=({mainWindow.Left}, {mainWindow.Top}, {mainWindow.Width}, {mainWindow.Height})");
					}
					break;

				default:
					// If not running in one of the sanctioned modes, shut down the app
					// immediately (because we don't have a GUI).
					Current.Shutdown();
					break;
				}
			}
			catch (Exception ex)
			{
				logger.Exception(ex);
			}
		}
	}
}
