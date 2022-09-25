using ConsoleApp.Locator.Services;
using Moq;
using Unity;
using Unity.Lifetime;

namespace ConsoleApp.SynchronizedTests.Locator
{
	internal class Class1ServiceLocator : TestableServiceLocator
	{
		public const string Service1Value = "Service1Mock_ByClass1ServiceLocator";
		public const string Service2Value = "Service2Mock_ByClass1ServiceLocator";

		public Class1ServiceLocator()
			: base(new UnityContainer())
		{
			// set services
			var service1 = Mock.Of<IService1>(mock => mock.GetValue() == Service1Value);
			var service2 = Mock.Of<IService2>(mock => mock.GetValue() == Service2Value);

			// set container
			Container
				.RegisterInstance(typeof(IService1), service1, new ExternallyControlledLifetimeManager())
				.RegisterInstance(typeof(IService2), service2, new ExternallyControlledLifetimeManager());
		}

		public Class1ServiceLocator(IUnityContainer container)
			: base(container)
		{
		}

	}
}
