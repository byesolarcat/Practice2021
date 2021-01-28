using System;

namespace Snake
{
	class FoodCreator
	{
		private char symbol;

		private Random random = new Random();
		public FoodCreator(char sym)
		{
			this.symbol = sym;
		}

		public Point CreateFood()
		{
			int x = random.Next(2, Console.WindowWidth - 2);
			int y = random.Next(4, Console.WindowHeight - 2);
			return new Point(x, y, this.symbol);
		}
	}
}
