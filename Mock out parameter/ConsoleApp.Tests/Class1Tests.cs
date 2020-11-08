using Moq;
using FluentAssertions;
using Xunit;

namespace ConsoleApp.Tests
{
	public class Class1Tests
	{
		[Theory]
		[InlineData(null)]
		[InlineData("short")]
		[InlineData("\t\t\ttabbed\t\t")]
		[InlineData("long long value")]
		public void Class1_Return_ConstValueWithoutCallback(string inputValue)
		{
			var expectedValue = "Expected value";
			var service = new Mock<IService>();
			service
				.Setup(mock => mock.ProcessValue(It.IsAny<string>(), out expectedValue))
				.Verifiable();

			// tested routine
			var class1 = new Class1(service.Object);
			var actualValue = class1.ProcessValue(inputValue);

			// assertion
			actualValue.Should().NotBeNull();
			actualValue.Should().Be(expectedValue);

			service.Verify();
		}

		[Theory]
		[InlineData(null)]
		[InlineData("short")]
		[InlineData("\t\t\ttabbed\t\t")]
		[InlineData("long long value")]
		public void Class1_Return_TheSameValueWithoutCallback(string inputValue)
		{
			var expectedValue = $"Output {inputValue}";
			var service = new Mock<IService>();
			service
				.Setup(mock => mock.ProcessValue(It.IsAny<string>(), out expectedValue))
				.Verifiable();

			// tested routine
			var class1 = new Class1(service.Object);
			var actualValue = class1.ProcessValue(inputValue);

			// assertion
			actualValue.Should().NotBeNull();
			actualValue.Should().Be(expectedValue);

			service.Verify();
		}

		private delegate void ServiceProcessValue(string inputValue, out string outputValue);

		[Theory]
		[InlineData(null)]
		[InlineData("short")]
		[InlineData("\t\t\ttabbed\t\t")]
		[InlineData("long long value")]
		public void Class1_Return_NewValueWithCallback(string inputValue)
		{
			string actualInputValue = null;

			const string outputValue = "Inner value";
			var expectedValue = "Not used value";
			var service = new Mock<IService>();
			service
				.Setup(mock => mock.ProcessValue(It.IsAny<string>(), out expectedValue))
				.Callback(new ServiceProcessValue(
					(string input, out string output) =>
					{
						actualInputValue = input;
						output = outputValue;
					}))
				.Verifiable();

			// tested routine
			var class1 = new Class1(service.Object);
			var actualValue = class1.ProcessValue(inputValue);

			// assertion
			actualValue.Should().NotBeNull();
			actualValue.Should().Be(outputValue);

			actualInputValue.Should().Be(inputValue);

			service.Verify();
		}
	}
}
