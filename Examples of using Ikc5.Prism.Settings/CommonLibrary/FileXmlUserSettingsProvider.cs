using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ikc5.Prism.Settings;
using Ikc5.Prism.Settings.Providers;
using Ikc5.TypeLibrary;
using Prism.Logging;

namespace CommonLibrary
{
    public class FileXmlUserSettingsProvider<T> : LocalXmlUserSettingsProvider<T> where T : class, IUserSettings
	{
	    public FileXmlUserSettingsProvider(ILiteObjectService liteObjectService, ILoggerFacade logger)
			: base(liteObjectService, logger)
	    {
			var assembly = Assembly.GetEntryAssembly();
			FolderName = Path.Combine(Path.GetDirectoryName(assembly.Location) ?? "", "NewSettings");
		}
	}
}
