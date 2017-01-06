using System;
using System.Windows.Controls;
using Microsoft.Practices.Unity;
using OxyPlot.Wpf;
using WpfApplication.ViewModels;

namespace WpfApplication.Views
{
	/// <summary>
	/// Interaction logic for ChartView.xaml
	/// </summary>
	public partial class ColumnChartView : UserControl
	{
		public ColumnChartView()
		{
			InitializeComponent();

			IndexAxis.LabelFormatter = (index =>
			{
				var ratio = (int)Math.Round(CountSeries.Items.Count / 10.0, 0);
				var label = (int)index + 1;
				return (ratio <= 1 || label % ratio == 1) ? label.ToString("D") : string.Empty;
			});
		}
	}
}
