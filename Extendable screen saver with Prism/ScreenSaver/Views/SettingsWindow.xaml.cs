using System.Windows;
using System.Windows.Controls;
using Ikc5.Prism.Settings;
using Ikc5.TypeLibrary;
using Prism.Modularity;
using Prism.Regions;

namespace Ikc5.ScreenSaver.Views
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class SettingsWindow : Window
	{
		private readonly IUserSettingsService _userSettingsService;

		public SettingsWindow(IUserSettingsService userSettingsService, IModuleCatalog moduleCatalog)
		{
			userSettingsService.ThrowIfNull(nameof(userSettingsService));
			_userSettingsService = userSettingsService;

			InitializeComponent();
			CommandGrid.DataContext = _userSettingsService;

			if (ModuleSettingsTabControl.Items.Count == 0)
			{
				foreach (var module in moduleCatalog.Modules)
				{
					var contentControl = new ContentControl
					{
						Margin = new Thickness(5, 5, 5, 5)
					};
					contentControl.SetValue(RegionManager.RegionNameProperty, $"{module.ModuleName}{RegionNames.ModuleSettingsRegion}");

					var tabItem = new TabItem
					{
						Header = module.ModuleName,
						Name = $"{module.ModuleName}TabItem",
						Content = contentControl
					};

					ModuleSettingsTabControl.Items.Add(tabItem);
				}
			}
		}

		private void CancelButtonClick(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void SaveButtonClick(object sender, RoutedEventArgs e)
		{
			// serialize settings
			var serializedData = new object();

			if (_userSettingsService.SaveCommand.CanExecute(serializedData))
			{
				_userSettingsService.SaveCommand.Execute(serializedData);
				if (_userSettingsService.SerializeCommand.CanExecute(serializedData))
					_userSettingsService.SerializeCommand.Execute(serializedData);
			}

			Close();
		}
	}
}
