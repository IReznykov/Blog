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

		public ScreenRectangle(string name, RectangleF bounds, string description = null)
		{
			Name = name;
			Bounds = bounds;
			Description = description;
		}

		public ScreenRectangle(string name, double left, double top, double width, double height, string description = null)
			: this(name, new RectangleF((float)left, (float)top, (float)width, (float)height), description)
		{
		}

		public ScreenRectangle(string name, double width, double height, string description = null)
			: this(name, new RectangleF(0, 0, (float)width, (float)height), description)
		{
		}

		#region Public properties

		private string _name;
		private string _description;
		private RectangleF _bounds;

		public string Name
		{
			get { return _name; }
			set { SetProperty(ref _name, value); }
		}

		public string Description
		{
			get { return _description; }
			set { SetProperty(ref _description, value); }
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
