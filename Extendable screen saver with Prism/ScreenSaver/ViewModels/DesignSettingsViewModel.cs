using System.Collections;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Media;
using Ikc5.ScreenSaver.Common.Models.Types;
using Color = System.Windows.Media.Color;

namespace Ikc5.ScreenSaver.ViewModels
{
	public class DesignSettingsViewModel : ISettingsViewModel
	{
		public DesignSettingsViewModel()
		{
			ModuleNameCollection = new ObservableCollection<string>(new[] { "Module1", "Module2", "Module3" });
		}

		public ObservableCollection<string> ModuleNameCollection { get; }
		public string ModuleName { get; set; } = "Module1";
		public SecondaryMonitorType SecondaryMonitorType { get; set; } = SecondaryMonitorType.Empty;
		public Color BackgroundColor { get; set; } = Colors.Black;
	}
}
