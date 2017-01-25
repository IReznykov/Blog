using System.Drawing;
using System.Windows.Media;
using Ikc5.ScreenSaver.Common.Models.Types;
using Ikc5.ScreenSaver.FirstModule.Models;
using Color = System.Windows.Media.Color;

namespace Ikc5.ScreenSaver.FirstModule.ViewModels
{
	internal class DesignSettings : ISettings
	{
		public int CellWidth { get; set; } = 40;
		public int CellHeight { get; set; } = 40;
		public int IterationDelay { get; set; } = 1000;
		public Color StartColor { get; set; } = Colors.BlueViolet;
		public Color FinishColor { get; set; } = Colors.DarkBlue;
		public Color BorderColor { get; set; } = Colors.MidnightBlue;
		public AnimationType AnimationType { get; set; } = AnimationType.Fade;
		public int AnimationDelay { get; set; } = 500;
	}
}
