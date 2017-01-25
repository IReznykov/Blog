using Ikc5.ScreenSaver.Common.Models;
using Prism.Commands;
using Prism.Mvvm;

namespace Ikc5.ScreenSaver.Models
{
	public class CommandProvider : BindableBase, ICommandProvider
	{
		public CompositeCommand IterateCommand { get; }
			= new CompositeCommand(monitorCommandActivity: true);
		public CompositeCommand StartIteratingCommand { get; }
			= new CompositeCommand(monitorCommandActivity: true);
		public CompositeCommand StopIteratingCommand { get; }
			= new CompositeCommand(monitorCommandActivity: true);
		public CompositeCommand RestartCommand { get; }
			= new CompositeCommand(monitorCommandActivity: true);
	}
}
