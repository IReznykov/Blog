using ConsoleApp.Locator.Services;
using Moq;
using Unity;
using Unity.Lifetime;

namespace ConsoleApp.SynchronizedTests.Locator
{
	internal class Class2ServiceLocator : TestableServiceLocator
	{
		public const string Service2Value = "Service2Mock_ByClass2ServiceLocator";
		public const string Service3Value = "Service3Mock_ByClass2ServiceLocator";

		public Class2ServiceLocator()
			: base(new UnityContainer())
		{
			// set services
			var service2 = Mock.Of<IService2>(mock => mock.GetValue() == Service2Value);
			var service3 = Mock.Of<IService3>(mock => mock.GetValue() == Service3Value);

			// set container
			Container
				.RegisterInstance(typeof(IService2), service2, new ExternallyControlledLifetimeManager())
				.RegisterInstance(typeof(IService3), service3, new ExternallyControlledLifetimeManager());
		}

		public Class2ServiceLocator(IUnityContainer container)
			: base(container)
		{
		}
	}
}
