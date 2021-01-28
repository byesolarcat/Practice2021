using System;
using System.Collections.Generic;
using System.Text;

namespace Snake
{
	class Point
	{
		public int X { get; set; }
		public int Y { get; set; }

		public char Symbol { get; set; }

		public Point(int x, int y, char symbol)
		{
			X = x;
			Y = y;
			Symbol = symbol;
		}

		public void Draw()
		{
			Console.SetCursorPosition(this.X, this.Y);
			Console.Write(this.Symbol);
		}
	}
}
