using System.Windows.Media;

namespace Part1Module.ViewModels
{
	public class DesignSettingsViewModel : ISettingsViewModel
	{
		public string Name { get; set; } = "Design";
		public Color BackgroundColor { get; set; } = Colors.DarkSeaGreen;
		public bool IsChecked { get; set; } = true;
		public int Count { get; set; } = 100;
	}
}
