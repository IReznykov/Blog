using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfApplication.Views
{
	/// <summary>
	/// Interaction logic for CellControl
	/// </summary>
	public partial class CellControl : UserControl
	{
		public CellControl()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Start brush for gradient filling - the most light color of the cell.
		/// Default was #FFE0E1F0
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
				typeof(CellControl),
				new PropertyMetadata(Color.FromArgb(255, Colors.WhiteSmoke.R, Colors.WhiteSmoke.G, Colors.WhiteSmoke.B)));

		/// <summary>
		/// Start Color for gradient filling - the most light color of the cell.
		/// Default was #FF000766
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
				typeof(CellControl),
				new PropertyMetadata(Color.FromArgb(255, Colors.DarkBlue.R, Colors.DarkBlue.G, Colors.DarkBlue.B)));

	}
}
