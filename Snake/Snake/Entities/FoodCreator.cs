using System;
using System.Threading;

namespace Snake
{
	class FoodCreator
	{
		private char symbol;
		private int foodEaten;

		private Random random = new Random();

		public FoodCreator(char sym)
		{
			this.symbol = sym;
			foodEaten = -1;

			Console.SetCursorPosition(2, 1);
			Console.Write("Food eaten: ");
		}

		public Point CreateFood()
		{
			UpdateFoodCounter();
			int x = random.Next(2, Console.WindowWidth - 2);
			int y = random.Next(4, Console.WindowHeight - 2);
			return new Point(x, y, this.symbol);
		}

		private void UpdateFoodCounter()
		{
			foodEaten++;
			Console.SetCursorPosition(14, 1);
			Console.Write(foodEaten);
		}
	}
}
