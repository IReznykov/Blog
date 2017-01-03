using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfApplication.Views
{
	/// <summary>
	/// Interaction logic for CellControl
	/// </summary>
	public partial class CellView : UserControl
	{
		public CellView()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Start color - the darkest color of the cell.
		/// </summary>
		public Color StartColor
		{
			get { return (Color)GetValue(StartColorProperty); }
			set { SetValue(StartColorProperty, value); }
		}

		public static readonly DependencyProperty StartColorProperty =
			DependencyProperty.Register(
				"StartColor",
				typeof(Color),
				typeof(CellView));

		/// <summary>
		/// Finish color - the lightest color of the cell.
		/// </summary>
		public Color FinishColor
		{
			get { return (Color)GetValue(FinishColorProperty); }
			set { SetValue(FinishColorProperty, value); }
		}

		public static readonly DependencyProperty FinishColorProperty =
			DependencyProperty.Register(
				"FinishColor",
				typeof(Color),
				typeof(CellView));

		/// <summary>
		/// Border color.
		/// </summary>
		public Color BorderColor
		{
			get { return (Color)GetValue(BorderColorProperty); }
			set { SetValue(BorderColorProperty, value); }
		}

		public static readonly DependencyProperty BorderColorProperty =
			DependencyProperty.Register(
				"BorderColor",
				typeof(Color),
				typeof(CellView));

	}

}
