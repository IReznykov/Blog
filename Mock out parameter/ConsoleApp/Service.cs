using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
	public class Service : IService
	{
		private const int resultLength = 10;

		/// <summary>
		/// Return trimmed value of input string no longer than <see cref="resultLength"/>.
		/// </summary>
		/// <param name="inputValue">Input value, could be null.</param>
		/// <param name="outputValue">Output value.</param>
		public void ProcessValue(string inputValue, out string outputValue)
		{
			if (string.IsNullOrEmpty(inputValue))
			{
				outputValue = null;
				return;
			}
			var result = inputValue.Trim(new[] { ' ', '\t' });
			if (result.Length > resultLength)
				outputValue = result.Substring(0, resultLength);
			else
				outputValue = result;
		}
	}
}
