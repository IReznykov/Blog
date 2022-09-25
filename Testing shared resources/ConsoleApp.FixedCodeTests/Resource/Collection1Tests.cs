using FluentAssertions;
using Moq;
using System.Threading;
using Xunit;

namespace ConsoleApp.FixedCodeTests.Resource
{
	/// <summary>
	/// All tests have statement Thread.Sleep(100) to emulate some set up work.
	/// </summary>
	public class Collection1Tests
	{
		[Fact]
		public void Class1_ShouldReturn_InitValue()
		{
			const string expectedValue = "017";
			var resourcesWrapper = new Mock<IResourcesWrapper>();
			resourcesWrapper.SetupGet(mock => mock.SharedProperty).Returns(17);
			Thread.Sleep(100);

			var class1 = new Class1(resourcesWrapper.Object);

			// test routine
			var actualValue = class1.GetValues();

			// assert
			actualValue.Should().NotBeNullOrEmpty();
			actualValue.Should().Be(expectedValue);
		}

		[Fact]
		public void Class1_ShouldReturn_FirstValue()
		{
			const string expectedValue = "043";
			var resourcesWrapper = new Mock<IResourcesWrapper>();
			resourcesWrapper.SetupGet(mock => mock.SharedProperty).Returns(43);
			Thread.Sleep(100);

			var class1 = new Class1(resourcesWrapper.Object);

			// test routine
			var actualValue = class1.GetValues();

			// assert
			actualValue.Should().NotBeNullOrEmpty();
			actualValue.Should().Be(expectedValue);
		}

		[Fact]
		public void Class1_ShouldReturn_SecondValue()
		{
			const string expectedValue = "139";
			var resourcesWrapper = new Mock<IResourcesWrapper>();
			resourcesWrapper.SetupGet(mock => mock.SharedProperty).Returns(139);
			Thread.Sleep(100);

			var class1 = new Class1(resourcesWrapper.Object);

			// test routine
			var actualValue = class1.GetValues();

			// assert
			actualValue.Should().NotBeNullOrEmpty();
			actualValue.Should().Be(expectedValue);
		}

	}
}
