using Ikc5.Prism.Settings;
using Ikc5.ScreenSaver.Common.Models;
using Ikc5.ScreenSaver.FirstModule.Models;
using Ikc5.ScreenSaver.FirstModule.ViewModels;
using Ikc5.ScreenSaver.FirstModule.Views;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;

namespace Ikc5.ScreenSaver.FirstModule
{
	public class FirstModule : IModule
	{
		private readonly IRegionManager _regionManager;

		private readonly IUnityContainer _container;

		public FirstModule(IRegionManager regionManager, IUnityContainer container)
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
