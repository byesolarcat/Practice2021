using System;
using System.Collections.Generic;
using System.Linq;

namespace Snake
{
	class Snake : Figure
	{
		private Direction MovementDirection { get; set; }

		public Snake(Point tail, int length, Direction direction)
		{
			this.MovementDirection = direction;
			PointsList = new List<Point>();
			for (int i = 0; i < length; i++)
			{
				Point p = new Point(tail);
				p.Move(i, direction);
				this.PointsList.Add(p);
			}

		}

		public void Move()
		{
			Point tail = PointsList.First();
			PointsList.Remove(tail);
			Point head = GetNextPoint();
			PointsList.Add(head);

			tail.Clear();
			head.Draw();
		}

		public bool isHitTail()
		{
			var head = PointsList.Last();
			for (int i = 0; i < PointsList.Count - 2; i++)
			{
				if (head.IsHit(PointsList[i]))
					return true;
			}
			return false;
		}

		public bool Eat(Point food)
		{
			Point head = GetNextPoint();
			if (head.IsHit(food))
			{
				food.Symbol = head.Symbol;
				PointsList.Add(food);
				return true;
			}
			return false;
		}

		private Point GetNextPoint()
		{
			Point head = PointsList.Last();
			Point nextPoint = new Point(head);
			nextPoint.Move(1, MovementDirection);
			return nextPoint;
		}

		public void SetMovementDirection(ConsoleKeyInfo key)
		{
			switch (key.Key)
			{
				case ConsoleKey.LeftArrow:
					{
						this.MovementDirection = Direction.Left;
						break;
					}
				case ConsoleKey.RightArrow:
					{
						this.MovementDirection = Direction.Right;
						break;
					}
				case ConsoleKey.UpArrow:
					{
						this.MovementDirection = Direction.Up;
						break;
					}
				case ConsoleKey.DownArrow:
					{
						this.MovementDirection = Direction.Down;
						break;
					}
			}
		}
	}
}
