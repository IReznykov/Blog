using System;
using System.Windows;
using CommonLibrary;
using Ikc5.Prism.Settings;
using Ikc5.Prism.Settings.Logging;
using Ikc5.Prism.Settings.Providers;
using Ikc5.Prism.Settings.Services;
using Ikc5.TypeLibrary;
using Microsoft.Practices.Unity;
using Prism.Logging;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Unity;
using WpfApplication.Models;
using WpfApplication.ViewModels;
using WpfApplication.Views;

namespace WpfApplication
{
	public class Bootstrapper : UnityBootstrapper
	{
		/// <summary>
		/// Configures the <see cref="IUnityContainer"/>. May be overwritten in a derived class to add specific
		/// type mappings required by the application.
		/// </summary>
		protected override void ConfigureContainer()
		{
			base.ConfigureContainer();

			ViewModelLocationProvider.SetDefaultViewModelFactory((type) => Container.Resolve(type));
			// create singleton of UserSettingsService
			Container
				.RegisterType<IUserSettingsService, UserSettingsService>(new ContainerControlledLifetimeManager())
				.RegisterType<ILiteObjectService, LiteObjectService>(new ContainerControlledLifetimeManager())
				.RegisterType<ISettings, Settings>(new ContainerControlledLifetimeManager())
				.RegisterType<IMainViewModel, MainViewModel>();

			Container.RegisterType(
				typeof(IUserSettingsProvider<>),
				typeof(FileXmlUserSettingsProvider<>),
				new ContainerControlledLifetimeManager());
		}

		/// <summary>
		/// Creates the shell or main window of the application.
		/// </summary>
		/// <returns>The shell of the application.</returns>
		/// <remarks>
		/// If the returned instance is a <see cref="DependencyObject"/>, the
		/// <see cref="Bootstrapper"/> will attach the default <seealso cref="IRegionManager"/> of
		/// the application in its <see cref="RegionManager.RegionManagerProperty"/> attached property
		/// in order to be able to add regions by using the <seealso cref="RegionManager.RegionNameProperty"/>
		/// attached property from XAML.
		/// </remarks>
		protected override DependencyObject CreateShell()
		{
			Window mainWindow = null;
			try
			{
				mainWindow = Container.Resolve<Views.MainWindow>();
			}
			catch (Exception ex)
			{
				var logger = Container.Resolve<ILoggerFacade>();
				logger?.Exception(ex);
			}
			return mainWindow;
		}

		/// <summary>
		/// Initializes the shell.
		/// </summary>
		/// <remarks>
		/// The base implemention ensures the shell is composed in the container.
		/// </remarks>
		protected override void InitializeShell()
		{
			// add some views to region adapter
			var regionManager = Container.Resolve<IRegionManager>();
			regionManager.RegisterViewWithRegion(RegionNames.AppSettingsRegion, typeof(SettingsView));

			Application.Current.MainWindow.Show();
		}

		protected override void ConfigureModuleCatalog()
		{
			var catalog = (ModuleCatalog)ModuleCatalog;
			// add all modules
			catalog.AddModule(typeof(Part1Module.Part1Module));
			catalog.AddModule(typeof(Part2Module.Part2Module));
		}
	}
}
