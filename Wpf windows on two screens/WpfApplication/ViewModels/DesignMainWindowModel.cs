using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Input;
using System.Windows.Media;
using WpfApplication.Models;
using Color = System.Windows.Media.Color;

namespace WpfApplication.ViewModels
{
	public class DesignMainWindowModel : IMainWindowModel
	{
		public DesignMainWindowModel()
		{
			Rectangles = new ObservableCollection<ScreenRectangle>(new []
			{
				new ScreenRectangle("primary", 0, 0, 300, 200),
				new ScreenRectangle("secondary", 300, 0, 600, 400)
			});
		}
		public Color BackgroundColor { get; } = Colors.LightGray;
		public double ViewWidth { get; set; } = 300;
		public double ViewHeight { get; set; } = 200;
		public ObservableCollection<ScreenRectangle> Rectangles { get; }
	}
}
