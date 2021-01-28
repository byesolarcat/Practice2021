using System.Collections.Generic;

namespace Snake
{
	class HorizontalLine
	{
		public List<Point> pointsList { get; set; }

		public HorizontalLine(int xLeft, int xRight, int y, char symbol)
		{
			pointsList = new List<Point>();
			for (int x = xLeft; x <= xRight; x++)
			{
				Point p = new Point(x, y, symbol);
				pointsList.Add(p);
			}
		}

		public void Draw()
		{
			foreach (var point in pointsList)
			{
				point.Draw();
			}
		}
	}
}
