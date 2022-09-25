using System.Threading;
using ConsoleApp.Resource;
using FluentAssertions;
using Moq;
using Xunit;
using Class1 = ConsoleApp.FixedCodeTests.Resource.Class1;

namespace ConsoleApp.FixedCodeTests.Resource
{
	/// <summary>
	/// All tests have statement Thread.Sleep(100) to emulate some set up work.
	/// </summary>
	public class Collection2Tests
	{
		[Theory]
		[InlineData(54)]
		[InlineData(941)]
		[InlineData(8548)]
		[InlineData(14720)]
		public void Class1_ShouldReturn_InputValue(int inputValue)
		{
			var expectedValue = inputValue.ToString("D3");
			var resourcesWrapper = new Mock<IResourcesWrapper>();
			resourcesWrapper.SetupGet(mock => mock.SharedProperty).Returns(inputValue);
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
			const string expectedValue = "319";
			var resourcesWrapper = new Mock<IResourcesWrapper>();
			resourcesWrapper.SetupGet(mock => mock.SharedProperty).Returns(319);
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
			const string expectedValue = "061";
			var resourcesWrapper = new Mock<IResourcesWrapper>();
			resourcesWrapper.SetupGet(mock => mock.SharedProperty).Returns(61);
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
