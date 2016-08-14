using System;

namespace XunitTestLibrary
{
	public class ThreadSafeData
	{
		private readonly object _lockObject = new object();

		private bool _state;
		private int _count;

		public bool State
		{
			get
			{
				lock (_lockObject)
				{
					return _state;
				}
			}
			set
			{
				lock (_lockObject)
				{
					if (_state == value)
						return;

					_state = value;
					Count += (_state ? 1 : -1);
				}
			}
		}

		public int Count
		{
			get
			{
				lock (_lockObject)
				{
					return _count;
				}
			}
			set
			{
				lock (_lockObject)
				{
					if (value < 0)
						throw new ArgumentOutOfRangeException(nameof(Count), "Count should be positive");
					_count = value;
				}
			}
		}

		public int AddCount(int delta)
		{
			lock (_lockObject)
			{
				Count += delta;
				return Count;
			}
		}

	}
}
