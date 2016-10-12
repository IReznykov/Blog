using System.Windows.Media;

namespace WpfApplication.ViewModels
{
	public class DesignSettingsViewModel : ISettingsViewModel
	{
		public string Name { get; set; }
		public Color BackgroundColor { get; set; } = Colors.DarkGray;
	}
}
