using System.Collections.Generic;
using CommonServiceLocator;
using ConsoleApp.Locator.Services;

namespace ConsoleApp.Locator
{
	public class Class2
	{
		public Class2()
		{ }

		/// <summary>
		/// Return values from used services.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<string> GetServiceValues()
		{
			var service2 = ServiceLocator.Current.GetInstance<IService2>();
			yield return service2.GetValue();

			var service3 = ServiceLocator.Current.GetInstance<IService3>();
			yield return service3.GetValue();
		}
	}
}
