using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ikc5.Prism.Settings;
using Microsoft.Practices.Unity;
using Part2Module.Models;
using Part2Module.Views;
using Prism.Modularity;
using Prism.Regions;

namespace Part2Module
{
	public class Part2Module : IModule
	{
		private readonly IRegionManager _regionManager;

		private readonly IUnityContainer _container;

		public Part2Module(IRegionManager regionManager, IUnityContainer container)
		{
			_regionManager = regionManager;
			_container = container;
			ConfigureContainer();
		}

		private void ConfigureContainer()
		{
			_container.RegisterType<ISettings, Models.Settings>(new ContainerControlledLifetimeManager());
		}

		public void Initialize()
		{
			_regionManager.RegisterViewWithRegion($"{GetType().Name}{RegionNames.ModuleSettingsRegion}", typeof(SettingsView));
			_regionManager.RegisterViewWithRegion($"{GetType().Name}{RegionNames.ModuleViewRegion}", typeof(UserControl1));
		}
	}
}
