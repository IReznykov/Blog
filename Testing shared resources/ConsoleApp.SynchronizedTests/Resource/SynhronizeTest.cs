using System;
using System.Threading;
using Ikc5.TypeLibrary;

namespace ConsoleApp.SynchronizedTests.Resource
{
	/// <summary>
	/// Generic type is used to synchronize tests that cover particular class.
	/// </summary>
	/// <typeparam name="T">The scope of synchronized tests.</typeparam>
	public class SynhronizeTest<T> : DisposableObject
	{
		/// <summary>
		/// Wait time for EventWaitHandle. Default is 60 sec.
		/// </summary>
		private double WaitTime { get; set; }

		private EventWaitHandle EventWaitHandle { get; set; }

		/// <summary>
		/// Name of synchronization object - one value per combination of type arguments.
		/// </summary>
		private static string EventWaitHandleName { get; }

		public SynhronizeTest(double waitTime = 60 * 1000)
		{
			WaitTime = Math.Max(waitTime, 100);
			EventWaitHandle = null;
		}

		static SynhronizeTest()
		{
			// set static unique name
			EventWaitHandleName = Guid.NewGuid().ToString("N");
		}

		private static EventWaitHandle GetEventWaitHandle()
		{
			if (EventWaitHandle.TryOpenExisting(EventWaitHandleName, out var eventWaitHandle))
				return eventWaitHandle;
			// TRUE to set signaled state that allows the first thread continue 
			return new EventWaitHandle(true, EventResetMode.AutoReset, EventWaitHandleName);
		}

		/// <summary>
		/// If test is occupied by another thread (tests), current thread waits till it be freed.
		/// Otherwise method throws an exception.
		/// </summary>
		public void Lock()
		{
			EventWaitHandle = GetEventWaitHandle();
			if (!EventWaitHandle.WaitOne(TimeSpan.FromMilliseconds(WaitTime)))
				throw new ArgumentException(
					$"Service locator could not initialize Container as other tests still use their own container. Either rewrite tests to dispose service locator as quick as possible or increase wait time that is currently {WaitTime} ms.");
		}

		/// <summary>
		/// Allows don't use IDisposable interface. It is added for demonstration purposes.
		/// </summary>
		public bool Unlock()
		{
			if (EventWaitHandle == null)
				return false;

			EventWaitHandle.Set();
			EventWaitHandle.Close();
			EventWaitHandle = null;
			return true;
		}

		#region IDisposable interface

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (EventWaitHandle != null)
				{
					EventWaitHandle.Set();
					EventWaitHandle.Close();
					EventWaitHandle = null;
				}
			}
			base.Dispose(disposing);
		}

		#endregion
	}

	//public static class StaticMock3Extensions
	//{
	//	public static StaticMock3<T2> SetAndLockStaticProperty<T1, T2>(this T1 classType, T2 value)
	//	{
	//		var staticMock3 = new StaticMock3<T2>(value);
	//		classType = value;

	//		return staticMock3;
	//	}
	//}
}
