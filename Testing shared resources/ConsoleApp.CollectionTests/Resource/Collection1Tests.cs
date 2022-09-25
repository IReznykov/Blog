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
	public class Collection1Tests
	{
		[Fact]
		public void Class1_ShouldReturn_InitValue()
		{
			const string expectedValue = "017";
			Resources.SharedProperty = 17;
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
			const string expectedValue = "043";
			Resources.SharedProperty = 43;
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
			const string expectedValue = "139";
			Resources.SharedProperty = 139;
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
