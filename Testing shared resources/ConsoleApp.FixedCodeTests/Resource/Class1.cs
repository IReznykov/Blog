namespace ConsoleApp.FixedCodeTests.Resource
{
	public class Class1
	{
		private readonly IResourcesWrapper _resourcesWrapper;

		// dependency injection in constructor
		public Class1(IResourcesWrapper resourcesWrapper)
		{
			_resourcesWrapper = resourcesWrapper;
		}

		public string GetValues()
		{
			// replace call to static property by the call to property of the instance
			return _resourcesWrapper.SharedProperty.ToString("D3");
		}
	}
}
