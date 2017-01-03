using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Ikc5.TypeLibrary.Logging;
using Microsoft.Practices.Unity;
using WpfApplication.ViewModels;
using System.Windows.Media;

namespace WpfApplication
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			IUnityContainer container = new UnityContainer();
			container.RegisterType<ILogger, EmptyLogger>();

			var dynamicGridViewModel = new DynamicGridViewModel(container.Resolve<ILogger>())
			{
				CellWidth = 25,
				CellHeight = 25,
				BorderColor = Colors.Blue,
				StartColor = Colors.Azure,
				FinishColor = Colors.CornflowerBlue
			};

			container.RegisterInstance(typeof(IDynamicGridViewModel), dynamicGridViewModel, new ContainerControlledLifetimeManager());

			var mainWindow = container.Resolve<MainWindow>();
			Application.Current.MainWindow = mainWindow;
			Application.Current.MainWindow.Show();
		}
	}
}
