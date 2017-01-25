using Ikc5.Prism.Settings;
using Ikc5.ScreenSaver.Common.Models;
using Ikc5.ScreenSaver.SecondModule.Models;
using Ikc5.ScreenSaver.SecondModule.ViewModels;
using Ikc5.ScreenSaver.SecondModule.Views;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;

namespace Ikc5.ScreenSaver.SecondModule
{
	public class SecondModule : IModule
	{
		private readonly IRegionManager _regionManager;

		private readonly IUnityContainer _container;

		public SecondModule(IRegionManager regionManager, IUnityContainer container)
		{
			_regionManager = regionManager;
			_container = container;
			ConfigureContainer();
		}

		private void ConfigureContainer()
		{
			_container.
				RegisterType<ICell, Cell>().
				RegisterType<ICellViewModel, CellViewModel>().
				RegisterType<IMainViewModel, MainViewModel>().
				RegisterType<ISettings, Settings>(new ContainerControlledLifetimeManager());
		}

		public void Initialize()
		{
			_regionManager.RegisterViewWithRegion(PrismNames.MainRegionName, typeof(MainView));
			_regionManager.RegisterViewWithRegion($"{GetType().Name}{RegionNames.ModuleSettingsRegion}", typeof(SettingsView));
		}

	}
}
