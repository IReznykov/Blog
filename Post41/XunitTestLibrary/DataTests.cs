using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace XunitTestLibrary
{
	public class DataTests
	{
		public readonly string ExceptionMessage = $"Count should be positive{Environment.NewLine}Parameter name: {nameof(Data.Count)}";

		[Fact]
		public void ThreadSafeData_ShouldNotThrow_ExceptionInTwoAsyncTasks()
		{
			var data = new ThreadSafeData();
			var tasks = new[]
				{
			Task.Run(() =>
				{
					for (var pos = 0; pos < 500000; pos++)
					{
						data.State = (pos % 2 == 0);
					}
				}),
			Task.Run(() =>
				{
					for (var pos = 0; pos < 500000; pos++)
					{
						data.AddCount (pos % 2 == 0 ? 1 : -1);
					}
			}),
		};

			var exception = Record.ExceptionAsync(async () => await Task.WhenAll(tasks));
			data.Should().NotBeNull();
			exception.Should().NotBeNull();
			exception.Result.Should().BeNull();
		}

		[Fact]
		public void ThreadSafeData_ShouldThrow_ExceptionInTwoAsyncTasks()
		{
			var data = new ThreadSafeData();
			var tasks = new[]
				{
					Task.Run(() =>
						{
							for (var pos = 0; pos < 500000; pos++)
							{
								data.State = (pos % 2 == 0);
							}
						}),
					Task.Run(() =>
						{
							for (var pos = 0; pos < 500000; pos++)
							{
								data.Count += (pos % 2 == 0 ? 1 : -1);
							}
					}),
				};

			var exception = Record.ExceptionAsync(async () => await Task.WhenAll(tasks));
			data.Should().NotBeNull();
			exception.Should().NotBeNull();
			exception.Result.Should().NotBeNull();
			exception.Result.Message.Should().Be(ExceptionMessage);
		}

		[Fact]
		public void Data_ShouldThrow_ExceptionOnNegativeStateInTwoAsyncTasks()
		{
			var data = new Data();
			var tasks = new[]
				{
			Task.Run(() =>
				{
					for (var pos = 0; pos < 500000; pos++)
					{
						data.State = (pos % 2 == 0);
					}
				}),
			Task.Run(() =>
				{
					for (var pos = 0; pos < 500000; pos++)
					{
						data.Count += (pos % 2 == 0 ? 1 : -1);
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
