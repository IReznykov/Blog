using WpfApplication.Models;

namespace WpfApplication.ViewModels
{
	public class DesignMainViewModel : IMainViewModel
	{
		public ISettings Settings { get; } = null;
	}
}
