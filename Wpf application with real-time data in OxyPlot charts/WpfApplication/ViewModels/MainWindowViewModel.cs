using Ikc5.TypeLibrary;
using Microsoft.Practices.Unity;
using WpfApplication.Models;

namespace WpfApplication.ViewModels
{
	public class MainWindowViewModel : IMainWindowViewModel
	{
		public MainWindowViewModel()
		{
		}

		#region IMainWindowViewModel

		[Dependency]
		public ILineChartViewModel LineChartViewModel { get; set; }

		[Dependency]
		public IColumnChartViewModel ColumnChartViewModel { get; set; }

		#endregion
	}
}