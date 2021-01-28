using System;
using System.Threading;

namespace Snake
{
	internal class Program
	{
		static void Main(string[] args)
		{
			InitGameField();

			StartGame();
		}

		static void InitGameField()
		{
			Console.SetWindowSize(121, 30);

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

			topBorder.Draw();
			bottomBorder.Draw();
			scoreBoardBorder.Draw();
			leftBorder.Draw();
			rightBorder.Draw();
		}

		static void StartGame()
		{
			Snake snk = new Snake(new Point(4, 5, '*'), 4, Direction.Right);
			snk.Draw();

			FoodCreator foodCreator = new FoodCreator('@');
			Point food = foodCreator.CreateFood();
			food.Draw();

			while (true)
			{
				if (snk.Eat(food))
				{
					food = foodCreator.CreateFood();
					food.Draw();
				}
				if (Console.KeyAvailable)
				{
					ConsoleKeyInfo key = Console.ReadKey();
					snk.SetMovementDirection(key);
				}
				Thread.Sleep(128);
				snk.Move();
			}
		}
	}
}
