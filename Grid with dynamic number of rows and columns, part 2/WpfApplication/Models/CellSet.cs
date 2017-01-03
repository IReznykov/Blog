using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication.Models
{
	/// <summary>
	/// Model for dynamic grid
	/// </summary>
	public class CellSet
	{
		public CellSet(int width, int height)
		{
			// cell set should has valid size
			Width = Math.Max(width, 3);
			Height = Math.Max(height, 3);

			// init cell array with empty values
			Cells = new ICell[Width, Height];
			for (var x = 0; x < Width; x++)
				for (var y = 0; y < Height; y++)
				{
					Cells[x, y] = new Cell(false);
				}
		}

		public int Width { get; }
		public int Height { get; }
		private ICell[,] Cells { get; }

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

	}
}
