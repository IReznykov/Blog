using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Ikc5.Prism.Settings.Logging;
using Microsoft.Practices.Unity;
using Prism.Logging;
using Application = System.Windows.Application;

namespace WpfApplication
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			ILoggerFacade logger = null;
			try
			{
				base.OnStartup(e);
				// create and launch bootstrapper, but main window is not shown
				var bootstrapper = new Bootstrapper();
				bootstrapper.Run();
				logger = bootstrapper.Container.Resolve<ILoggerFacade>();

				// parse input arguments
				logger?.Log($"Start parameters: {string.Join("; ", e.Args)}", Category.Info);
			}
			catch (Exception ex)
			{
				logger?.Exception(ex);
			}
		}
	}
}
