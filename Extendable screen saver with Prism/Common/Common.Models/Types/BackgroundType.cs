using System.ComponentModel;
using Ikc5.TypeLibrary;

namespace Ikc5.ScreenSaver.Common.Models.Types
{
	[TypeConverter(typeof(EnumDescriptionTypeConverter))]
	public enum BackgroundType
	{
		[Description("Solid color")]
		SolidColor,

		[Description("Desktop background")]
		Desktop,

		[Description("Picture")]
		// code could taken from http://www.codeproject.com/Articles/30078/WPF-A-D-screensaver-written-in-WPF
		Picture
	}
}
