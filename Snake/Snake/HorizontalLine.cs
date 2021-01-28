using System.Collections.Generic;

namespace Snake
{
	class HorizontalLine : Figure
	{
		public HorizontalLine(int xLeft, int xRight, int y, char symbol)
		{
			PointsList = new List<Point>();
			for (int x = xLeft; x <= xRight; x++)
			{
				Point p = new Point(x, y, symbol);
				PointsList.Add(p);
			}
		}
	}
}
