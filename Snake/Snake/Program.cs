using System;
using System.Collections.Generic;
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

			Snake snk = new Snake(new Point(4, 5, '*'), 4, Direction.Right);
			snk.Draw();

			FoodCreator foodCreator = new FoodCreator('@');
			Point food = foodCreator.CreateFood();
			food.Draw();

			while (true)
			{
				if (walls.IsHit(snk) || snk.isHitTail())
				{
					break;
				}
				if (snk.Eat(food))
				{
					food = foodCreator.CreateFood();
					food.Draw();
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
	}
}
