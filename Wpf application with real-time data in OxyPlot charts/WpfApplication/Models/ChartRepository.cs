using System;
using System.Collections.Generic;
using System.Linq;
using Ikc5.TypeLibrary;

namespace WpfApplication.Models
{
	public class ChartRepository : BaseNotifyPropertyChanged, IChartRepository
	{
		#region Lists

		private const int LineCount = 100;
		private readonly List<int> _lineCountList = new List<int>(LineCount);

		private const int ColumnCount = 50;
		private readonly IDictionary<int, int> _columnCountDictionary = new Dictionary<int, int>(ColumnCount);

		#endregion

		#region IChartRepository

		public IReadOnlyList<int> LineCountList => _lineCountList;

		public IReadOnlyList<int> ColumnCountList => GetColumnCountList().ToList();

		public void AddLineCount(int newValue)
		{
			_lineCountList.Add(newValue);
			if (_lineCountList.Count > LineCount)
				_lineCountList.RemoveAt(0);

			OnPropertyChanged(nameof(LineCountList));
		}

		public void AddColumnCount(int index, int newValue)
		{
			index = Math.Max(0, Math.Min(index, ColumnCount - 1));
			_columnCountDictionary[index] = newValue;

			OnPropertyChanged(nameof(ColumnCountList));
		}

		private IEnumerable<int> GetColumnCountList()
		{
			var maxIndex = _columnCountDictionary.Count == 0
				? -1
				: _columnCountDictionary.Keys.Max();
			for (var index = 0; index <= maxIndex; index++)
			{
				yield return _columnCountDictionary.ContainsKey(index) ?
								_columnCountDictionary[index]
								: 0;
			}
		}

		#endregion
	}
}
