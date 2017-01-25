using System.Windows.Media;
using Ikc5.ScreenSaver.Common.Models.Types;
using Color = System.Windows.Media.Color;

namespace Ikc5.ScreenSaver.SecondModule.ViewModels
{
	internal class DesignSettingsViewModel : ISettingsViewModel
	{
		public int CellWidth { get; set; } = 10;
		public int IterationDelay { get; set; } = 1000;

		public Color StartColor { get; set; } = Colors.YellowGreen;
		public Color FinishColor { get; set; } = Colors.Olive;
		public Color BorderColor { get; set; } = Colors.DarkOliveGreen;
		public AnimationType AnimationType { get; set; } = AnimationType.None;
		public int AnimationDelay { get; set; } = 100;
	}
}
