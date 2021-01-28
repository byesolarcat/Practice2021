using System;
using System.Collections.Generic;
using System.Text;

namespace Snake
{
	class Walls : Figure
	{
		private List<Figure> wallList;
		public Walls()
		{
			wallList = new List<Figure>();

			HorizontalLine topBorder =
				new HorizontalLine(Console.WindowLeft, Console.WindowWidth - 1, Console.WindowTop, '—');
			HorizontalLine bottomBorder =
				new HorizontalLine(Console.WindowLeft, Console.WindowWidth - 1, Console.WindowHeight - 1, '—');
			HorizontalLine scoreBoardBorder =
				new HorizontalLine(Console.WindowLeft, Console.WindowWidth - 1, Console.WindowTop + 2, '—');
			VerticalLine leftBorder =
				new VerticalLine(Console.WindowTop, Console.WindowHeight - 1, Console.WindowLeft, '|');
			VerticalLine rightBorder =
				new VerticalLine(Console.WindowTop, Console.WindowHeight - 1, Console.WindowWidth - 1, '|');

			wallList.Add(topBorder);
			wallList.Add(bottomBorder);
			wallList.Add(scoreBoardBorder);
			wallList.Add(leftBorder);
			wallList.Add(rightBorder);
		}

		public bool IsHit(Figure figure)
		{
			foreach (var wall in wallList)
			{
				if (wall.IsHit(figure))
					return true;
			}
			return false;
		}

		public override void Draw()
		{
			foreach (var wall in wallList)
			{
				wall.Draw();
			}
		}
	}
}
