using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WpfApplication.ViewModels
{
	public  class DesignColumnChartViewModel : IColumnChartViewModel
	{
		public IReadOnlyList<Tuple<string, int>> CountList =>
			new List<Tuple<string, int>>(new []
			{
				new Tuple<string, int>("1", 5),
				new Tuple<string, int>("2", 10),
				new Tuple<string, int>("3", 7)
			});
	}
}
