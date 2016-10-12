using Ikc5.TypeLibrary;
using Prism.Mvvm;
using WpfApplication.Models;

namespace WpfApplication.ViewModels
{
	public class MainViewModel : BindableBase, IMainViewModel
	{
		public MainViewModel(ISettings settings)
		{
			settings.ThrowIfNull(nameof(settings));
			Settings = settings;
		}

		#region IMainViewModel

		private ISettings _settings;

		/// <summary>
		/// Main window settings.
		/// </summary>
		public ISettings Settings
		{
			get { return _settings; }
			private set { SetProperty(ref _settings, value); }
		}

		#endregion IMainViewModel
	}
}