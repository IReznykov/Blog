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

namespace ConsoleApp.FixedCodeTests.Locator
{
    public class Class2Tests
    {
	    [Fact]
	    public void Class2_ShouldReturn_TwoServiceMockValues()
	    {
			const string expectedService2Value = "Service2Mock";
			const string expectedService3Value = "Service3Mock";

			// set services
		    var service2 = Mock.Of<IService2>(mock => mock.GetValue() == expectedService2Value);
		    var service3 = Mock.Of<IService3>(mock => mock.GetValue() == expectedService3Value);

			// set container
			var container = new UnityContainer();

		    container
			    .RegisterInstance(typeof(IService2), service2, new ExternallyControlledLifetimeManager())
			    .RegisterInstance(typeof(IService3), service3, new ExternallyControlledLifetimeManager());

		    var locator = new UnityServiceLocator(container);

			// test routine
			var class2 = new Class2(locator);
		    var values = class2.GetServiceValues()?.ToList();

		    // assert
		    values.Should().NotBeNull();
		    values.Count.Should().Be(2);
		    values[0].Should().Be(expectedService2Value);
		    values[1].Should().Be(expectedService3Value);
	    }
	}
}
