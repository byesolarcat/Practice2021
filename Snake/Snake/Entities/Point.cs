using System;

namespace Snake
{
	class Point
	{
		public int X { get; set; }
		public int Y { get; set; }

		public char Symbol { get; set; }

		public Point(int x, int y, char symbol)
		{
			this.X = x;
			this.Y = y;
			this.Symbol = symbol;
		}

		public Point(Point p)
		{
			this.X = p.X;
			this.Y = p.Y;
			this.Symbol = p.Symbol;
		}

		public void Draw()
		{
			Console.SetCursorPosition(this.X, this.Y);
			Console.Write(this.Symbol);
		}

		public void Move(int offset, Direction direction)
		{
			switch (direction)
			{
				case Direction.Right:
					{
						X += offset;
						break;
					}
				case Direction.Left:
					{
						X -= offset;
						break;
					}
				case Direction.Up:
					{
						Y -= offset;
						break;
					}
				case Direction.Down:
					{
						Y += offset;
						break;
					}
			}
		}
	}
}

