using System;
using Ikc5.ScreenSaver.Common.Models;
using Prism.Mvvm;

namespace Ikc5.ScreenSaver.Common.Views.ViewModels
{
	internal class DesignCell : BindableBase, ICell
	{
		public bool State { get; set; } = true;
	}
}
