using System;

namespace ConsoleApplication
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine(new DefaultsClass1());
			Console.WriteLine(new DefaultsClass2());
			Console.WriteLine(new DefaultsClass3());

			// wait for some input
			Console.ReadKey();
		}
	}
}
