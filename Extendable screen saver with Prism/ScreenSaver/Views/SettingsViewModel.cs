using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using Ikc5.Prism.Settings;
using Ikc5.Prism.Settings.ViewModels;
using Ikc5.ScreenSaver.Common.Models.Types;
using Ikc5.ScreenSaver.Models;
using Ikc5.ScreenSaver.ViewModels;
using Prism.Modularity;
using Color = System.Windows.Media.Color;

namespace Ikc5.ScreenSaver.Views
{
	/// <summary>
	/// View model for module's settings
	/// https://ireznykov.com/2016/10/16/examples-of-using-ikc5-prism-settings/
	/// https://ireznykov.com/2016/10/15/nuget-package-ikc5-prism-settings/
	/// </summary>
	public class SettingsViewModel : UserSettingsViewModel<ISettings>, ISettingsViewModel
	{
		public SettingsViewModel(ISettings settingsModel, IUserSettingsService userSettingsService, IModuleCatalog moduleCatalog)
			: base(settingsModel as IUserSettings, userSettingsService)
		{
			ModuleNameCollection = new ObservableCollection<string>(moduleCatalog.Modules.Select(module => module.ModuleName));
			if (string.IsNullOrEmpty(ModuleName))
				ModuleName = ModuleNameCollection[0];
		}

		#region ISettings

		private ObservableCollection<string> _moduleNameCollection;
		private string _moduleName;
		private SecondaryMonitorType _secondaryMonitorType;
		private Color _backgroundColor;

		#endregion ISettings

		#region ISettingsViewModel

		/// <summary>
		/// Collection of module names.
		/// </summary>
		public ObservableCollection<string> ModuleNameCollection
		{
			get { return _moduleNameCollection; }
			set { SetProperty(ref _moduleNameCollection, value); }
		}

		/// <summary>
		/// Name of active module.
		/// </summary>
		public string ModuleName
		{
			get { return _moduleName; }
			set { SetProperty(ref _moduleName, value); }
		}

		/// <summary>
		/// Type of the window at secondary monitors.
		/// </summary>
		public SecondaryMonitorType SecondaryMonitorType
		{
			get { return _secondaryMonitorType; }
			set { SetProperty(ref _secondaryMonitorType, value); }
		}

		/// <summary>
		/// Color of the background.
		/// </summary>
		public Color BackgroundColor
		{
			get { return _backgroundColor; }
			set { SetProperty(ref _backgroundColor, value); }
		}

		#endregion ISettingsViewModel

	}
}