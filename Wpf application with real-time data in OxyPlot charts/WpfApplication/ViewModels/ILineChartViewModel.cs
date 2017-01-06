using System.Collections.Generic;
using OxyPlot;

namespace WpfApplication.ViewModels
{
	public interface ILineChartViewModel
	{
		IReadOnlyList<DataPoint> CountList { get; }
	}
}