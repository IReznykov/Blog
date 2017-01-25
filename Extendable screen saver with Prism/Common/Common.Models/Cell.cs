using System.ComponentModel;
using Ikc5.TypeLibrary;

namespace Ikc5.ScreenSaver.Common.Models
{
	public class Cell : BaseNotifyPropertyChanged, ICell
	{
		public Cell()
		{
			this.SetDefaultValues();
		}

		public Cell(bool state)
			: this()
		{
			State = state;
		}

		#region Implementation ICell

		private bool _state;

		[DefaultValue(false)]
		public bool State
		{
			get { return _state; }
			set { SetProperty(ref _state, value); }
		}

		#endregion

		public static ICell Empty => new Cell();
	}
}
