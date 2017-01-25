using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Ikc5.Prism.Common.Logging;
using log4net;
using Category = Prism.Logging.Category;
using Priority = Prism.Logging.Priority;

namespace Ikc5.ScreenSaver.Logging
{
	public class Log4NetLogger : ILoggerTimeFacade
	{
		#region Fields

		// Member variables
		// TODO: update logger's type to type of declared/called method
		private readonly ILog _logger = LogManager.GetLogger(typeof(Log4NetLogger));

		#endregion

		#region ILoggerFacade Members

		/// <summary>
		/// Writes a log message.
		/// </summary>
		/// <param name="message">The message to write.</param>
		/// <param name="category">The message category.</param>
		/// <param name="priority">Not used by Log4Net; pass Priority.None.</param>
		public void Log(string message, Category category, Priority priority)
		{
			switch (category)
			{
			case Category.Debug:
				_logger.Debug(message);
				break;
			case Category.Warn:
				_logger.Warn(message);
				break;
			case Category.Exception:
				_logger.Error(message);
				break;
			case Category.Info:
			default:
				_logger.Info(message);
				break;
			}
		}

		#endregion

		#region ILoggerTimeFacade

		private readonly IDictionary<string, DateTime> _logTimes = new Dictionary<string, DateTime>();

		/// <summary>
		/// Writes a log message and make "start" point of the execution.
		/// </summary>
		/// <param name="message">The message to write.</param>
		/// <param name="category">The message category.</param>
		/// <param name="priority">Not used by Log4Net; pass Priority.None.</param>
		/// <param name="propertyName">Property or method name.</param>
		public void LogStart(string message, Category category, Priority priority, [CallerMemberName] string propertyName = null)
		{

			if (!string.IsNullOrEmpty(propertyName))
			{
				message = $"{propertyName}: {message}";
				_logTimes[propertyName] = DateTime.UtcNow;
			}
			Log(message, category, priority);
		}

		/// <summary>
		/// Writes a log message and make "start" point of the execution.
		/// </summary>
		/// <param name="message">The message to write.</param>
		/// <param name="category">The message category.</param>
		/// <param name="priority">Not used by Log4Net; pass Priority.None.</param>
		/// <param name="propertyName">Property or method name.</param>
		public void LogEnd(string message, Category category, Priority priority, [CallerMemberName] string propertyName = null)
		{
			if (string.IsNullOrEmpty(propertyName))
			{
				Log(message, category, priority);
			}
			else
			{
				var endTime = DateTime.UtcNow;
				Log($"{propertyName}: {message}", category, priority);

				if (_logTimes.ContainsKey(propertyName))
				{
					Log($"{propertyName}: start {_logTimes[propertyName]}, end {endTime}, duration {endTime - _logTimes[propertyName]}", category, priority);
					_logTimes.Remove(propertyName);
				}
			}
		}

		#endregion

	}

}