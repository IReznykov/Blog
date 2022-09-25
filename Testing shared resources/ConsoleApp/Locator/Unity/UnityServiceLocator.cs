using System;
using System.Collections.Generic;
using CommonServiceLocator;
using Unity;

namespace ConsoleApp.Locator.Unity
{
	/// <summary>
	/// Implementation is taken from Prism Library and simplified.
	/// Defines a <see cref="IUnityContainer"/> adapter for the <see cref="IServiceLocator"/> interface.
	/// </summary>
	public class UnityServiceLocator : ServiceLocatorImplBase
	{
		private readonly IUnityContainer _unityContainer;

		/// <summary>
		/// Initializes a new instance of <see cref="UnityServiceLocator"/>.
		/// </summary>
		/// <param name="unityContainer">The <see cref="IUnityContainer"/> that will be used
		/// by the <see cref="DoGetInstance"/> and <see cref="DoGetAllInstances"/> methods.</param>
		public UnityServiceLocator(IUnityContainer unityContainer)
		{
			_unityContainer = unityContainer;
		}

		/// <summary>
		/// Resolves the instance of the requested service.
		/// </summary>
		/// <param name="serviceType">Type of instance requested.</param>
		/// <param name="key">Name of registered service you want. May be null.</param>
		/// <returns>The requested service instance.</returns>
		protected override object DoGetInstance(Type serviceType, string key)
		{
			return _unityContainer.Resolve(serviceType, key);
		}

		/// <summary>
		/// Resolves all the instances of the requested service.
		/// </summary>
		/// <param name="serviceType">Type of service requested.</param>
		/// <returns>Sequence of service instance objects.</returns>
		protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
		{
			return _unityContainer.ResolveAll(serviceType);
		}
	}
}
