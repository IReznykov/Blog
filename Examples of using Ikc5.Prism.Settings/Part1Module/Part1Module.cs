using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ikc5.Prism.Settings;
using Microsoft.Practices.Unity;
using Part1Module.Models;
using Part1Module.Views;
using Prism.Modularity;
using Prism.Regions;

namespace Part1Module
{
	public class Part1Module : IModule
	{
		private readonly IRegionManager _regionManager;

		private readonly IUnityContainer _container;

		public Part1Module(IRegionManager regionManager, IUnityContainer container)
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
