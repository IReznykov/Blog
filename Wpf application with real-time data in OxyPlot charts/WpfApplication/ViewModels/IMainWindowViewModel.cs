
namespace WpfApplication.ViewModels
{
	public interface IMainWindowViewModel
	{
		ILineChartViewModel LineChartViewModel { get; }
		IColumnChartViewModel ColumnChartViewModel { get; }
	}
}
