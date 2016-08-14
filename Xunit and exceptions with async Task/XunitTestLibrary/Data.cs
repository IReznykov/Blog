using System;

namespace XunitTestLibrary
{
	public class Data
	{
		private readonly object _lockObject = new object();

		private int _state;

		public int State
		{
			get { return _state; }
			set
			{
				lock (_lockObject)
				{
					if (value < 0)
						throw new ArgumentOutOfRangeException(nameof(State), "State should be positive");
					_state = value;
					// some inner changes
				}
			}
		}
	}
}
