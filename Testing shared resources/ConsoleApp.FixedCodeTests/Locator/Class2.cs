using System.Collections.Generic;
using CommonServiceLocator;
using ConsoleApp.Locator.Services;

namespace ConsoleApp.FixedCodeTests.Locator
{
	public class Class2
	{
		private readonly IServiceLocator _serviceLocator;

		public Class2(IServiceLocator serviceLocator)
		{
			_serviceLocator = serviceLocator;
		}

		public IEnumerable<string> GetServiceValues()
		{
			var service2 = _serviceLocator.GetInstance<IService2>();
			yield return service2.GetValue();

			var service3 = _serviceLocator.GetInstance<IService3>();
			yield return service3.GetValue();
		}
	}
}
