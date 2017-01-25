using System.Windows.Media;
using Ikc5.ScreenSaver.Common.Models.Types;
using Color = System.Windows.Media.Color;

namespace Ikc5.ScreenSaver.FirstModule.ViewModels
{
	internal class DesignSettingsViewModel : ISettingsViewModel
	{
		public int CellWidth { get; set; } = 25;
		public int CellHeight { get; set; } = 25;
		public int IterationDelay { get; set; } = 1000;

		public Color StartColor { get; set; } = Colors.AliceBlue;
		public Color FinishColor { get; set; } = Colors.DarkBlue;
		public Color BorderColor { get; set; } = Colors.DarkGray;
		public AnimationType AnimationType { get; set; } = AnimationType.None;
		public int AnimationDelay { get; set; } = 100;
	}
}
