using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using Ikc5.TypeLibrary;
using Microsoft.Practices.Unity;
using WpfApplication.Models;
using Color = System.Windows.Media.Color;

namespace WpfApplication.ViewModels
{
	public class MainWindowModel : BaseNotifyPropertyChanged, IMainWindowModel
	{
		[InjectionConstructor]
		public MainWindowModel(Color backgroundColor, bool primary, string displayName)
		{
			this.SetDefaultValues();
			BackgroundColor = backgroundColor;

			_rectangles = new ObservableCollection<ScreenRectangle>(new[]
			{
				new ScreenRectangle(ScreenNames.View, ViewWidth, ViewHeight, "View uses border with BorderThickness='3', and its size is lesser than screen size at 6px at each dimension")
			});
			if( primary)
			{
				_rectangles.Add(new ScreenRectangle(ScreenNames.PrimaryScreen,
					(float)SystemParameters.PrimaryScreenWidth, (float)SystemParameters.PrimaryScreenHeight, "'Emulated' screen dimensions for Wpf applications"));
				_rectangles.Add(new ScreenRectangle(ScreenNames.FullPrimaryScreen,
					(float) SystemParameters.FullPrimaryScreenWidth, (float) SystemParameters.FullPrimaryScreenHeight, "Height difference with working area height depends on locale"));
				_rectangles.Add(new ScreenRectangle(ScreenNames.VirtualScreen,
					(float) SystemParameters.VirtualScreenLeft, (float) SystemParameters.VirtualScreenTop,
					(float) SystemParameters.VirtualScreenWidth, (float) SystemParameters.VirtualScreenHeight));
				_rectangles.Add(new ScreenRectangle(ScreenNames.WorkingArea,
					SystemParameters.WorkArea.Width, SystemParameters.WorkArea.Height, "40px is holded for the taskbar height"));
				_rectangles.Add(new ScreenRectangle(ScreenNames.PrimaryWorkingArea,
					Screen.PrimaryScreen.WorkingArea.Left, Screen.PrimaryScreen.WorkingArea.Top,
					Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height));
			}

			foreach (var screeen in Screen.AllScreens)
			{
				if (!primary && !Equals(screeen.DeviceName, displayName))
					continue;
				_rectangles.Add(new ScreenRectangle($"Screen \"{screeen.DeviceName}\"", screeen.WorkingArea, "Physical dimensions"));
			}
		}

		/// <summary>
		/// Add or update screen rectangle for the view.
		/// </summary>
		private void AddOrUpdateViewRectangle()
		{
			if (Rectangles == null)
				return;
			var screenRectangle = Rectangles.FirstOrDefault(item => ScreenNames.View.Equals(item.Name));
			if (screenRectangle == null)
			{
				screenRectangle = new ScreenRectangle(ScreenNames.View, ViewWidth, ViewHeight);
				Rectangles.Add(screenRectangle);
			}
			else
			{
				screenRectangle.SetSize(ViewWidth, ViewHeight);
			}
		}

		#region IMainWindowModel

		private Color _backgroundColor;
		private double _viewWidth;
		private double _viewHeight;
		private ObservableCollection<ScreenRectangle> _rectangles;

		/// <summary>
		/// Background color.
		/// </summary>
		[DefaultValue(typeof(Color), "#FFD3D3D3")]
		public Color BackgroundColor
		{
			get { return _backgroundColor; }
			private set { SetProperty(ref _backgroundColor, value); }
		}

		/// <summary>
		/// Width of the view.
		/// </summary>
		[DefaultValue(0)]
		public double ViewWidth
		{
			get { return _viewWidth; }
			set
			{
				SetProperty(ref _viewWidth, value);
				AddOrUpdateViewRectangle();
			}
		}

		/// <summary>
		/// Height of the view.
		/// </summary>
		[DefaultValue(0)]
		public double ViewHeight
		{
			get { return _viewHeight; }
			set
			{
				SetProperty(ref _viewHeight, value);
				AddOrUpdateViewRectangle();
			}
		}

		/// <summary>
		/// Set of rectangles.
		/// </summary>
		public ObservableCollection<ScreenRectangle> Rectangles
		{
			get { return _rectangles; }
			private set { SetProperty(ref _rectangles, value); }
		}

		#endregion IMainWindowModel

	}
}