using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Ikc5.ScreenSaver.Common.Models
{
	/// <summary>
	/// Model for dynamic grid
	/// </summary>
	public class CellSet
	{
		public CellSet(int width, int height)
		{
			// cell set should has valid size
			Width = System.Math.Max(width, 1);
			Height = System.Math.Max(height, 1);

			// init cell array with empty values
			Init(new Size(Width, Height));
		}

		private ICell[,] Cells { get; set; }

		public int Width { get; }
		public int Height { get; }

		public ICell GetCell(int x, int y)
		{
			if (x >= 0 && x < Width && y >= 0 && y < Height)
				return Cells[x, y];
			return null;
		}

		/// <summary>
		/// Method returns current points.
		/// </summary>
		public IEnumerable<Point> GetPoints()
		{
			var points = new List<Point>();

			for (var x = 0; x < Width; x++)
				for (var y = 0; y < Height; y++)
				{
					if (Cells[x, y].State)
						points.Add(new Point(x, y));
				}

			return points;
		}

		/// <summary>
		/// Method sets initial points.
		/// </summary>
		/// <param name="newPoints">Set of new cells that are added to set.</param>
		public void SetPoints(IEnumerable<Point> newPoints)
		{
			if (newPoints == null)
				return;

			foreach (var point in newPoints.Where(point => !Cells[point.X, point.Y].State))
			{
				Cells[point.X, point.Y].State = true;
			}
		}

		/// <summary>
		/// Method invertss cells.
		/// </summary>
		/// <param name="newPoints">Set of cells that are should be initiated.</param>
		public void InvertPoints(IEnumerable<Point> newPoints)
		{
			if (newPoints == null)
				return;

			foreach (var point in newPoints)
			{
				Cells[point.X, point.Y].State = !Cells[point.X, point.Y].State;
			}
		}

		/// <summary>
		/// Remove all cells.
		/// </summary>
		public void Clear()
		{
			for (var x = 0; x < Width; x++)
				for (var y = 0; y < Height; y++)
				{
					Cells[x, y].State = false;
				}
		}

		/// <summary>
		/// Initialization of _cells.
		/// </summary>
		/// <param name="size"></param>
		private void Init(Size size)
		{
			Cells = new ICell[size.Width, size.Height];

			for (var x = 0; x < size.Width; x++)
				for (var y = 0; y < size.Height; y++)
				{
					Cells[x, y] = new Cell();
				}
		}
	}
}
