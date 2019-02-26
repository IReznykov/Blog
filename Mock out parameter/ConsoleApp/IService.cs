using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
	public interface IService
	{
		void ProcessValue(string inputValue, out string outputValue);
	}
}
