using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OxyPlot;

namespace WpfApplication.ViewModels
{
	public class DesignLineChartViewModel : ILineChartViewModel
	{
		public DesignLineChartViewModel()
		{
			CountList = new List<DataPoint>(new[] { new DataPoint(0,  0), new DataPoint(1, 3), new DataPoint(2, 6), new DataPoint(3, 5), });
		}

		public IReadOnlyList<DataPoint> CountList { get; }
	}
}
