using System.Threading;
using ConsoleApp.Resource;
using FluentAssertions;
using Xunit;

namespace ConsoleApp.SynchronizedTests.Resource
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
		public void Class1_ShouldRetrn_InputValue(int inputValue)
		{
			var expectedValue = inputValue.ToString("D3");
			string actualValue;
			using (var synchronizeTest = new SynhronizeTest<Class1>())
			{
				synchronizeTest.Lock();
				Resources.SharedProperty = inputValue;
				Thread.Sleep(100);

				var class1 = new Class1();

				// test routine
				actualValue = class1.GetValues();
			}

			// assert
			actualValue.Should().NotBeNullOrEmpty();
			actualValue.Should().Be(expectedValue);
		}

		[Fact]
		public void Class1_ShouldRetrn_FirstValue()
		{
			const string expectedValue = "419";
			string actualValue;
			using (var synchronizeTest = new SynhronizeTest<Class1>())
			{
				synchronizeTest.Lock();
				Resources.SharedProperty = 419;
				Thread.Sleep(100);

				var class1 = new Class1();

				// test routine
				actualValue = class1.GetValues();
			}

			// assert
			actualValue.Should().NotBeNullOrEmpty();
			actualValue.Should().Be(expectedValue);
		}

		[Fact]
		public void Class1_ShouldRetrn_SecondValue()
		{
			const string expectedValue = "061";
			string actualValue;
			using (var synchronizeTest = new SynhronizeTest<Class1>())
			{
				synchronizeTest.Lock();
				Resources.SharedProperty = 61;
				Thread.Sleep(100);

				var class1 = new Class1();

				// test routine
				actualValue = class1.GetValues();
			}

			// assert
			actualValue.Should().NotBeNullOrEmpty();
			actualValue.Should().Be(expectedValue);
		}

	}
}
