using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Ikc5.TypeLibrary.Logging;
using Microsoft.Practices.Unity;
using WpfApplication.Models;
using WpfApplication.Services;
using WpfApplication.ViewModels;
using WpfApplication.Views;

namespace WpfApplication
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private IService _chartService = null;

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			IUnityContainer container = new UnityContainer();

			container.RegisterType<ILogger, EmptyLogger>();
			container.RegisterType<IChartRepository, ChartRepository>(new ContainerControlledLifetimeManager());
			container.RegisterType<IService, ChartService>(new ContainerControlledLifetimeManager());
			container.RegisterType<IMainWindowViewModel, MainWindowViewModel>();
			container.RegisterType<IColumnChartViewModel, ColumnChartViewModel>();
			container.RegisterType<ILineChartViewModel, LineChartViewModel>();

			_chartService = container.Resolve<IService>();
			var mainWindow = container.Resolve<MainWindow>();
			Application.Current.MainWindow = mainWindow;
			Application.Current.MainWindow.Show();

			_chartService.OnStart();
		}

		protected override void OnExit(ExitEventArgs e)
		{
			_chartService.OnStop();
			base.OnExit(e);
		}
	}
}
