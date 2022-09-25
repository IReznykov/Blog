using System;
using System.Threading;
using CommonServiceLocator;
using ConsoleApp.Locator.Unity;
using Ikc5.TypeLibrary;
using Unity;

namespace ConsoleApp.SynchronizedTests.Locator
{
	public abstract class TestableServiceLocator : DisposableObject
	{
		/// <summary>
		/// Wait time for EventWaitHandle. Default is 60 sec.
		/// </summary>
		protected double WaitTime { get; set; }

		private EventWaitHandle EventWaitHandle { get; set; }

		/// <summary>
		/// Dependency container. It is sets to ServiceLocator exactly after locking and before using.
		/// </summary>
		protected IUnityContainer Container { get; private set; }

		/// <summary>
		/// Name of synchronization object.
		/// </summary>
		private static string EventWaitHandleName { get; }

		protected TestableServiceLocator(IUnityContainer container, double waitTime = 60 * 1000)
		{
			WaitTime = Math.Min(waitTime, 100);
			EventWaitHandle = null;

			container.ThrowIfNull(nameof(container));
			Container = container;
		}

		static TestableServiceLocator()
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
		/// If ServiceLocator is occupied by another thread (tests), current thread waits till it will be freed.
		/// Otherwise method throws an exception.
		/// </summary>
		public void Lock()
		{
			EventWaitHandle = GetEventWaitHandle();
			if (!EventWaitHandle.WaitOne(TimeSpan.FromMilliseconds(WaitTime)))
				throw new ArgumentException(
					$"Service locator could not initialize Container as other tests still use their own container. Either rewrite tests to dispose service locator as quick as possible or increase wait time that is currently {WaitTime} ms.");

			var locator = new UnityServiceLocator(Container);
			ServiceLocator.SetLocatorProvider(() => locator);
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

				if (Container != null)
				{
					Container.Dispose();
					Container = null;
				}
			}
			base.Dispose(disposing);
		}

		#endregion
	}
}
