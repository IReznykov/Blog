using System.ComponentModel;
using Ikc5.TypeLibrary;

namespace Ikc5.ScreenSaver.Common.Models.Types
{
	[TypeConverter(typeof(EnumDescriptionTypeConverter))]
	public enum LaunchType
	{
		[Description("No parameters")]
		Default = 0,

		[Description("\\s, Show the screen saver")]
		Show,

		[Description("\\c, Configure settings")]
		Configure,

		[Description("\\p, Show in preview mode")]
		Preview
	}
}
