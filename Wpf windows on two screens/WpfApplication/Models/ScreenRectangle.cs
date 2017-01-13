using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ikc5.TypeLibrary;

namespace WpfApplication.Models
{
	public class ScreenRectangle : BaseNotifyPropertyChanged
	{
		protected ScreenRectangle()
		{
			Name = string.Empty;
			Bounds = new RectangleF();
		}

		public ScreenRectangle(string name, RectangleF bounds)
		{
			Name = name;
			Bounds = bounds;
		}

		public ScreenRectangle(string name, double left, double top, double width, double height)
			: this(name, new RectangleF((float)left, (float)top, (float)width, (float)height))
		{
		}

		public ScreenRectangle(string name, double width, double height)
			: this(name, new RectangleF(0, 0, (float)width, (float)height))
		{
		}

		#region Public properties

		private string _name;
		private RectangleF _bounds;

		public string Name
		{
			get { return _name; }
			set { SetProperty(ref _name, value); }
		}

		public RectangleF Bounds
		{
			get { return _bounds; }
			set { SetProperty(ref _bounds, value); }
		}

		#endregion Public properties

		public void SetSize(double width, double height)
		{
			Bounds = new RectangleF(Bounds.Location, new SizeF((float)width, (float)height));
		}

	}
}
