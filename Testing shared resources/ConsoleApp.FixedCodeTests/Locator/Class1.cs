using System.Collections.Generic;
using CommonServiceLocator;
using ConsoleApp.Locator.Services;

namespace ConsoleApp.FixedCodeTests.Locator
{
	public class Class1
	{
		private readonly IServiceLocator _serviceLocator;

		public Class1(IServiceLocator serviceLocator)
		{
			_serviceLocator = serviceLocator;
		}

		public IEnumerable<string> GetServiceValues()
		{
			var service1 = _serviceLocator.GetInstance<IService1>();
			yield return service1.GetValue();

			var service2 = _serviceLocator.GetInstance<IService2>();
			yield return service2.GetValue();
		}
	}
}
