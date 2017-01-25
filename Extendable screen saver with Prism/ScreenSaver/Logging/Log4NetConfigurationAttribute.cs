using System;
using System.IO;
using System.Reflection;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using log4net.Repository;
using log4net.Repository.Hierarchy;

namespace Ikc5.ScreenSaver.Logging
{
	/// <summary>
	/// Configuration attribute for log4net
	/// http://stackoverflow.com/questions/16336917/can-you-configure-log4net-in-code-instead-of-using-a-config-file
	/// </summary>
	[AttributeUsage(AttributeTargets.Assembly)]
	public class Log4NetConfigurationAttribute : ConfiguratorAttribute
	{
		public Log4NetConfigurationAttribute()
			: base(0)
		{
		}

		public override void Configure(Assembly sourceAssembly, ILoggerRepository targetRepository)
		{
			var hierarchy = (Hierarchy)targetRepository;	//LogManager.GetRepository();

			// in order to remove appenders from other sources 
			hierarchy.Root.RemoveAllAppenders();

			// add several appenders
			hierarchy.Root.AddAppender(GetRollingAppender(sourceAssembly));
			hierarchy.Root.AddAppender(GetConsoleAppender());

			hierarchy.Root.Level = Level.All;
			hierarchy.Configured = true;
		}

		private static PatternLayout GetPattern()
		{
			// don't use %logger, as it is intended for creating loggers each time,
			// and here we use singleton from UnityContainer
			var patternLayout = new PatternLayout
			{
				ConversionPattern = "%utcdate [%thread] %-5level - %message%newline"
			};
			patternLayout.ActivateOptions();
			return patternLayout;
		}

		private static IAppender GetRollingAppender(Assembly assembly)
		{
			var appName = Path.GetFileNameWithoutExtension(assembly.CodeBase);

			var appender = new RollingFileAppender
			{
				File = $"Logs\\{appName}.Rolling.log",
				AppendToFile = true,
				LockingModel = new FileAppender.MinimalLock(),
				RollingStyle = RollingFileAppender.RollingMode.Size,
				MaxSizeRollBackups = 5,
				MaximumFileSize = "10MB",
				StaticLogFileName = true,
				Layout = GetPattern()
			};
			appender.ActivateOptions();

			return appender;
		}

		private static IAppender GetMemoryAppender()
		{
			var memory = new MemoryAppender
			{
				Layout = GetPattern()
			};
			memory.ActivateOptions();
			return memory;
		}

		private static IAppender GetConsoleAppender()
		{
			var appender = new ConsoleAppender
			{
				Layout = GetPattern()
			};
			appender.ActivateOptions();
			return appender;
		}
	}
}
