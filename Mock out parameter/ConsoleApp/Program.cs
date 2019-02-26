using System;

namespace ConsoleApp
{
	internal static class Program
	{
		public static void Main(string[] args)
		{
			const string nullValue = "null";
			var class1 = new Class1(new Service());
			var inputValues = new[] { null, " some ", "long     value"};
			foreach (var inputValue in inputValues)
			{
				var outputValue = class1.ProcessValue(inputValue);
				Console.WriteLine($"Input ({inputValue ?? nullValue}) produces output ({outputValue ?? nullValue})");
			}
		}
	}
}
