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
		}

		static void StartGame()
		{
			Walls walls = new Walls();
			Snake snk = new Snake(new Point(4, 5, '*'), 9, Direction.Right);
			FoodCreator foodCreator = new FoodCreator('@');
			Point food = foodCreator.CreateFood();

			snk.Draw();
			walls.Draw();
			food.Draw();

			Console.SetCursorPosition(2, 1);
			Console.Write("Food eaten: {0}", snk.FoodEaten);

			while (true)
			{
				if (walls.IsHit(snk) || snk.isHitTail())
				{
					InitGameOver();
					break;
				}
				if (snk.Eat(food))
				{
					food = foodCreator.CreateFood();
					food.Draw();
					Console.SetCursorPosition(14, 1);
					Console.Write(snk.FoodEaten);
				}
				if (Console.KeyAvailable)
				{
					snk.SetMovementDirection(Console.ReadKey());
				}
				Thread.Sleep(150);
				snk.Move();
			}
		}

		static void InitGameOver()
		{
			Thread.Sleep(1000);
			PrintGameOver();
			ConsoleKeyInfo key = Console.ReadKey();
			while (true)
			{
				switch (key.KeyChar)
				{
					case 'Y':
						{
							Console.Clear();
							StartGame();
							break;
						}
					case 'y':
						{
							goto case 'Y';
						}
					default:
						{
							break;
						}
				}
				break;
			}
		}
		static void PrintGameOver()
		{
			Console.Clear();
			Console.SetCursorPosition(Console.WindowWidth / 2 - 6, Console.WindowHeight / 2);
			Console.Write("Game Over!");
			Console.SetCursorPosition(Console.WindowWidth / 2 - 18, Console.WindowHeight / 2 + 1);
			Console.Write("Press 'Y' if you want to restart\n");
		}
	}
}
