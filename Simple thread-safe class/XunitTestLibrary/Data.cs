using System;

namespace XunitTestLibrary
{
	public class Data
	{
		private bool _state;
		private int _count;

		public bool State
		{
			get { return _state; }
			set
			{
				if (_state == value)
					return;

				_state = value;
				Count += (_state ? 1 : -1);
			}
		}

		public int Count
		{
			get { return _count; }
			set
			{
				if (value < 0)
					throw new ArgumentOutOfRangeException(nameof(Count), "Count should be positive");
				_count = value;
			}
		}
	}
}
