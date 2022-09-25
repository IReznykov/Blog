using System.Threading;
using ConsoleApp.Resource;
using FluentAssertions;
using Xunit;

namespace ConsoleApp.CollectionTests.Resource
{
	/// <summary>
	/// All tests have statement Thread.Sleep(100) to emulate some set up work.
	/// </summary>
	[Collection("Resource Collection")]
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
			Resources.SharedProperty = inputValue;
			Thread.Sleep(100);

			var class1 = new Class1();

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
			Resources.SharedProperty = 319;
			Thread.Sleep(100);

			var class1 = new Class1();

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
			Resources.SharedProperty = 61;
			Thread.Sleep(100);

			var class1 = new Class1();

			// test routine
			var actualValue = class1.GetValues();

			// assert
			actualValue.Should().NotBeNullOrEmpty();
			actualValue.Should().Be(expectedValue);
		}

	}
}
