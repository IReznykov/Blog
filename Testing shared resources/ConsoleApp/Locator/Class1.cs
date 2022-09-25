using System.Collections.Generic;
using CommonServiceLocator;
using ConsoleApp.Locator.Services;

namespace ConsoleApp.Locator
{
	public class Class1
	{
		public Class1()
		{ }

		/// <summary>
		/// Return values from used services.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<string> GetServiceValues()
		{
			var service1 = ServiceLocator.Current.GetInstance<IService1>();
			yield return service1.GetValue();

			var service2 = ServiceLocator.Current.GetInstance<IService2>();
			yield return service2.GetValue();
		}
	}
}
