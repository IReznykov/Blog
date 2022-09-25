using System.Linq;
using CommonServiceLocator;
using ConsoleApp.Locator;
using ConsoleApp.Locator.Services;
using ConsoleApp.Locator.Unity;
using FluentAssertions;
using Moq;
using Unity;
using Unity.Lifetime;
using Xunit;

namespace ConsoleApp.FailedTests.Locator
{
	public class Class1Tests
	{
		[Fact]
		public void Class1_ShouldReturn_TwoServiceMockValues()
		{
			const string expectedService1Value = "Service1Mock";
			const string expectedService2Value = "Service2Mock";

			// set services
			var service1 = Mock.Of<IService1>(mock => mock.GetValue() == expectedService1Value);
			var service2 = Mock.Of<IService2>(mock => mock.GetValue() == expectedService2Value);

			// set container
			var container = new UnityContainer();

			container
				.RegisterInstance(typeof(IService1), service1, new ExternallyControlledLifetimeManager())
				.RegisterInstance(typeof(IService2), service2, new ExternallyControlledLifetimeManager());

			var locator = new UnityServiceLocator(container);
			ServiceLocator.SetLocatorProvider(() => locator);

			// test routine
			var class1 = new Class1();
			var values = class1.GetServiceValues()?.ToList();

			// assert
			values.Should().NotBeNull();
			values.Count.Should().Be(2);
			values[0].Should().Be(expectedService1Value);
			values[1].Should().Be(expectedService2Value);
		}

	}
}
