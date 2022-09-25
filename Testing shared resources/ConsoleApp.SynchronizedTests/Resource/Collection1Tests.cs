using System.Threading;
using ConsoleApp.Resource;
using FluentAssertions;
using Moq;
using Xunit;

namespace ConsoleApp.SynchronizedTests.Resource
{
	/// <summary>
	/// All tests have statement Thread.Sleep(100) to emulate some set up work.
	/// </summary>
	public class Collection1Tests
	{
		[Fact]
		public void Class1_ShouldRetrn_InitValue()
		{
			const string expectedValue = "017";
			string actualValue;
			using (var synchronizeTest = new SynhronizeTest<Class1>())
			{
				synchronizeTest.Lock();
				Resources.SharedProperty = 17;
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
			const string expectedValue = "043";
			string actualValue;
			using (var synchronizeTest = new SynhronizeTest<Class1>())
			{
				synchronizeTest.Lock();
				Resources.SharedProperty = 43;
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
			const string expectedValue = "139";
			string actualValue;
			using (var synchronizeTest = new SynhronizeTest<Class1>())
			{
				synchronizeTest.Lock();
				Resources.SharedProperty = 139;
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

