using System.ComponentModel;
using Ikc5.TypeLibrary;

namespace Ikc5.ScreenSaver.Common.Models.Types
{
	[TypeConverter(typeof(EnumDescriptionTypeConverter))]
	public enum SecondaryMonitorType
	{
		[Description("Empty window")]
		Empty,

		[Description("Saver window")]
		MainWindow,
	}
}
