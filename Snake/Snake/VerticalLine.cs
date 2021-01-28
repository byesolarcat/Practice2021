=using System.Collections.Generic;

namespace Snake
{
	class VerticalLine
	{
		public List<Point> PointsList { get; set; }

		public VerticalLine(int yTop, int yBottom, int x, char symbol)
		{
			PointsList = new List<Point>();
			for (int y = yTop; y <= yBottom; y++)
			{
				Point p = new Point(x, y, symbol);
				PointsList.Add(p);
			}
		}

		public void Draw()
		{
			foreach (var point in PointsList)
			{
				point.Draw();
			}
		}
	}
}
