using System.Drawing;
using System.Windows.Media;
using Ikc5.ScreenSaver.Common.Models.Types;
using Ikc5.ScreenSaver.SecondModule.Models;
using Color = System.Windows.Media.Color;

namespace Ikc5.ScreenSaver.SecondModule.ViewModels
{
	internal class DesignSettings : ISettings
	{
		public int CellWidth { get; set; } = 10;
		public int IterationDelay { get; set; } = 1000;
		public Color StartColor { get; set; } = Colors.Yellow;
		public Color FinishColor { get; set; } = Colors.Fuchsia;
		public Color BorderColor { get; set; } = Colors.Transparent;
		public AnimationType AnimationType { get; set; } = AnimationType.Fade;
		public int AnimationDelay { get; set; } = 500;
	}
}
