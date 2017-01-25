using System.ComponentModel;
using Prism.Commands;

namespace Ikc5.ScreenSaver.Common.Models
{
	public interface ICommandProvider : INotifyPropertyChanged
	{
		CompositeCommand IterateCommand { get; }
		CompositeCommand StartIteratingCommand { get; }
		CompositeCommand StopIteratingCommand { get; }
		CompositeCommand RestartCommand { get; }

	}
}
