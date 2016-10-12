using WpfApplication.Models;

namespace WpfApplication.ViewModels
{
	public interface IMainViewModel
	{
		/// <summary>
		/// Main window settings.
		/// </summary>
		ISettings Settings { get; }
	}
}