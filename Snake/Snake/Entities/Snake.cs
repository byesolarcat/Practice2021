using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snake
{
	class Snake : Figure
	{
		public Direction MovementDirection { get; set; }

		public Snake(Point tail, int length, Direction direction )
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
