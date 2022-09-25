using System;
using CommonServiceLocator;
using ConsoleApp.Locator;
using ConsoleApp.Locator.Services;
using ConsoleApp.Locator.Unity;
using ConsoleApp.Resource;
using Unity;
using Class1 = ConsoleApp.Locator.Class1;

namespace ConsoleApp
{
	public class Program
	{
		private static void Main(string[] args)
		{
			// call class that uses static property
			Resources.SharedProperty = 42;
			Console.WriteLine($"Class1 returns {(new Resource.Class1()).GetValues()}");

			// set unity container and service locator provider
			var locator = new UnityServiceLocator(ConfigureUnityContainer());
			ServiceLocator.SetLocatorProvider(() => locator);

			// call classes that uses services from service locator
			var class1 = new Class1();
			Console.WriteLine("Services that are used by Locator.Class1:");
			foreach (var serviceValue in class1.GetServiceValues())
			{
				Console.WriteLine(serviceValue);
			}

			var class2 = new Class2();
			Console.WriteLine("Services that are used by Locator.Class2:");
			foreach (var serviceValue in class2.GetServiceValues())
			{
				Console.WriteLine(serviceValue);
			}
		}

		private static IUnityContainer ConfigureUnityContainer()
		{
			var container = new UnityContainer();

			// register implementations of all used services
			container
				.RegisterType(typeof(IService1), typeof(Service1))
				.RegisterType(typeof(IService2), typeof(Service2))
				.RegisterType(typeof(IService3), typeof(Service3));

			return container;
		}
	}
}
