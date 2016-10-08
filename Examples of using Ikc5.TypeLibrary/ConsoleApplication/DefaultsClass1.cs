using System.ComponentModel;
using Ikc5.TypeLibrary;

namespace ConsoleApplication
{
	internal class DefaultsClass1
	{
		public DefaultsClass1()
		{
			// set value of Name property to default value "Instance1"
			this.SetDefaultValue<string>(nameof(Name));
			// set value of Index property to default value, 1
			this.SetDefaultValue<int>(nameof(Index));
			// do nothing, as DefaultValue attribute is not used for Checked property
			this.SetDefaultValue<bool>(nameof(Checked));
		}

		[DefaultValue("Instance1")]
		public string Name { get; set; }

		private int _index;

		[DefaultValue(1)]
		public int Index { get; set; }

		private bool _checked;

		public bool Checked { get; set; }

		public override string ToString()
		{
			return $"Index {Index}, Name \"{Name}\", Checked " + (Checked ? "On" : "Off");
		}
	}
}
