using ConsoleApp.Resource;

namespace ConsoleApp.FixedCodeTests.Resource
{
	public class ResourcesWrapper : IResourcesWrapper
	{
		private readonly Resources _resources = new Resources();

		public string InstanceProperty
		{
			get => _resources.InstanceProperty;
			set => _resources.InstanceProperty = value;
		}

		public int SharedProperty
		{
			get => Resources.SharedProperty;
			set => Resources.SharedProperty = value;
		}
	}
}
