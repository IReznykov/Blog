using System;
using System.Windows.Controls;
using Ikc5.ScreenSaver.FirstModule.ViewModels;
using Prism;

namespace Ikc5.ScreenSaver.FirstModule.Views
{
	/// <summary>
	/// Presentation of dynamical data structure:
	/// https://ireznykov.com/2017/01/04/grid-with-dynamic-number-of-rows-and-columns-part-2/
	/// </summary>
	public partial class MainView : UserControl, IActiveAware
	{
		public MainView()
		{
			Loaded += MainView_Loaded;
			InitializeComponent();
		}

		private void MainView_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			(DataContext as IMainViewModel)?.StartIteratingCommand.Execute(null);
		}

		#region IActiveAware implementation

		private bool _isActive;

		public bool IsActive
		{
			get { return _isActive; }
			set
			{
				if (_isActive == value)
					return;
				_isActive = value;

				var viewModelActiveAware = DataContext as IActiveAware;
				if (viewModelActiveAware != null)
					viewModelActiveAware.IsActive = value;

				OnIsActiveChanged();
			}
		}

		public event EventHandler IsActiveChanged = delegate { };

		protected virtual void OnIsActiveChanged()
		{
			IsActiveChanged.Invoke(this, EventArgs.Empty);
		}

		#endregion
	}
}
