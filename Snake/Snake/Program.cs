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
			walls.Draw();

			Console.SetCursorPosition(2, 1);
			Console.Write("Food eaten: 0");
			int foodEaten = 0;

			Snake snk = new Snake(new Point(4, 5, '*'), 9, Direction.Right);
			snk.Draw();

			FoodCreator foodCreator = new FoodCreator('@');
			Point food = foodCreator.CreateFood();
			food.Draw();

			while (true)
			{
				if (walls.IsHit(snk) || snk.isHitTail())
				{
					PrintGameOver();
					ConsoleKeyInfo key = Console.ReadKey();
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
				if (snk.Eat(food))
				{
					foodEaten++;
					food = foodCreator.CreateFood();
					food.Draw();
					Thread.Sleep(10);
					Console.SetCursorPosition(14, 1);
					Console.Write(foodEaten);
					
				}
				Thread.Sleep(50);
				if (Console.KeyAvailable)
				{
					ConsoleKeyInfo key = Console.ReadKey();
					snk.SetMovementDirection(key);
				}
				Thread.Sleep(50);
				snk.Move();
			}
		}

		static void PrintGameOver()
		{
			Console.Clear();
			Console.SetCursorPosition(Console.WindowWidth / 2 - 6, Console.WindowHeight / 2);
			Console.Write("Game Over!");
			Console.SetCursorPosition(Console.WindowWidth / 2 - 18, Console.WindowHeight / 2 + 1);
			Console.Write("Press 'Y' if you want to restart");
		}
	}
}
