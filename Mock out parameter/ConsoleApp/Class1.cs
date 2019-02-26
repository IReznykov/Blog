namespace ConsoleApp
{
	public class Class1
	{
		private readonly IService _service;

		public Class1(IService service)
		{
			_service = service;
		}

		/// <summary>
		/// Return trimmed value of input string processed by service.
		/// </summary>
		/// <param name="inputValue">Input value, could be null.</param>
		/// <param name="outputValue">Output value.</param>
		public string ProcessValue(string inputValue)
		{
			_service.ProcessValue(inputValue, out string outputValue);
			return outputValue;
		}
	}
}
