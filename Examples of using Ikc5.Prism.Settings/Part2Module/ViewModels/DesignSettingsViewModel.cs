using System.Windows.Media;

namespace Part2Module.ViewModels
{
	public class DesignSettingsViewModel : ISettingsViewModel
	{
		public string Name { get; set; }
		public Color BackgroundColor { get; set; } = Colors.DeepSkyBlue;
		public int Width { get; set; } = 10;
		public int Height { get; set; } = 10;
	}
}
