namespace ConsoleApp.Resource
{
	public class Class1
	{
		public Class1()
		{ }

		/// <summary>
		/// Method get and process static property.
		/// </summary>
		/// <returns></returns>
		public string GetValues()
		{
			return Resources.SharedProperty.ToString("D3");
		}
	}
}
