using System.ComponentModel;
using Ikc5.TypeLibrary;

namespace Ikc5.ScreenSaver.Common.Models.Types
{
	[TypeConverter(typeof(EnumDescriptionTypeConverter))]
	public enum AnimationType : byte
	{
		/// <summary>
		/// No animation
		/// </summary>
		[Description("No animation")]
		None = 0,

		/// <summary>
		/// Fade in / Fade out
		/// </summary>
		[Description("Fade")]
		Fade
	}

}
