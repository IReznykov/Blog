using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Xunit.Sdk;

namespace XunitTestLibrary
{
	public class DataTests
	{
		public readonly string ExceptionMessage = $"State should be positive{Environment.NewLine}Parameter name: {nameof(Data.State)}";

		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		public void Data_ShouldAccept_NonNegativeValue(int state)
		{
			Data data = null;
			var exception = Record.Exception(() =>
			{
				data = new Data();
				data.State = state;
			});

			data.Should().NotBeNull();
			exception.Should().BeNull();
		}

		[Theory]
		[InlineData(-1)]
		public void Data_ShouldThrow_ExceptionOnNegativeValue(int state)
		{
			Data data = null;
			var exception = Record.Exception(() =>
			{
				data = new Data();
				data.State = state;
			});

			data.Should().NotBeNull();
			exception.Should().NotBeNull();
			exception.Message.Should().Be(ExceptionMessage);
		}

		[Theory]
		[InlineData(-1)]
		public void Data_ShouldThrow_ExceptionOnNegativeValueAndReturnNullObject(int state)
		{
			Data data = null;
			Action task = () =>
				{
					data = new Data
					{
						State = state
					};
				};

			var exception = Record.Exception(task);
			data.Should().BeNull();
			exception.Should().NotBeNull();
			exception.Message.Should().Be(ExceptionMessage);
		}

		[Fact]
		public async void Data_ShouldAccept_NonNegativeValueInAsync()
		{
			var data = new Data();
			await Task.Run(() =>
			{
				for (var pos = 5; pos >= 0; pos--)
				{
					data.State = pos;
				}
			});

			data.Should().NotBeNull();
			data.State.Should().Be(0);
		}

		[Fact]
		public void Data_ShouldNotThrow_ExceptionOnNonNegativeValueInAsync()
		{
			var data = new Data();
			var task = Task.Run(() =>
						{
							for (var pos = 5; pos >= 0; pos--)
							{
								data.State = pos;
							}
						});

			var taskException = Record.ExceptionAsync(async () => await task);
			data.Should().NotBeNull();
			data.State.Should().Be(0);
			taskException.Should().NotBeNull();
			taskException.Result.Should().BeNull();
		}

		[Fact]
		public void Data_ShouldThrow_ExceptionOnNegativeValueInAsync()
		{
			var data = new Data();
			var task = Task.Run(() =>
						{
							for (var pos = 1; pos >= -2; pos--)
							{
								data.State = pos;
							}
						});

			var exception = Record.ExceptionAsync(async () => await task);

			data.Should().NotBeNull();
			data.State.Should().Be(0);
			exception.Should().NotBeNull();
			exception.Result.Should().NotBeNull();
			exception.Result.Message.Should().Be(ExceptionMessage);
		}

		[Fact]
		public void Data_ShouldThrow_ExceptionOnNegativeStateInTwoAsyncTasks()
		{
			var data = new Data();
			var tasks = new Task[]
				{
			Task.Run(() =>
				{
					for (var pos = 0; pos < 10; pos++)
					{
						data.State += 1;
					}
				}),
			Task.Run(() =>
				{
					for (var pos = 0; pos < 20; pos++)
					{
						data.State -= 1;
					}
				}),
				};

			var exception = Record.ExceptionAsync(async () => await Task.WhenAll(tasks));
			data.Should().NotBeNull();
			exception.Should().NotBeNull();
			exception.Result.Should().NotBeNull();
			exception.Result.Message.Should().Be(ExceptionMessage);
		}
	}
}
