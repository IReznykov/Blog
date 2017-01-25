using Ikc5.ScreenSaver.Models;
using Ikc5.ScreenSaver.ViewModels;
using Ikc5.TypeLibrary;
using Prism.Mvvm;

namespace Ikc5.ScreenSaver.Views
{
	public class EmptyWindowModel : BindableBase, IEmptyWindowModel
	{
		public EmptyWindowModel(ISettings settings)
		{
			settings.ThrowIfNull(nameof(settings));
			Settings = settings;
		}

		#region IEmptyWindowModel

		private ISettings _settings;

		/// <summary>
		/// Main window settings.
		/// </summary>
		public ISettings Settings
		{
			get { return _settings; }
			private set { SetProperty(ref _settings, value); }
		}

		#endregion IEmptyWindowModel
	}
}