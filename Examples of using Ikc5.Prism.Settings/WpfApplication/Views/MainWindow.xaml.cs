using System.Windows;
using Microsoft.Practices.Unity;
using WpfApplication.ViewModels;

namespace WpfApplication.Views
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private readonly IUnityContainer _container;

		public MainWindow(IUnityContainer container)
		{
			_container = container;
			DataContext = _container.Resolve<IMainViewModel>();
			InitializeComponent();
		}

		private void Settings_OnClick(object sender, RoutedEventArgs e)
		{
			var settingsWindow = _container.Resolve<SettingsWindow>();
			settingsWindow.Owner = this;
			settingsWindow.ShowDialog();
		}

		//protected override void OnClosing(CancelEventArgs e)
		//{
		//	// serialize settings
		//	var serializedData = new object();

		//	var userSettingsService = _container.Resolve<IUserSettingsService>();
		//	if (userSettingsService.SerializeCommand.CanExecute(serializedData))
		//		userSettingsService.SerializeCommand.Execute(serializedData);

		//	base.OnClosing(e);
		//}
	}
}
